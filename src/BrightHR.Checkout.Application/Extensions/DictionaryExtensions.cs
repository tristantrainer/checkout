namespace BrightHR.Checkout.Application.Extensions;

internal static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue, Func<TValue, TValue> updater)
        where TKey : notnull
    {
        dictionary[key] = dictionary.TryGetValue(key, out TValue? existing) ? updater(existing) : newValue;
    }
}
