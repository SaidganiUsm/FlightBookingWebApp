using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class Location : BaseAuditableEntity
    {
        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public Flight? Flight { get; set; }
    }
}
