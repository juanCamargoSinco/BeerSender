namespace BeerSender.Domain.Boxes;
//Eventos
public record BoxCreated(BoxCapacity Capacity);

public record FailedToCreateBox(FailedToCreateBox.FailReason Reason)
{
    public enum FailReason
    {
        BoxAlreadyCreated,
        InvalidCapacity
    }
}

public record BeerBottleAdded(BeerBottle Bottle);

public record FailedToAddBeerBottle(FailedToAddBeerBottle.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasFull
    }
}

public record ShippingLabelAdded(ShippingLabel Label);
//evento fallido
//public record ShippingLabelFailedToAddForInvalidTrackingCode();
public record FailedToAddShippingLabel(FailedToAddShippingLabel.FailReason Reason)
{
    public enum FailReason
    {
        TrackingCodeInvalid
    }
}

public record BoxClosed;
//evento fallido
public record FailedToCloseBox(FailedToCloseBox.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasEmpty
    }
}


public record BoxSent;
//evento fallido
public record FailedToSendBox(FailedToSendBox.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasNotClosed,
        BoxHadNoLabel
    }
}