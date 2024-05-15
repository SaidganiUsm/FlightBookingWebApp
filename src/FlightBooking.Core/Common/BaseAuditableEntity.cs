namespace FlightBookingApp.Core.Common
{
    public class BaseAuditableEntity : BaseEntitiy
    {
        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }
    }
}
