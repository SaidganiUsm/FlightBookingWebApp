using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Common.DTOs
{
    public class FlightDto
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public FlightStatusDto? FlightStatus { get; set; }

        public LocationDto? DepartureLocation { get; set; }

        public LocationDto? DestinationLocation { get; set; }

        public int TotalTickets { get; set; }

        public int TicketsAvailable { get; set; }
    }
}
