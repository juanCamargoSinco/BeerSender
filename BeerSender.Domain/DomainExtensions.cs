using BeerSender.Domain.Boxes.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.Domain;

public static class DomainExtensions
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        //Se registra como Scoped, es decir, una instancia por solicitud.
        //se creará una única vez por cada solicitud del ciclo de vida
        //(por ejemplo, en aplicaciones web, por cada request HTTP).
        //Esto significa que durante una petición, cualquier solicitud de CommandRouter
        //devolverá la misma instancia, pero en peticiones distintas se crearán
        //nuevas instancias.
        services.AddScoped<CommandRouter>();
        //Transient se crea una nueva instancia cada vez que se solicita.
        //Es decir, cada vez que se inyecta o se resuelve uno de estos manejadores,
        //se instanciará uno nuevo. Esto es común cuando el objeto es liviano o se requiere
        //tener una nueva instancia para cada operación.
        services.AddTransient<CommandHandler<CreateBox>, CreateBoxHandler>();
        services.AddTransient<CommandHandler<AddShippingLabel>, AddShippingLabelHandler>();
        services.AddTransient<CommandHandler<AddBeerBottle>, AddBeerBottleHandler>();
        services.AddTransient<CommandHandler<CloseBox>, CloseBoxHandler>();
        services.AddTransient<CommandHandler<SendBox>, SendBoxHandler>();
    }
}
