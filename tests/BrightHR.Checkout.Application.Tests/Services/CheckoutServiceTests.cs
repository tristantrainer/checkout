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
            .Setup((repo) => repo.GetCurrentUnitPrices())
            .Returns([new UnitPrice("A", 123)]);

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
            .Setup((repo) => repo.GetCurrentUnitPrices())
            .Returns([
                new UnitPrice("A", 1), 
                new UnitPrice("B", 20), 
                new UnitPrice("C", 300)
            ]);

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
            .Setup((repo) => repo.GetCurrentSpecialPrices())
            .Returns([new SpecialPrice("A", 3, 25)]);

        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetCurrentUnitPrices())
            .Returns([new UnitPrice("A", 10)]);

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

    [Fact]
    public void GetTotalPrice_WhenOrderScannedIsRandom_ReturnsCorrectlyDiscountedPrice() 
    {
        // Arrange
        var items = new string[] { "A", "A", "A", "A", "B", "B", "C", "C", "D" };
        Random.Shared.Shuffle(items);

        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        specialPriceRepositoryMock
            .Setup((repo) => repo.GetCurrentSpecialPrices())
            .Returns([new SpecialPrice("A", 3, 25)]);

        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetCurrentUnitPrices())
            .Returns([
                new UnitPrice("A", 10), 
                new UnitPrice("B", 100), 
                new UnitPrice("C", 1_000),
                new UnitPrice("D", 10_000),
            ]);
            
        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);
        var expected = 12235;

        foreach(var sku in items) 
        {
            checkout.Scan(sku);
        }

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenUnitPricesUpdateDuringCheckout_ReturnsTotalInitialPrice() 
    {
        // Arrange
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .SetupSequence((repo) => repo.GetCurrentUnitPrices())
            .Returns([new UnitPrice("A", 1)])
            .Returns([new UnitPrice("A", 999)]);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);

        checkout.Scan("A");

        var expected = checkout.GetTotalPrice();

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTotalPrice_WhenSpecialPricesUpdateDuringCheckout_ReturnsTotalInitialPrice() 
    {
        // Arrange
        var specialPriceRepositoryMock = new Mock<ISpecialPriceRepository>();
        specialPriceRepositoryMock
            .SetupSequence((repo) => repo.GetCurrentSpecialPrices())
            .Returns([new SpecialPrice("A", 3, 25)])
            .Returns([new SpecialPrice("A", 3, 999)]);

        var unitPriceRepositoryMock = new Mock<IUnitPriceRepository>();
        unitPriceRepositoryMock
            .Setup((repo) => repo.GetCurrentUnitPrices())
            .Returns([new UnitPrice("A", 1)]);

        var checkout = new CheckoutService(unitPriceRepositoryMock.Object, specialPriceRepositoryMock.Object);

        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");

        var expected = checkout.GetTotalPrice();

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }


    #endregion GetTotalPrice
}
