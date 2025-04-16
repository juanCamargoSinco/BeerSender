using BeerSender.Projections.Database.Projections;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.Projections.Projections;

public static class ProjectionExtensions
{
    public static void RegisterProjections(this IServiceCollection services)
    {
        services.AddTransient<OpenBoxProjection>();
        services.AddHostedService<ProjectionService<OpenBoxProjection>>();
    }
}
