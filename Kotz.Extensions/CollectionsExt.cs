using System.Diagnostics.CodeAnalysis;

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
}