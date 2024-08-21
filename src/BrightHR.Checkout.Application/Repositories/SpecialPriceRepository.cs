using BrightHR.Checkout.Application.Entities;

namespace BrightHR.Checkout.Application.Repositories;

public interface ISpecialPriceRepository
{
    SpecialPrice? GetSpecialPrice(string sku);
    SpecialPrice[] GetCurrentSpecialPrices();
}