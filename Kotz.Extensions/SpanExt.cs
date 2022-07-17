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
    /// <exception cref="ArgumentException">Occurs when <paramref name="amount"/> equals or is lower than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the boundaries of the span.</exception>
    public static Span<T> Rotate<T>(this Span<T> span, int startIndex, int amount)
    {
        if (amount <= 0 || amount >= span.Length)
            throw new ArgumentException($"Amount cannot be equal or lower than zero, or greater than {nameof(span)}'s length.", nameof(amount));
        if (startIndex >= span.Length || startIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(startIndex), $"Index of {startIndex} is out of range. Span length: {span.Length}");

        // Determine the amount of elements that are being moved
        // to the end of the span
        amount %= span.Length;

        // Create a temporary buffer for the elements at the start
        // of the span that must be preserved
        var startBuffer = span[0..startIndex].ToArray();

        // Create a temporary buffer for the elements at the middle
        // of the span that must be moved to the end of the span

        var endBuffer = span[startIndex..(startIndex + amount)].ToArray();

        // Copy the span - amount, overwritting itself and effectively
        // moving the entire span by amount. At this point, only the
        // shifted elements in the middle are at their correct position
        span[amount..].CopyTo(span);

        // Copy the elements that are supposed to be at the end
        endBuffer.CopyTo(span[^amount..]);

        // Copy the elements that are supposed to be at the start
        startBuffer.CopyTo(span);

        return span;
    }
}
