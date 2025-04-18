using BeerSender.Domain;
using Dapper;

namespace BeerSender.EventStore;

public class EventStore(EventStoreConnectionFactory DbConnectionFactory
    , INotificationService notificationService
    )
    : IEventStore
{
    public IEnumerable<StoredEvent> GetEvents(Guid aggregateId)
    {
        const string query = """
                             SELECT [AggregateId], [SequenceNumber], [TimeStamp]
                                   ,[EventTypeName], [EventBody], [RowVersion]
                             FROM dbo.[Events]
                             WHERE [AggregateId] = @AggregateId
                             ORDER BY [SequenceNumber]
                             """;

        using var connection = DbConnectionFactory.Create();

        return connection.Query<DatabaseEvent>(
                query,
                new { AggregateId = aggregateId }) //Parametro que se pasa a la consulta que se esta ejecutando en el query.
                                                   //En este caso pasamos @AggregateId lo que hay en aggregateId
            .Select(e => e.ToStoredEvent());
    }

    public IEnumerable<StoredEvent> GetEventsUntilSequence(Guid aggregateId, int sequence)
    {
        const string query = """
                             SELECT [AggregateId], [SequenceNumber], [Timestamp]
                                   ,[EventTypeName], [EventBody], [RowVersion]
                             FROM dbo.[Events]
                             WHERE [AggregateId] = @AggregateId
                                   AND [SequenceNumber] <= @Sequence
                             ORDER BY [SequenceNumber]
                             """;

        using var connection = DbConnectionFactory.Create();

        return connection.Query<DatabaseEvent>(
                query,
                new { AggregateId = aggregateId, Sequence = sequence })
            .Select(e => e.ToStoredEvent());
    }


    private readonly List<StoredEvent> _newEvents = [];
    //Cachea los eventos para usarlos luego
    public void AppendEvent(StoredEvent @event)
    {
        _newEvents.Add(@event);
    }

    public void SaveChanges()
    {
        const string insertCommand = """
                                     INSERT INTO dbo.[Events]
                                                ([AggregateId], [SequenceNumber], [TimeStamp]
                                                ,[EventTypeName], [EventBody])    
                                     VALUES
                                                (@AggregateId, @SequenceNumber,@TimeStamp
                                                ,@EventTypeName, @EventBody)
                                     """;

        using var connection = DbConnectionFactory.Create();
        connection.Open();
        //Se crea una transaccion ya que o se guardan todos los eventos o no se guarda ninguno
        using var transaction = connection.BeginTransaction();

        connection.Execute(
            insertCommand,
            _newEvents.Select(DatabaseEvent.FromStoredEvent), //Dapper detecta que esto es una coleccion y lo ejecuta para cada elemento de la lista
            transaction);

        transaction.Commit();

        foreach (var storedEvent in _newEvents)
        {
            notificationService.PublishEvent(
                storedEvent.AggregateId,
                storedEvent.EventData);
        }

        _newEvents.Clear();
    }
}