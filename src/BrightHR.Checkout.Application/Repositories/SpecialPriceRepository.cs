using BrightHR.Checkout.Application.Entities;

namespace BrightHR.Checkout.Application.Repositories;

public interface ISpecialPriceRepository
{
    SpecialPrice[] GetCurrentSpecialPrices();
}