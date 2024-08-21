namespace BrightHR.Checkout.Application.Repositories;

public interface IUnitPriceRepository
{
    int GetUnitPrice(string sku);
}