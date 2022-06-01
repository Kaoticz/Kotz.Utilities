namespace Kotz.Collections.Extensions;

/// <summary>
/// Provides extensions for common types that shouldn't be exposed to the public API.
/// </summary>
internal static class InternalExtensions
{
    /// <summary>
    /// Gets the index of the first non-<see langword="null"/> element that matches the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="predicate">Delegate that defines the conditions of the item to get.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <remarks>This method is almost a copy of the LinqExt.IndexOf method in Kotz.Extensions.</remarks>
    /// <returns>The index of the item, -1 otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    internal static int IndexOfNonNull<T>(this IReadOnlyList<T> collection, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        for (var index = 0; index < collection.Count; index++)
        {
            if (collection[index] is not null && predicate(collection[index]))
                return index;
        }

        return -1;
    }
}
