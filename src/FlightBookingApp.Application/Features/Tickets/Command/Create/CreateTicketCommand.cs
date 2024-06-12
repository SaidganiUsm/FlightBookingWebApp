using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Features.Tickets.Command.BaseValidator;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Create
{
    public class CreateTicketCommand : IRequest<CreateTicketResponse>, ITicketCommandValidator
    {
        public int FlightId { get; set; }
        public string? TicketRank { get; set; }
        public double? Price { get; set; }
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

            var ticket = new Ticket
            {
                Flight = flight,
                User = user,
                Price = request.Price,
                Rank = ticketRank,
                TicketStatus = defaultStatus,
            };
            
            await _ticketRepository.AddAsync(ticket);

            return new CreateTicketResponse { Id = ticket.Id };
        }
    }
}
