using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;

namespace FlightBookingApp.Application.Common.Interfaces.Repositories
{
    public interface IFlightRepository : IAsyncRepository<Flight>
    {

    }
}
