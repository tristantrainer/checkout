using BrightHR.Checkout.Application.Entities;
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
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);
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
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetUnitPrice("A"))
            .Returns(123);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);
        var expected = 123;

        checkout.Scan("A");

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenMultipleItemsScannedAndNotSpecialPrice_ReturnsTotalPrice() 
    {
        // Arrange
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
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

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);
        var expected = 321;

        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("C");

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenSpecialPriceApplies_ReturnsSpecialPrice() 
    {
        // Arrange
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        specialPriceRepositoryMock
            .Setup((repo) => repo.GetSpecialPrice("A"))
            .Returns(new SpecialPrice("A", 3, 25));

        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetUnitPrice("A"))
            .Returns(10);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);
        var expected = 25;

        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    #endregion GetTotalPrice
}
