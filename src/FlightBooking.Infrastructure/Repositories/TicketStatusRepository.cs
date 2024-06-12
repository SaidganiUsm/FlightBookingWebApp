using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;
using FlightBookingApp.Infrastructure.Persistence;

namespace FlightBookingApp.Infrastructure.Repositories
{
    public class TicketStatusRepository 
        : EfBaseRepository<TicketStatus, ApplicationDbContext>, ITicketStatusRepository
    {
        public TicketStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
