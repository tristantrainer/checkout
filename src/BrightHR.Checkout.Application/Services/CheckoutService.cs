using BrightHR.Checkout.Application.Repositories;

namespace BrightHR.Checkout.Application.Services;

public interface ICheckout
{
    void Scan(string sku);
    int GetTotalPrice();
}

internal sealed class CheckoutService(IUnitPriceRepository unitPriceRepository) : ICheckout
{
    private int _currentTotal = 0;

    public void Scan(string sku)
    {
        _currentTotal += unitPriceRepository.GetUnitPrice(sku);
    }

    public int GetTotalPrice()
    {
        return _currentTotal;
    }
}
