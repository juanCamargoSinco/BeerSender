using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerSender.Domain.Boxes.Commands
{
    //Comando
    public record AddBeer(Guid BoxId, BeerBox BeerBox);

    public class AddBeerHandler(IEventStore eventStore)
    : CommandHandler<AddBeer>(eventStore)
    {
        public override void Handle(AddBeer command)
        {
            var boxStream = GetStream<Box>(command.BoxId);
            var box = boxStream.GetEntity();


            boxStream.Append(new BeerAdded(command.BeerBox));
        }
    }
}
