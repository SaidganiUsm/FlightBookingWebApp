using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class Flight : BaseAuditableEntity
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int FlightStatusId { get; set; }

        public FlightStatus? FlightStatus { get; set; }

        public int DepartureLocationId { get; set; }

        public Location? DepartureLocation { get; set; }

        public int DestinationLocationId { get; set; }

        public Location? DestinationLocation { get; set; }

        public int TotalTickets { get; set; }

        public int TicketsAvailable { get; set; }

        public int FirstClassTicketsAmount { get; set; }

        public int BusinessTicketsAmount { get; set; }

        public int EconomyTicketsAmount { get; set; }

        public int StandartPriceForFlight { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
