namespace BeerSender.Domain.Boxes.Commands;
//Comandos
//Los command no cambian despues de haberlos ejecutado
//Ideal manejar commands con records ya que una vez se crean no cambian
public record CreateBox(
    Guid BoxId,
    int DesiredNumberOfSpots 
);

public class CreateBoxHandler(IEventStore eventStore)
    : CommandHandler<CreateBox>(eventStore)
{
    public override void Handle(CreateBox command)
    {
        var boxStream = GetStream<Box>(command.BoxId);
        var box = boxStream.GetEntity();
        
        var capacity = BoxCapacity.Create(command.DesiredNumberOfSpots);
        boxStream.Append(new BoxCreated(capacity));
    }
}