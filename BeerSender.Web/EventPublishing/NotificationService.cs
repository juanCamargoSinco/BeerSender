using BeerSender.Domain;
using Microsoft.AspNetCore.SignalR;

namespace BeerSender.Web.EventPublishing;

public class NotificationService(IHubContext<EventHub> hubContext) : INotificationService
//Un Hub es el punto central en SignalR donde los clientes se conectan y reciben mensajes.
//En este caso hubContext envia a PublishEvent que esta registrado en el front (signalNPM)
{
    //Publicacion de eventos a los clientes conectados a través de SignalR.
    public void PublishEvent(Guid aggregateId, object @event)
    {
        //Se envia los eventos a los clientes que se han conectado a PublishEvent
        hubContext.Clients.Group(aggregateId.ToString())
            .SendAsync("PublishEvent", aggregateId, @event, @event.GetType().Name);
    }
}
