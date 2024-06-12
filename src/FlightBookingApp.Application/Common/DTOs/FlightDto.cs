using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Common.DTOs
{
    public class FlightDto
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public FlightStatus FlightStatus { get; set; }

        public Location? DepartureLocation { get; set; }

        public Location? DestinationLocation { get; set; }

        public int TotalTickets { get; set; }

        public int TicketsAvailable { get; set; }
    }
}
