using System.Diagnostics.CodeAnalysis;

namespace Kotz.Extensions.InternalUtilities;

/// <summary>
/// Internal utility methods.
/// </summary>
internal static class Utilities
{
    /// <summary>
    /// Safely gets a random element from an indexed <paramref name="collection"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">The indexed collection to retrieve from.</param>
    /// <param name="random">The random number generator.</param>
    /// <param name="randomElement">The random element.</param>
    /// <returns><see langword="true"/> if the element was returned, <see langword="false"/> otherwise.</returns>
    internal static bool TryGetRandom<T>(IEnumerable<T> collection, Random random, [MaybeNullWhen(false)] out T randomElement)
    {
        if (collection is IList<T> mutableList && mutableList.Count > 0)
        {
            randomElement = mutableList[random.Next(mutableList.Count)];
            return true;
        }

        if (collection is IReadOnlyList<T> immutableList && immutableList.Count > 0)
        {
            randomElement = immutableList[random.Next(immutableList.Count)];
            return true;
        }

        randomElement = default;
        return false;
    }

    /// <summary>
    /// Safely gets a random element from an indexed <paramref name="collection"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">The indexed collection to retrieve from.</param>
    /// <param name="random">The random number generator.</param>
    /// <param name="maxIndex">The maximum index to pick from.</param>
    /// <param name="randomElement">The random element.</param>
    /// <returns><see langword="true"/> if the element was returned, <see langword="false"/> otherwise.</returns>
    internal static bool TryGetRandom<T>(IEnumerable<T> collection, Random random, int maxIndex, [MaybeNullWhen(false)] out T randomElement)
    {
        if (collection is IList<T> mutableList && mutableList.Count > 0)
        {
            randomElement = mutableList[random.Next(Math.Min(mutableList.Count, maxIndex))];
            return true;
        }

        if (collection is IReadOnlyList<T> immutableList && immutableList.Count > 0)
        {
            randomElement = immutableList[random.Next(Math.Min(immutableList.Count, maxIndex))];
            return true;
        }

        randomElement = default;
        return false;
    }

    /// <summary>
    /// Counts the occurences of elements in a <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <returns>
    /// A dictionary with the element as the key and how many times it appears in
    ///  the <paramref name="collection"/> as the value.
    /// </returns>
    internal static Dictionary<T, uint> CountElements<T>(IEnumerable<T> collection) where T : notnull
        => CountElements(collection, x => x);

    /// <summary>
    /// Counts the occurences of elements in a <paramref name="collection"/> according to
    /// a specified key <paramref name="selector"/> function.
    /// </summary>
    /// <typeparam name="T1">The type of the elements.</typeparam>
    /// <typeparam name="T2">The selected type.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="selector">The selector function.</param>
    /// <returns>
    /// A dictionary with the selected element as the key and how many times it appears in
    /// the <paramref name="collection"/> as the value.
    /// </returns>
    internal static Dictionary<T2, uint> CountElements<T1, T2>(IEnumerable<T1> collection, Func<T1, T2> selector) where T2 : notnull
    {
        var result = new Dictionary<T2, uint>();

        foreach (var element in collection)
        {
            var newKey = selector(element);

            if (!result.TryAdd(newKey, 1))
                result[newKey]++;
        }

        return result;
    }
}