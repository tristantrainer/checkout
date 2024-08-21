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
            total += CalculateTotalForItem(sku);
        }

        return total;
    }

    private int CalculateTotalForItem(string sku)
    {
        var numberOfItems = _items[sku];

        var (specialPriceTotal, remainingItems) = CalculateSpecialPriceTotal(sku, numberOfItems);
        var remainingTotal = CalculateUnitPriceTotal(sku, remainingItems);
        
        return specialPriceTotal + remainingTotal;
    }

    private (int Total, int RemainingItems) CalculateSpecialPriceTotal(string sku, int numberOfItems) 
    {
        if(_specialPrices.ContainsKey(sku) is false)
            return (0, numberOfItems);

        var specialPrice = _specialPrices[sku];
        var numberOfSpecialPriceItems = numberOfItems / specialPrice.Quantity;
        var numberOfUndiscountedItems = numberOfItems % specialPrice.Quantity;

        return (numberOfSpecialPriceItems * specialPrice.Price, numberOfUndiscountedItems);
    }

    private int CalculateUnitPriceTotal(string sku, int numberOfItems)
    {
        return numberOfItems * _unitPrices[sku].Price;
    }
}
