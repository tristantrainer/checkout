using BrightHR.Checkout.Application.Extensions;

namespace BrightHR.Checkout.Application.Tests.Extensions;

public class DictionaryExtensionsTests
{
    #region AddOrUpdate

    [Fact]
    public  void AddOrUpdate_WhenDictionaryDoesNotContainKey_AddsNewItem() 
    {
        // Arrange
        var key = "Item-Key";
        var item = "New Item";

        var dictionary = new Dictionary<string, string>();

        // Act
        dictionary.AddOrUpdate(key, item);

        // Assert
        Assert.Equal(item, dictionary[key]);
    }

    #endregion AddOrUpdate
}
