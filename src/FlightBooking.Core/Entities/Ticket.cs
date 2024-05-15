using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class Ticket : BaseAuditableEntity
    {
        public double Price { get; set; }

        public DateTime DateOfCreation { get; set; }

        public int FlightId { get; set; }

        public Flight? Flight { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int RankId { get; set; }

        public TicketType? Rank { get; set; }
    }
}
