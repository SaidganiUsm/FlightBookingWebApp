using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateFlightCommand : IRequest<CreateFlightResponse>
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DepartureLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsAvailable { get; set; }
    }

    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, CreateFlightResponse>
    {
        public async Task<CreateFlightResponse> Handle(
            CreateFlightCommand request,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}
