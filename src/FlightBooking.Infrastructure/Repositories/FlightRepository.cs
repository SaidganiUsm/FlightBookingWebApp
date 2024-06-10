using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;
using FlightBookingApp.Infrastructure.Persistence;

namespace FlightBookingApp.Infrastructure.Repositories
{
    public class FlightRepository : EfBaseRepository<Flight, ApplicationDbContext>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
