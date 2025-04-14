using static BeerSender.Domain.Boxes.ShippingLabelFailedToAdd;

namespace BeerSender.Domain.Boxes
{
    //Root entity, del agregado. Todo lo que este en Box, definiria el agregado
    public class Box : AggregateRoot
    {
        public BoxCapacity? Capacity { get; private set; }
        public ShippingLabel? ShippingLabel { get; private set; }

        public void Apply(BoxCreated @event)
        {
            Capacity = @event.Capacity;
        }

        public void Apply(ShippingLabelAdded @event)
        {
            ShippingLabel = @event.Label;
        }


    }


    //Comandos
    //Los command no cambian despues de haberlos ejecutado
    //Ideal manejar commands con events ya que una vez se crean no cambian
    public record CreateBox(Guid BoxId, int DesiredNumberOfSpots);

    //Eventos
    public record BoxCreated(BoxCapacity Capacity);
    public record ShippingLabelAdded(ShippingLabel Label);

    //evento fallido
    //public record ShippingLabelFailedToAddForInvalidTrackingCode();
    public record ShippingLabelFailedToAdd(FailReason Reason)
    {
        public enum FailReason
        {
            TrackingCodeInvalid
        }
    };

    public enum Carrier
    {
        UPS, FedEX, BPost
    }

    //Value objects
    //Divido en strict que genera excepcion cuando reciben valores invalidos, lo que puede hacer que no pueda deserializar o crear estados despues si mi logica ha cambiado, lo que me obliga a versionar las cosas
    //Loose que no tienen problemas con esto y se pueden añadir validaciones en su lugar, lo que me da mas libertad a la hora de crear o deserelizar estos, pero con el riesgo de crear algo incorrecto
    public record ShippingLabel(Carrier Carrier, string TrackingCode)
    {
        public bool IsValid()
        {
            return Carrier switch
            {
                Carrier.UPS => TrackingCode.StartsWith("ABC"),
                Carrier.FedEX => TrackingCode.StartsWith("ABC"),
                Carrier.BPost => TrackingCode.StartsWith("GHI"),
                _ => throw new ArgumentOutOfRangeException(nameof(Carrier), Carrier, null)
            };
        }
    }

    public record BoxCapacity(int NumberOfSpots)
    {
        public static BoxCapacity Create(int desiredNumberOfSpots)
        {
            return desiredNumberOfSpots switch
            {
                <= 6 => new BoxCapacity(6),
                <= 12 => new BoxCapacity(12),
                _ => new BoxCapacity(24)
            };
        }
    }

}
