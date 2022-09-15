namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ReadOnlySpan{T}"/>.
/// </summary>
public static class ReadOnlySpanExt
{
    /// <summary>
    /// Enumerates the current <paramref name="span"/>.
    /// </summary>
    /// <param name="span">This span.</param>
    /// <typeparam name="T">The type of data in the span.</typeparam>
    /// <returns>An enumeration of this <paramref name="span"/>.</returns>
    public static IEnumerable<T> AsEnumerable<T>(this ReadOnlySpan<T> span)
    {
        // This needs to be done this way because the compiler doesn't allow
        // Spans to be used in iterator methods (yield return).
        var result = Enumerable.Empty<T>();

        for (var index = 0; index < span.Length; index++)
            result = result.Append(span[index]);

        return result;
    }
}
