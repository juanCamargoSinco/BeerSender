namespace BeerSender.Domain.Boxes;


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
            Carrier.FedEx => TrackingCode.StartsWith("DEF"),
            Carrier.BPost => TrackingCode.StartsWith("GHI"),
            _ => throw new ArgumentOutOfRangeException(nameof(Carrier), Carrier, null)
        };
    }
}