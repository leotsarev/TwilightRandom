namespace Twilight.Domain;

public static class StaticCollectionHelpers
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        foreach (var i in source)
        {
            if (i is T v)
            {
                yield return v;
            }
        }
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : struct
    {
        foreach (var i in source)
        {
            if (i is T v)
            {
                yield return v;
            }
        }
    }
}
