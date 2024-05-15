using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class Flight : BaseAuditableEntity
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string? DeparturePoint { get; set; }

        public string? DestinationPoint { get; set; }

        public DateTime DateOfCreation { get; set; }

        public int TotalTickets { get; set; }

        public int TicketsAvailable { get; set; }

        public ICollection<TicketType>? TicketTypes { get; set; }
    }
}
