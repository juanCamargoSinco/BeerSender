﻿

namespace BeerSender.Domain.Boxes
{
    //Root entity, del agregado. Todo lo que este en Box, definiria el agregado
    public class Box : AggregateRoot
    {
        public List<BeerBottle> BeerBottles { get; } = [];
        public BoxCapacity? Capacity { get; private set; }
        public ShippingLabel? ShippingLabel { get; private set; }
        public bool IsClosed { get; private set; }
        public bool IsSent { get; private set; }

        public void Apply(BoxCreated @event)
        {
            Capacity = @event.Capacity;
        }

        public void Apply(BeerBottleAdded @event)
        {
            BeerBottles.Add(@event.Bottle);
        }

        public void Apply(ShippingLabelAdded @event)
        {
            ShippingLabel = @event.Label;
        }

        public void Apply(BoxClosed @event)
        {
            IsClosed = true;
        }

        public void Apply(BoxSent @event)
        {
            IsSent = true;
        }

        public bool IsFull => BeerBottles.Count >= Capacity?.NumberOfSpots;

    }

}
