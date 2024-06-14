using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Create
{
    public class CreateTicketCommand : IRequest<CreateTicketResponse>
    {
        public int FlightId { get; set; }
        public string? TicketRank { get; set; }
    }

    public class CreateTicketCommandHandler 
        : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketStatusRepository _ticketStatusRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly UserManager<User> _userManager;

        public CreateTicketCommandHandler(
            ICurrentUserService currentUserService,
            ITicketRepository ticketRepository,
            ITicketStatusRepository ticketStatusRepository,
            IRankRepository rankRepository,
            IFlightRepository flightRepository,
            UserManager<User> userManager
        )
        {
            _currentUserService = currentUserService;
            _ticketRepository = ticketRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _rankRepository = rankRepository;
            _flightRepository = flightRepository;
            _userManager = userManager;
        }

        public async Task<CreateTicketResponse> Handle(
            CreateTicketCommand request, 
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(
                u => u.Email == _currentUserService.UserEmail! && !u.IsDeleted,
                cancellationToken: cancellationToken
            );

            if (user == null)
                return null;

            var flight = await _flightRepository.GetAsync(
                s => s.Id == request.FlightId,
                cancellationToken: cancellationToken
            );

            var defaultStatus = await _ticketStatusRepository.GetAsync(
                s => s.Name == TicketStatuses.Booked.ToString(),
                cancellationToken: cancellationToken
            );

            var ticketRank = await _rankRepository.GetAsync(
                s => s.RankName == request.TicketRank,
                cancellationToken: cancellationToken
            );

            var price = ticketRank!.RankPriceRatio * flight!.StandartPriceForFlight;

            var ticket = new Ticket
            {
                Flight = flight,
                UserId = user.Id,
                Price = price,
                Rank = ticketRank,
                TicketStatus = defaultStatus,
            };

            switch (request.TicketRank)
            {
                case "FirstClass":
                    if (flight.FirstClassTicketsAmount <= 0)
                        throw new ValidationException("No any first class tickets left");
                    flight.FirstClassTicketsAmount -= 1;
                    break;
                case "Business":
                    if (flight.BusinessTicketsAmount <= 0)
                        throw new ValidationException("No any business class tickets left");
                    flight.BusinessTicketsAmount -= 1;
                    break;
                case "Economy":
                    if (flight.EconomyTicketsAmount <= 0)
                        throw new ValidationException("No any econom class tickets left");
                    flight.EconomyTicketsAmount -= 1;
                    break;
            }

            await _ticketRepository.AddAsync(ticket);

            flight!.TicketsAvailable -= 1;

            await _flightRepository.UpdateAsync(flight);

            return new CreateTicketResponse { Id = ticket.Id };
        }
    }
}
