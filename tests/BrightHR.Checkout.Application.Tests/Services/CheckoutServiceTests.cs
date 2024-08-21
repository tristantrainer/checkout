using BrightHR.Checkout.Application.Repositories;
using BrightHR.Checkout.Application.Services;
using Moq;

namespace BrightHR.Checkout.Application.Tests.Services;

public class CheckoutServiceTests
{
    #region GetTotalPrice

    [Fact]
    public void GetTotalPrice_WhenCheckoutIsEmpty_ReturnsZero() 
    {
        // Arrange
        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        var checkout = new CheckoutService(unitPriceRepositoryMock.Object);
        var expected = 0;

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenItemScanned_ReturnsItemPrice() 
    {
        // Arrange
        var unitPrice = 123;
        var sku = "A";

        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock.Setup((repo) => repo.GetUnitPrice(sku)).Returns(unitPrice);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object);
        var expected = 123;

        checkout.Scan(sku);

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenMultipleItemScanned_ReturnsSumOfUnitPrices() 
    {
        // Arrange
        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetUnitPrice("A"))
            .Returns(1);
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetUnitPrice("B"))
            .Returns(20);
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetUnitPrice("C"))
            .Returns(300);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object);
        var expected = 321;

        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("C");

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    #endregion GetTotalPrice
}
