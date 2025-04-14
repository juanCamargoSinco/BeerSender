namespace BeerSender.Domain.Boxes.Commands
{
    public record CreateBox(Guid BoxId, int DesiredNumberOfSpots);
    //Manejador de comandos
    public class CreateBoxHandler(IEventStore eventStore)
        : CommandHandler<CreateBox>(eventStore)
    {
        public override void Handle(CreateBox command)
        {
            var boxStream = GetStream<Box>(command.BoxId);
            var box = boxStream.GetEntity();

            boxStream.Append(new BoxCreated(new BoxCapacity(command.DesiredNumberOfSpots)));

        }
    }
}
