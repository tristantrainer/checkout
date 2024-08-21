using BrightHR.Checkout.Application.Repositories;

namespace BrightHR.Checkout.Application.Services;

public interface ICheckout
{
    void Scan(string sku);
    int GetTotalPrice();
}

internal sealed class CheckoutService : ICheckout
{
    private int _total = 0;

    public void Scan(string sku)
    {
        _total += 123;
    }

    public int GetTotalPrice()
    {
        return _total;
    }
}
