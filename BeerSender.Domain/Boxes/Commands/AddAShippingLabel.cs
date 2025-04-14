﻿namespace BeerSender.Domain.Boxes.Commands
{
    //Comando
    public record AddShippingLabel(Guid BoxId, ShippingLabel Label);

    //Manejador de comandos
    public class AddShippingLabelHandler(IEventStore eventStore)
        : CommandHandler<AddShippingLabel>(eventStore)
    {
        public override void Handle(AddShippingLabel command)
        {
            var boxStream = GetStream<Box>(command.BoxId);
            var box = boxStream.GetEntity();

            if (command.Label.IsValid())
            {
                boxStream.Append(new ShippingLabelAdded(command.Label));
            }
            else
            {
                boxStream.Append(new FailedToAddShippingLabel(FailedToAddShippingLabel.FailReason.TrackingCodeInvalid));
            }
        }
    }
}
