using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;
using FlightBookingApp.Infrastructure.Persistence;

namespace FlightBookingApp.Infrastructure.Repositories
{
    public class TicketRepository 
        : EfBaseRepository<Ticket, ApplicationDbContext>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
