using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace UrfuPassSystem.Infrastructure;

public static class NameValueCollectionExtensions
{
    public static NameValueCollection SetIf<T>(this NameValueCollection collection, bool isSet, T value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        if (isSet)
            collection[name] = value?.ToString();
        else
            collection.Remove(name);
        return collection;
    }
}
