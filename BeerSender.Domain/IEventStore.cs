namespace BeerSender.Domain;

public interface IEventStore
{
    IEnumerable<StoredEvent> GetEvents(Guid aggregateId);
    void AppendEvent(StoredEvent @event);
    void SaveChanges();
    IEnumerable<StoredEvent> GetEventsUntilSequence(Guid aggregateId, int sequenceNumber);
}