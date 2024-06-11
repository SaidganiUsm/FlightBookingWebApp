using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class FlightStatus : BaseAuditableEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Flight>? Flights { get; set; }
    }
}
