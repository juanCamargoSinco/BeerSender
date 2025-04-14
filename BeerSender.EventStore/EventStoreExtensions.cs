using BeerSender.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.EventStore;

public static class EventStoreExtensions
{
    public static void RegisterEventStore(this IServiceCollection services)
    {
        //Se registra como singleton para tener una unica instancia durante toda la ejecucion de la aplicacion
        //Y ser un punto de acceso global mediante esa instancia.
        //Ademas de que no cambia durante la ejecucion por lo que no necesita ser recreada siempre
        services.AddSingleton<EventStoreConnectionFactory>();
        services.AddScoped<IEventStore, EventStore>();
    }
}
