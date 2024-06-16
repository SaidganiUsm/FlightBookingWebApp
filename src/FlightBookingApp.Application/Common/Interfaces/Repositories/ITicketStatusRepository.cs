using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Repositories;

namespace FlightBookingApp.Application.Common.Interfaces.Repositories
{
    public interface ITicketStatusRepository : IAsyncRepository<TicketStatus>
    {

    }
}
