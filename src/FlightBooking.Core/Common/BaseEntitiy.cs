using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBookingApp.Core.Common
{
    public abstract class BaseEntitiy
    {
        public int Id { get; set; }

        private readonly List<BaseEvent> domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
    }
}
