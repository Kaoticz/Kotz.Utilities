using System.Runtime.InteropServices;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="List{T}"/>.
/// </summary>
public static class ListExt
{
    /// <summary>
    /// Performs an in-place shuffle of this span.
    /// </summary>
    /// <param name="list">The span to shuffle.</param>
    /// <param name="random">The <see cref="Random"/> object to suffle with.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="list"/>.</typeparam>
    /// <returns>The shuffled <paramref name="list"/>.</returns>
    public static List<T> Shuffle<T>(this List<T> list, Random? random)
    {
        random ??= Random.Shared;
        random.Shuffle(CollectionsMarshal.AsSpan(list));

        return list;
    }

    /// <summary>
    ///  Gets a <see cref="Span{T}"/> view over the data in a list. Items should not be
    ///  added or removed from the <see cref="List{T}"/> while the <see cref="Span{T}"/>
    ///  is in use.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="list">List from which to create the <see cref="Span{T}"/>.</param>
    /// <returns>A <see cref="Span{T}"/> instance over the <see cref="List{T}"/>.</returns>
    public static Span<T> AsSpan<T>(this List<T> list)
        => CollectionsMarshal.AsSpan(list);

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> view over the data in a list. Items should not be
    ///  added or removed from the <see cref="List{T}"/> while the <see cref="ReadOnlySpan{T}"/>
    ///  is in use.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="list">List from which to create the <see cref="ReadOnlySpan{T}"/>.</param>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> instance over the <see cref="List{T}"/>.</returns>
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> list)
        => CollectionsMarshal.AsSpan(list);
}