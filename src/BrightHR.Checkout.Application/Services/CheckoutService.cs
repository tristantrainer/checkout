using BrightHR.Checkout.Application.Repositories;

namespace BrightHR.Checkout.Application.Services;

public interface ICheckout
{
    void Scan(string sku);
    int GetTotalPrice();
}

internal sealed class CheckoutService(IUnitPriceRepository unitPriceRepository, ISpecialPriceRepository specialPriceRepository) : ICheckout
{
    private readonly Dictionary<string, int> _items = [];

    public void Scan(string sku)
    {
        _items[sku] += _items.ContainsKey(sku) ? _items[sku] + 1 : 1;
    }

    public int GetTotalPrice()
    {
        var total = 0;

        foreach(var sku in _items.Keys) 
        {
            total += _items[sku] * unitPriceRepository.GetUnitPrice(sku);
        }

        return total;
    }
}
