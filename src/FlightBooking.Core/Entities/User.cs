using Microsoft.AspNetCore.Identity;

namespace FlightBookingApp.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DeletionDate { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Ticket>? BookedTickets { get; set; }
    }
}
