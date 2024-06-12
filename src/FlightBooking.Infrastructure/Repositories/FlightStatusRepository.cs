using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;
using FlightBookingApp.Infrastructure.Persistence;

namespace FlightBookingApp.Infrastructure.Repositories
{
    public class FlightStatusRepository 
        : EfBaseRepository<FlightStatus, ApplicationDbContext>, IFlightStatusRepository
    {
        public FlightStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
