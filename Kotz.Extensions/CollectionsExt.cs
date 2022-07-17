using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for collections in general.
/// </summary>
public static class CollectionsExt
{
    /// <summary>
    /// Gets the value associated with the specified index.
    /// </summary>
    /// <typeparam name="T">The type of data the stored in the collection.</typeparam>
    /// <param name="collection">This list.</param>
    /// <param name="index">The index of the desired element.</param>
    /// <param name="value">The value at the specified index or <see langword="default"/> if the index is not found.</param>
    /// <returns><see langword="true"/> if the element is found, <see langword="false"/> otherwise.</returns>
    public static bool TryGetValue<T>(this IReadOnlyList<T> collection, int index, [MaybeNullWhen(false)] out T value)
    {
        if (index < 0 || collection.Count <= index)
        {
            value = default;
            return false;
        }

        value = collection[index];
        return true;
    }

    /// <summary>
    /// Gets the item that meets the criteria of the speficied <paramref name="predicate"/>
    /// </summary>
    /// <param name="collection">This collection</param>
    /// <param name="predicate">Delegate that defines the conditions of the item to get.</param>
    /// <param name="item">The resulting item.</param>
    /// <typeparam name="T">The type of data the stored in the collection.</typeparam>
    /// <returns><see langword="true"/> if the element is found, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static bool TryGetValue<T>(this IReadOnlyList<T> collection, Func<T?, bool> predicate, [MaybeNullWhen(false)] out T item)
    {
        var index = collection.IndexOf(predicate);
        item = (index is not -1) ? collection[index] : default;

        return index is not -1;
    }

    /// <summary>
    /// Gets the index of the first element that matches the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="predicate">Delegate that defines the conditions of the item to get.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>The index of the item, -1 otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this IReadOnlyList<T> collection, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        for (var index = 0; index < collection.Count; index++)
        {
            if (predicate(collection[index]))
                return index;
        }

        return -1;
    }

    /// <summary>
    /// Gets the index of the last element that matches the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="predicate">Delegate that defines the conditions of the item to get.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>The index of the item, -1 otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static int LastIndexOf<T>(this IReadOnlyList<T> collection, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        for (var index = collection.Count - 1; index >= 0; index--)
        {
            if (predicate(collection[index]))
                return index;
        }

        return -1;
    }

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