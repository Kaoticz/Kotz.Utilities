namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Span{T}"/>.
/// </summary>
public static class SpanExt
{
    /// <summary>
    /// Rotates a span from a starting position by the specified <paramref name="amount"/> of indices.
    /// </summary>
    /// <param name="span">This span.</param>
    /// <param name="startIndex">The index where shifting should start.</param>
    /// <param name="amount">The amount of positions each element should be shifted by.</param>
    /// <typeparam name="T">The type of data in the span.</typeparam>
    /// <returns>This span rotated.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the boundaries of the span.</exception>
    public static Span<T> Rotate<T>(this Span<T> span, int startIndex, int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount cannot be lower than zero.");
        else if (startIndex >= span.Length || startIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, $"'{nameof(startIndex)}' is out of range.");

        // Normalize the amount requested
        amount = (startIndex + (amount % span.Length) > span.Length - startIndex)
            ? amount % span.Length % startIndex
            : amount % span.Length;

        if (amount is 0)
            return span;

        // If the middle overlaps the end, use a buffer.
        var buffer = (span[startIndex..(startIndex + amount)].Length <= span[(span.Length - startIndex - amount)..].Length)
            ? span[startIndex..(startIndex + amount)].ToArray()
            : Array.Empty<T>();

        for (int counter = 0, index = startIndex + amount; index < span.Length; counter++, index++)
            (span[index], span[startIndex + counter]) = (span[startIndex + counter], span[index]);

        buffer.CopyTo(span[^amount..]);

        return span;
    }

    /// <summary>
    /// Enumerates the current <paramref name="span"/>.
    /// </summary>
    /// <param name="span">This span.</param>
    /// <typeparam name="T">The type of data in the span.</typeparam>
    /// <returns>An enumeration of this <paramref name="span"/>.</returns>
    public static IEnumerable<T> AsEnumerable<T>(this Span<T> span)
    {
        // This needs to be done this way because the compiler doesn't allow
        // Spans to be used in iterator methods (yield return).
        var result = Enumerable.Empty<T>();

        for (var index = 0; index < span.Length; index++)
            result = result.Append(span[index]);

        return result;
    }
}