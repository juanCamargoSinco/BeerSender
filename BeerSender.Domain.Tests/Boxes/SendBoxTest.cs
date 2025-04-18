using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Commands;

namespace BeerSender.Domain.Tests.Boxes;

public class SendBoxTest : BoxTest<SendBox>
{
    protected override CommandHandler<SendBox> Handler
        => new SendBoxHandler(eventStore);
    
    [Fact]
    public void WhenBoxIsClosedAndHasLabel_ShouldSucceed()
    {
        Given(
            Box_created_with_capacity(24),
            Beer_bottle_added(gouden_carolus),
            Box_was_closed(),
            Shipping_label_added_with_carrier_and_code(Carrier.UPS, "ABC123")
        );

        When(
            Send_box()
        );

        Then(
            Box_was_sent()
        );
    }

    [Fact]
    public void WhenBoxHasNoLabel_ShouldFail()
    {
        Given(
            Box_created_with_capacity(24),
            Beer_bottle_added(gouden_carolus),
            Box_was_closed()
        );

        When(
            Send_box()
        );

        Then(
            Box_has_no_label()
        );
    }

    [Fact]
    public void WhenIsNotClosed_ShouldFail()
    {
        Given(
            Box_created_with_capacity(24),
            Beer_bottle_added(gouden_carolus),
            Shipping_label_added_with_carrier_and_code(Carrier.UPS, "ABC123")
        );

        When(
            Send_box()
        );

        Then(
            Box_was_not_closed()
        );
    }

    [Fact]
    public void WhenIsNotClosedAndHasNoLabel_ShouldFail()
    {
        Given(
            Box_created_with_capacity(24),
            Beer_bottle_added(gouden_carolus)
        );

        When(
            Send_box()
        );

        Then(
            Box_was_not_closed(),
            Box_has_no_label()
        );
    }
    
    protected SendBox Send_box()
    {
        return new SendBox(Box_Id);
    }
}