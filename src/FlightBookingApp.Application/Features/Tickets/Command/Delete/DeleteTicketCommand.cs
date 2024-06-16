﻿using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Delete
{
	public class DeleteTicketCommand : IRequest<DeleteTicketResponse>
	{
		public int Id { get; set; }
	}

	public class DeleteTicketCommandHandler
		: IRequestHandler<DeleteTicketCommand, DeleteTicketResponse>
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly ITicketStatusRepository _ticketStatusRepository;
		private readonly IFlightRepository _flightRepository;

        public DeleteTicketCommandHandler(
			ITicketRepository ticketRepository,
			ITicketStatusRepository ticketStatusRepository,
			IFlightRepository flightRepository
		)
        {
            _ticketRepository = ticketRepository;
			_ticketStatusRepository = ticketStatusRepository;
			_flightRepository = flightRepository;
        }

        public async Task<DeleteTicketResponse> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
		{
			var ticket = await _ticketRepository.GetAsync(
				t => t.Id == request.Id,
				include: x => x.Include(x => x.Rank!),
				cancellationToken: cancellationToken
			);

			var status = await _ticketStatusRepository.GetAsync(
				s => s.Name == TicketStatuses.Cancelled.ToString(),
				cancellationToken: cancellationToken
			);

			ticket!.TicketStatus = status;

			var flight = await _flightRepository.GetAsync(
				x => x.Id == ticket.FlightId,
				cancellationToken: cancellationToken
			);

            AdjustTicketAvailability(flight!, ticket.Rank!, 1);

            flight!.TicketsAvailable += 1;

			await _flightRepository.UpdateAsync(flight);

			return new DeleteTicketResponse { Id = request.Id };
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
