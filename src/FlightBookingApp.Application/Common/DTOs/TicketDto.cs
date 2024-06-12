using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Common.DTOs
{
    public class TicketDto
    {
        public double? Price { get; set; }

        public Flight? Flight { get; set; }

        public User? User { get; set; }

        public Rank? Rank { get; set; }
    }
}
