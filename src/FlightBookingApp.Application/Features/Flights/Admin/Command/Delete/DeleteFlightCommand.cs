using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Enums;
using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Delete
{
    public class DeleteFlightCommand : IRequest<DeleteFlightResponse>
    {
        public int Id { get; set; }
    }

    public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, DeleteFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IFlightStatusRepository _flightStatusRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketStatusRepository _ticketStatusRepository;

        public DeleteFlightCommandHandler(
            IFlightRepository flightRepository, 
            IFlightStatusRepository flightStatusRepository,
            ITicketRepository ticketRepository,
            ITicketStatusRepository ticketStatusRepository
        )
        {
            _flightRepository = flightRepository;
            _flightStatusRepository = flightStatusRepository;
            _ticketRepository = ticketRepository;
            _ticketStatusRepository = ticketStatusRepository;
        }

        public async Task<DeleteFlightResponse> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var status = await _flightStatusRepository.GetAsync(
                s => s.Name == FlightStatuses.Cancelled.ToString(),
                cancellationToken: cancellationToken
            ); 

            var flight = await _flightRepository.GetAsync(
                f => f.Id == request.Id, 
                cancellationToken: cancellationToken
            );

            if (flight == null)
                return null!;

            var tickets = await _ticketRepository.GetUnpaginatedListAsync(
                x => x.FlightId == flight.Id && 
                x.TicketStatus!.Name == TicketStatuses.Booked.ToString(),
                cancellationToken: cancellationToken 
            );

            var ticketStatus = await _ticketStatusRepository.GetAsync(
                x => x.Name == FlightStatuses.Cancelled.ToString(),
                cancellationToken: cancellationToken
            );

            foreach ( var ticket in tickets)
            {
                ticket.TicketStatus = ticketStatus;
                await _ticketRepository.UpdateAsync( ticket );
            }

            flight.FlightStatus = status;

            await _flightRepository.UpdateAsync(flight);

            return new DeleteFlightResponse { Id = flight.Id};
        }
    }
}
