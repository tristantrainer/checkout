namespace BrightHR.Checkout.Application.Extensions;

internal static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        where TKey : notnull
    {
        throw new NotImplementedException();
    }
}
