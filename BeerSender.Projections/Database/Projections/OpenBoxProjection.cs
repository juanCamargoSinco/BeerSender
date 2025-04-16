using BeerSender.Domain;
using BeerSender.Domain.Boxes;
using BeerSender.Projections.Database.Repositories;

namespace BeerSender.Projections.Database.Projections
{
    public class OpenBoxProjection(OpenBoxRepository openBoxRepo) : IProjection
    {
        public List<Type> RelevantEventTypes => [typeof(BoxCreated), typeof(BeerBottleAdded), typeof(BoxClosed)];

        public int BatchSize => 50; //Cantidad de eventos maxima que se va a manejar por lote.

        public int WaitTime => 5000; //Cantidad de tiempo a esperar para realizar proyeccion. 5 segundos en este caso


        //Logica para hacer la proyeccion
        public void Project(IEnumerable<StoredEventWithVersion> events)
        {

            foreach (var storedEvent in events)
            {
                var boxId = storedEvent.AggregateId;

                switch (storedEvent.EventData)
                {
                    case BoxCreated boxCreated:
                        var capacity = boxCreated.Capacity.NumberOfSpots;
                        openBoxRepo.CreateOpenBox(boxId, capacity);
                        break;
                    case BeerBottleAdded:
                        openBoxRepo.AddBottleToBox(boxId);
                        break;
                    case BoxClosed:
                        openBoxRepo.RemoveOpenBox(boxId);
                        break;
                }
            }
        }
    }
}
