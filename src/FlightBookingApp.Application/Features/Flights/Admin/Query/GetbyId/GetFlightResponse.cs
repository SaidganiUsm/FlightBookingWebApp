using FlightBookingApp.Application.Common.DTOs;

namespace FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId
{
    public class GetFlightResponse
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public LocationDto? DepartureLocation { get; set; }
        public LocationDto? DestinationLocation { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsAvailable { get; set; }
    }
}
