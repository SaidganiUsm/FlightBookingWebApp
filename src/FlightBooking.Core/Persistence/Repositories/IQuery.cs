namespace FlightBookingApp.Core.Persistence.Repositories
{
    public interface IQuery<T>
    {
        IQueryable<T> Query();

        IQueryable<T> QueryAsNoTracking();
    }
}
