using BrightHR.Checkout.Application.Entities;
using BrightHR.Checkout.Application.Extensions;
using BrightHR.Checkout.Application.Repositories;

namespace BrightHR.Checkout.Application.Services;

public interface ICheckout
{
    void Scan(string item);
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

    public void Scan(string item)
    {
        _items.AddOrUpdate(item, 1, (count) => count + 1);
    }

    public int GetTotalPrice()
    {
        var total = 0;

        foreach(var sku in _items.Keys) 
        {
            var numberOfItems = _items[sku];

            var (remaining, discountedTotal) = CalculateDiscountedItemsTotal(sku, numberOfItems);

            total += discountedTotal + remaining * _unitPrices[sku].Price;
        }

        return total;
    }

    private (int RemainingItems, int Total) CalculateDiscountedItemsTotal(string sku, int initialItems) 
    {
        if(_specialPrices.ContainsKey(sku) is false)
            return (initialItems, 0);

        var specialPrice = _specialPrices[sku];

        var discountedQuantity = initialItems / specialPrice.Quantity;
        var remainingItems = initialItems % specialPrice.Quantity;

        var discountedTotal = discountedQuantity * specialPrice.Price;

        return (remainingItems, discountedTotal);
    }
}
