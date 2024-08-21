using BrightHR.Checkout.Application.Entities;
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

    private readonly Dictionary<string, UnitPrice> _unitPrices = unitPriceRepository
        .GetCurrentUnitPrices()
        .ToDictionary((price) => price.SKU, (price) => price);
    private readonly Dictionary<string, SpecialPrice> _specialPrices = specialPriceRepository
        .GetCurrentSpecialPrices()
        .ToDictionary((price) => price.SKU, (price) => price);

    public void Scan(string sku)
    {
        _items[sku] = _items.TryGetValue(sku, out int value) ? value + 1 : 1;
    }

    public int GetTotalPrice()
    {
        var total = 0;

        foreach(var sku in _items.Keys) 
        {
            var numberOfItems = _items[sku];

            var (remaining, subtotal) = CalculateDiscountedItems(sku, numberOfItems);

            total += subtotal + remaining * _unitPrices[sku].Price;
        }

        return total;
    }

    private (int RemainingItems, int Total) CalculateDiscountedItems(string sku, int initialItems) 
    {
        if(_specialPrices.ContainsKey(sku) is false)
            return (initialItems, 0);

        var specialPrice = _specialPrices[sku];

        var discountedQuanity = initialItems / specialPrice.Quantity;
        var remainingItems = initialItems % specialPrice.Quantity;

        var subtotal = discountedQuanity * specialPrice.Price;

        return (remainingItems, subtotal);
    }
}
