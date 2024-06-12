using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class TicketStatus :  BaseAuditableEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
