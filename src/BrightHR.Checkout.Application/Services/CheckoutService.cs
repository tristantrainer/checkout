namespace BrightHR.Checkout.Application.Services;

public interface ICheckout
{
    int GetTotalPrice();
}

internal sealed class CheckoutService : ICheckout
{
    public int GetTotalPrice()
    {
        throw new NotImplementedException();
    }
}
