using System.Runtime.InteropServices;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="List{T}"/>.
/// </summary>
public static class ListExt
{
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
}