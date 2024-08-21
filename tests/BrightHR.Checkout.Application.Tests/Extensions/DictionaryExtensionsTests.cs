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
        var newItem = "NewItem";

        var dictionary = new Dictionary<string, string>();
        var expected = "NewItem";

        // Act
        dictionary.AddOrUpdate(key, newItem, (existing) => $"{existing}-{newItem}");

        // Assert
        Assert.Equal(expected, dictionary[key]);
    }

    [Fact]
    public  void AddOrUpdate_WhenDictionaryContainsKey_UpdatesExistingItem() 
    {
        // Arrange
        var key = "Item-Key";
        var newItem = "ItemUpdate";

        var dictionary = new Dictionary<string, string>
        {
            [key] = "ExistingItem"
        };

        var expected = "ExistingItem-ItemUpdate";

        // Act
        dictionary.AddOrUpdate(key, newItem, (existing) => $"{existing}-{newItem}");

        // Assert
        Assert.Equal(expected, dictionary[key]);
    }

    #endregion AddOrUpdate
}
