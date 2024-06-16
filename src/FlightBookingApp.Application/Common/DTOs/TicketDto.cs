using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Common.DTOs
{
    public class TicketDto
    {
        public double? Price { get; set; }

        public FlightDto? Flight { get; set; }

        public UserDto? User { get; set; }

        public RankDto? Rank { get; set; }

        public TicketStatusDto? TicketStatus { get; set; }
    }
}
