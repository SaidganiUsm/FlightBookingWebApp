using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;
using FlightBookingApp.Infrastructure.Persistence;

namespace FlightBookingApp.Infrastructure.Repositories
{
    public class RankRepository 
        : EfBaseRepository<Rank, ApplicationDbContext>, IRankRepository
    {
        public RankRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
