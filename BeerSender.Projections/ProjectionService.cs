using BeerSender.Projections.Database;
using BeerSender.Projections.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BeerSender.Projections;

public class ProjectionService<TProjection>(
    IServiceProvider serviceProvider,
    EventStoreRepository eventStore) : BackgroundService
    where TProjection : class, IProjection
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 1. Obtener el checkpoint del ultimo evento que hemos visto para saber desde donde nos quedamos
        var checkpoint = GetCheckPoint();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Un scope es un contenedor de objetos que están vivos durante un período de tiempo determinado,
            // como una sola solicitud HTTP o una unidad de trabajo.
            // Esto es útil para manejar servicios que necesitan ser creados y destruidos de manera eficiente.
            using var scope = serviceProvider.CreateScope();

            var connection = scope.ServiceProvider
                .GetRequiredService<ReadStoreConnection>();
            using var transaction = connection.GetTransaction();

            var projection = scope.ServiceProvider
                .GetRequiredService<TProjection>();

            // 2.Obtener el lote de eventos
            var events = eventStore.GetEvents(
                projection.RelevantEventTypes,
                checkpoint,
                projection.BatchSize
             );

            // 3.Si esta vacio esperar
            if (!events.Any())
            {
                await Task.Delay(projection.WaitTime, stoppingToken);
            }
            // 4.Si no ejecutar el codigo de proyeccion
            else 
            {
                projection.Project(events);

                checkpoint = events.Last().RowVersion;
                var checkpointRepo = scope.ServiceProvider
                    .GetRequiredService<CheckpointRepository>();

                // 5.Actualizar el checkpoint 
                checkpointRepo.SetCheckpoint(
                    typeof(TProjection).Name,
                    checkpoint);
            }
            // 6.Commit
            transaction.Commit();
        }

    }

    private byte[] GetCheckPoint()
    {
        using var scope = serviceProvider.CreateScope();
        var checkpointService = scope.ServiceProvider
            .GetRequiredService<CheckpointRepository>();

        var checkpoint = checkpointService.GetCheckpoint(
            typeof(TProjection).Name);

        return checkpoint;
    }
}
