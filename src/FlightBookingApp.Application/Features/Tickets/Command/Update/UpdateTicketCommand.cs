using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Update
{
    public class UpdateTicketCommand : IRequest<UpdateTicketResponse>
    {
        public int Id { get; set; }
        public string? TicketRank { get; set; }
    }

    public class UpdateTicketCommandHandler
        : IRequestHandler<UpdateTicketCommand, UpdateTicketResponse>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IFlightRepository _flightRepository;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IRankRepository rankRepository,
            IFlightRepository flightRepository
        )
        {
            _ticketRepository = ticketRepository;
            _rankRepository = rankRepository;
            _flightRepository = flightRepository;
        }

        public async Task<UpdateTicketResponse> Handle(
            UpdateTicketCommand request, 
            CancellationToken cancellationToken
        )
        {
            var ticket = await _ticketRepository.GetAsync(
                x => x.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var flight = await _flightRepository.GetAsync(
                x => x.Id == ticket!.FlightId,
                cancellationToken: cancellationToken
            );

            if (flight == null)
                return null;

            var newRank = await _rankRepository.GetAsync(
                x => x.RankName == request.TicketRank,
            cancellationToken: cancellationToken
            );

            Rank? oldRank = ticket!.Rank;

            if (oldRank != null && newRank != null && oldRank.Id != newRank.Id)
            {
                switch (oldRank.RankName)
                {
                    case "FirstClass":
                        flight.FirstClassTicketsAmount += 1;
                        break;
                    case "Business":
                        flight.BusinessTicketsAmount += 1;
                        break;
                    case "Economy":
                        flight.EconomyTicketsAmount += 1;
                        break;
                }

                switch (newRank.RankName)
                {
                    case "FirstClass":
                        flight.FirstClassTicketsAmount -= 1;
                        break;
                    case "Business":
                        flight.BusinessTicketsAmount -= 1;
                        break;
                    case "Economy":
                        flight.EconomyTicketsAmount -= 1;
                        break;
                }
            }

            var price = newRank!.RankPriceRatio * flight!.StandartPriceForFlight;

            ticket!.Rank = newRank;

            return new UpdateTicketResponse { Id = request.Id };
        }
    }
}
