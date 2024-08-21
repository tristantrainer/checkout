using BrightHR.Checkout.Application.Services;

namespace BrightHR.Checkout.Application.Tests.Services;

public class CheckoutServiceTests
{
    #region GetTotalPrice

    [Fact]
    public void GetTotalPrice_WhenCheckoutIsEmpty_ReturnsZero() 
    {
        // Arrange
        var checkout = new CheckoutService();
        var expected = 0;

        // Act
        var actual = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expected, actual);
    }

    #endregion GetTotalPrice
}
