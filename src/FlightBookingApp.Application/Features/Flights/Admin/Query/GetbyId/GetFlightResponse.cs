using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId
{
    public class GetFlightResponse
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Location? DepartureLocation { get; set; }
        public Location? DestinationLocation { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsAvailable { get; set; }
    }
}
