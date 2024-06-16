using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using MediatR;

namespace FlightBookingApp.Application.Features.Tickets.Command.Update
{
    public class UpdateTicketCommand : IRequest<UpdateTicketResponse>
    {
        public int Id { get; set; }
        public TicketRanks TicketRank { get; set; }
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

            var newRank = await _rankRepository.GetAsync(
                x => x.RankName == request.TicketRank.ToString(),
            cancellationToken: cancellationToken
            );

            var oldRank = ticket!.Rank;

            if (oldRank != null && oldRank.Id != newRank!.Id)
            {
                AdjustTicketAvailability(flight, oldRank, 1);
                AdjustTicketAvailability(flight, newRank, -1);
            }

            var price = newRank!.RankPriceRatio * flight!.StandartPriceForFlight;

            ticket!.Rank = newRank;

            return new UpdateTicketResponse { Id = request.Id };
        }

        private void AdjustTicketAvailability(Flight flight, Rank rank, int adjustment)
        {
            if (Enum.TryParse(rank.RankName, out TicketRanks ticketRank))
            {
                switch (ticketRank)
                {
                    case TicketRanks.FirstClass:
                        flight.FirstClassTicketsAmount += adjustment;
                        break;
                    case TicketRanks.Business:
                        flight.BusinessTicketsAmount += adjustment;
                        break;
                    case TicketRanks.Economy:
                        flight.EconomyTicketsAmount += adjustment;
                        break;
                }
            }
        }
    }
}
