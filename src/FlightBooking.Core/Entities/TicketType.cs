using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class TicketType : BaseAuditableEntity
    {
        public string? RankName { get; set; }
    }
}
