namespace BeerSender.Domain;

public class EventStream<TEntity>(IEventStore eventStore, Guid aggregateId)
    where TEntity : AggregateRoot, new() // new () especifica que TEntity debe tener un constructor público sin parámetros.
// Esto es útil en casos donde, por ejemplo, necesites reconstituir o "rehidratar" la aggregate
// a partir de una serie de eventos sin tener que pasarle parámetros adicionales durante la creación.
{
    private int _lastSequenceNumber;
    //Obtener la entidad al reconstruirla apartir de los eventos
    public TEntity GetEntity()
    {
        var events = eventStore.GetEvents(aggregateId);
        TEntity entity = new();

        foreach (var @event in events)
        {
            //Al castearlo a dynamic hara que se busque la implementacion mas cercana del metodo segun el tipo de EventData
            //Haciendo que se apliquen los Apply correspondientes del agregado segun el value object
            entity.Apply((dynamic)@event.EventData);
            _lastSequenceNumber = @event.SequenceNumber;
        }

        return entity;
    }
    //Añadir eventos al stream
    //Recordar que el stream son todos los eventos en una linea de tiempo

    public void Append(object @event)
    {
        _lastSequenceNumber++;

        StoredEvent storedEvent = new(aggregateId, _lastSequenceNumber, DateTime.UtcNow, @event);

        eventStore.AppendEvent(storedEvent);

    }

}