using System.Diagnostics.CodeAnalysis;

namespace Kotz.Extensions;

public static class CollectionsTryGetExt
{
    /// <summary>
    /// Gets the value associated with the specified index.
    /// </summary>
    /// <typeparam name="T">The type of data the stored in the array.</typeparam>
    /// <param name="array">This array.</param>
    /// <param name="index">The index of the desired element.</param>
    /// <param name="value">The value at the specified index or <see langword="default"/> if the index is not found.</param>
    /// <returns><see langword="true"/> if the element is found, <see langword="false"/> otherwise.</returns>
    public static bool TryGetValue<T>(this T[] array, int index, [MaybeNullWhen(false)] out T value)
    {
        if (index < 0 || array.Length <= index)
        {
            value = default;
            return false;
        }

        value = array[index];
        return true;
    }

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
    /// Gets the value associated with the specified index.
    /// </summary>
    /// <typeparam name="T">The type of data the stored in the collection.</typeparam>
    /// <param name="collection">This list.</param>
    /// <param name="index">The index of the desired element.</param>
    /// <param name="value">The value at the specified index or <see langword="default"/> if the index is not found.</param>
    /// <returns><see langword="true"/> if the element is found, <see langword="false"/> otherwise.</returns>
    public static bool TryGetValue<T>(this IList<T> collection, int index, [MaybeNullWhen(false)] out T value)
    {
        if (index < 0 || collection.Count <= index)
        {
            value = default;
            return false;
        }

        value = collection[index];
        return true;
    }
}