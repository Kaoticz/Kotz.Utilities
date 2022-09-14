using System.Collections.Concurrent;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class LinqExt
{
    /// <summary>
    /// Applies a deferred <paramref name="action"/> on a collection if the <paramref name="predicate"/> is <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="predicate">The condition to be checked.</param>
    /// <param name="action">The action to be performed.</param>
    /// <returns>The modified collection if <paramref name="predicate"/> is <see langword="true"/>, otherwise the original collection.</returns>
    public static IEnumerable<T> When<T>(this IEnumerable<T> collection, Predicate<IEnumerable<T>> predicate, Func<IEnumerable<T>, IEnumerable<T>> action)
        => (predicate(collection)) ? action(collection) : collection;

    /// <summary>
    /// Saves an <see cref="IEnumerable{T2}"/> collection to a concurrent dictionary.
    /// </summary>
    /// <typeparam name="T1">Type of the key.</typeparam>
    /// <typeparam name="T2">Type of the value.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <param name="keySelector">A method that defines the value to be used as the key for the dictionary.</param>
    /// <returns>A <see cref="ConcurrentDictionary{T1, T2}"/> whose keys are defined by <paramref name="keySelector"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either the collection or the key selector are <see langword="null"/>.</exception>
    public static ConcurrentDictionary<T1, T2> ToConcurrentDictionary<T1, T2>(this IEnumerable<T2> collection, Func<T2, T1> keySelector) where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var result = new ConcurrentDictionary<T1, T2>();

        foreach (var value in collection)
            result.TryAdd(keySelector(value), value);

        return result;
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T2}"/> collection to a concurrent dictionary.
    /// </summary>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <param name="keySelector">A method that defines the value to be used as the key for the dictionary.</param>
    /// <param name="elementSelector">A method that defines the value to be used as the value for the dictionary.</param>
    /// <typeparam name="T1">Type of the key.</typeparam>
    /// <typeparam name="T2">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <typeparam name="T3">Type of the value.</typeparam>
    /// <returns>A <see cref="ConcurrentDictionary{T1, T3}"/> whose keys are defined by <paramref name="keySelector"/> and values are defined by <paramref name="elementSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either the collection, or the key selector, or the element selector are <see langword="null"/>.</exception>
    public static ConcurrentDictionary<T1, T3> ToConcurrentDictionary<T1, T2, T3>(this IEnumerable<T2> collection, Func<T2, T1> keySelector, Func<T2, T3> elementSelector) where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

        var result = new ConcurrentDictionary<T1, T3>();

        foreach (var value in collection)
            result.TryAdd(keySelector(value), elementSelector(value));

        return result;
    }

    /// <summary>
    /// Checks if the current collection contains all elements of a given collection.
    /// </summary>
    /// <typeparam name="T">Data type contained in the collection.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="targetCollection">The collection to compare to.</param>
    /// <returns>
    /// <see langword="true"/> if all elements contained in <paramref name="targetCollection"/> are present
    /// in <paramref name="collection"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when either collections are <see langword="null"/>.</exception>
    public static bool ContainsSubcollection<T>(this IEnumerable<T> collection, IEnumerable<T> targetCollection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(targetCollection, nameof(targetCollection));

        if (!collection.Any() || !targetCollection.Any())
            return false;

        var matches = 0;

        foreach (var element in targetCollection)
        {
            if (collection.Any(x => Equals(x, element)))
                matches++;
        }

        return matches == targetCollection.Count();
    }

    /// <summary>
    /// Checks if the current collection contains at least one element of a given collection.
    /// </summary>
    /// <typeparam name="T">Data type contained in the collection.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="targetCollection">The collection to compare to.</param>
    /// <returns>
    /// <see langword="true"/> if at least one element contained in <paramref name="targetCollection"/> is present
    /// in <paramref name="collection"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when either collections are <see langword="null"/>.</exception>
    public static bool ContainsOne<T>(this IEnumerable<T> collection, params T[] targetCollection)
        => collection.ContainsOne(targetCollection.AsEnumerable());

    /// <summary>
    /// Checks if the current collection contains at least one element of a given collection.
    /// </summary>
    /// <typeparam name="T">Data type contained in the collection.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="targetCollection">The collection to compare to.</param>
    /// <returns>
    /// <see langword="true"/> if at least one element contained in <paramref name="targetCollection"/> is present
    /// in <paramref name="collection"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when either collections are <see langword="null"/>.</exception>
    public static bool ContainsOne<T>(this IEnumerable<T> collection, IEnumerable<T> targetCollection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(targetCollection, nameof(targetCollection));

        foreach (var element in targetCollection)
        {
            if (collection.Any(x => Equals(x, element)))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns when all of them have completed.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task WhenAllAsync(this IEnumerable<Task> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        await Task.WhenAll(collection).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns their results in an array.
    /// </summary>
    /// <typeparam name="T">The data that needs to be awaited.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task<T[]> WhenAllAsync<T>(this IEnumerable<Task<T>> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return await Task.WhenAll(collection).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns when any of them have completed.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task WhenAnyAsync(this IEnumerable<Task> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        await (await Task.WhenAny(collection).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits the first task that completes in the current collection and returns its result.
    /// </summary>
    /// <typeparam name="T">The data that needs to be awaited.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>The <typeparamref name="T"/> object of the task that first finished executing.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task<T> WhenAnyAsync<T>(this IEnumerable<Task<T>> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return await (await Task.WhenAny(collection).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the symmetric difference between two collections based on the key defined by <paramref name="keySelector"/>.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="secondCollection">The second collection to compare with.</param>
    /// <param name="keySelector">A method that defines the property to filter by.</param>
    /// <typeparam name="T1">Data type contained in the collection.</typeparam>
    /// <typeparam name="T2">Data type of the property to be selected.</typeparam>
    /// <returns>A collection of <typeparamref name="T1"/> with the symmetric difference between this <paramref name="collection"/> and <paramref name="secondCollection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either of the parameters is <see langword="null"/>.</exception>
    public static IEnumerable<T1> ExceptBy<T1, T2>(this IEnumerable<T1> collection, IEnumerable<T1> secondCollection, Func<T1, T2> keySelector)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(secondCollection, nameof(secondCollection));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var seenKeys = new HashSet<T2>(collection.Intersect(secondCollection).Select(x => keySelector(x)));

        foreach (var element in collection)
        {
            if (seenKeys.Add(keySelector(element)))
                yield return element;
        }

        foreach (var element in secondCollection)
        {
            if (seenKeys.Add(keySelector(element)))
                yield return element;
        }

        seenKeys.Clear();
    }

    /// <summary>
    /// Gets all elements present in this <paramref name="collection"/> and <paramref name="secondCollection"/>
    /// that share the same property defined by <paramref name="keySelector"/>.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="secondCollection">The collection to be intersected with.</param>
    /// <param name="keySelector">A method that defines the property to filter by.</param>
    /// <typeparam name="T1">Data type contained in the collection.</typeparam>
    /// <typeparam name="T2">Data type of the property to be selected.</typeparam>
    /// <returns>A collection of intersected <typeparamref name="T1"/> objects.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either of the parameters is <see langword="null"/>.</exception>
    public static IEnumerable<T1> IntersectBy<T1, T2>(this IEnumerable<T1> collection, IEnumerable<T1> secondCollection, Func<T1, T2> keySelector)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(secondCollection, nameof(secondCollection));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var seenKeys = new HashSet<T2>(collection.Select(x => keySelector(x)));
        seenKeys.IntersectWith(secondCollection.Select(x => keySelector(x)));

        foreach (var element in collection.Concat(secondCollection).DistinctBy(x => keySelector(x)))
        {
            if (!seenKeys.Add(keySelector(element)))
                yield return element;
        }

        seenKeys.Clear();
    }

    /// <summary>
    /// Adds the <typeparamref name="T1"/> defined in <paramref name="sample"/> to the inner collections
    /// of this <see cref="IEnumerable{T}"/> until all of them reach the same amount of elements.
    /// </summary>
    /// <param name="collection">This collection of collections of <typeparamref name="T1"/>.</param>
    /// <param name="sample">The <typeparamref name="T1"/> object to be added to the inner collections.</param>
    /// <typeparam name="T1">Data type contained in the inner collections.</typeparam>
    /// <typeparam name="T2">The type of collections stored.</typeparam>
    /// <returns>A <see cref="List{T}"/> with <see cref="IEnumerable{T}"/> collections of the same size.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either of the parameters is <see langword="null"/>.</exception>
    public static List<List<T1>> NestedFill<T1, T2>(this IEnumerable<T2> collection, T1 sample) where T2 : IEnumerable<T1>
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(sample, nameof(sample));

        var outerCollection = collection.Select(x => x.ToList()).ToList();

        // Get the max count of the inner collections
        var max = 0;
        foreach (var innerCollection in outerCollection)
            max = Math.Max(max, innerCollection.Count);

        // Fill the collections until they have the same size
        for (var index = 0; index < outerCollection.Count; index++)
        {
            while (outerCollection[index].Count != max)
                outerCollection[index].Add(sample);
        }

        return outerCollection;
    }

    /// <summary>
    /// Splits the elements of this <paramref name="collection"/> into several subcollections according to the value of the property defined by <paramref name="selector"/>.
    /// </summary>
    /// <typeparam name="T1">Type of the elements.</typeparam>
    /// <typeparam name="T2">Type of the selected property.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="selector">A method that defines the property to filter the elements.</param>
    /// <returns>An <see cref="IEnumerable{T1}"/> where all <typeparamref name="T1"/> have the same value for the property defined by <paramref name="selector"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection or the selector are <see langword="null"/>.</exception>
    public static IEnumerable<IEnumerable<T1>> ChunkBy<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector) where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return collection
            .Select(x => (Value: selector(x), Collection: collection.Where(y => selector(y).Equals(selector(x)))))
            .DistinctBy(x => x.Value)
            .Select(x => x.Collection);
    }

    /// <summary>
    /// Gets a random <typeparamref name="T"/> from the current collection.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="random">A <see cref="Random"/> instance to generate the random index.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>A random <typeparamref name="T"/> element from this collection.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="collection"/> is empty.</exception>
    public static T RandomElement<T>(this IEnumerable<T> collection, Random? random = default)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        random ??= Random.Shared;
        return collection.ElementAt(random.Next(collection.Count()));
    }

    /// <summary>
    /// Gets a random <typeparamref name="T"/> from the current collection.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="maxIndex">The maximum index to pick from.</param>
    /// <param name="random">A <see cref="Random"/> instance to generate the random index.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>A random <typeparamref name="T"/> element from this collection.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="collection"/> is empty.</exception>
    public static T RandomElement<T>(this IEnumerable<T> collection, int maxIndex, Random? random = default)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        random ??= Random.Shared;
        return collection.ElementAt(random.Next(Math.Min(collection.Count(), Math.Abs(maxIndex))));
    }

    /// <summary>
    /// Gets a random <typeparamref name="T"/> from the current collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="random">A <see cref="Random"/> instance to generate the random index.</param>
    /// <returns>A random <typeparamref name="T"/> element from this collection or <see langword="default"/>(<typeparamref name="T"/>) if the collection is empty.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? RandomElementOrDefault<T>(this IEnumerable<T> collection, Random? random = default)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        random ??= Random.Shared;
        return collection.ElementAtOrDefault(random.Next(collection.Count()));
    }

    /// <summary>
    /// Gets a random <typeparamref name="T"/> from the current collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="maxIndex">The maximum index to pick from.</param>
    /// <param name="random">A <see cref="Random"/> instance to generate the random index.</param>
    /// <returns>A random <typeparamref name="T"/> element from this collection or <see langword="default"/>(<typeparamref name="T"/>) if the collection is empty.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? RandomElementOrDefault<T>(this IEnumerable<T> collection, int maxIndex, Random? random = default)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        random ??= Random.Shared;
        return collection.ElementAtOrDefault(random.Next(Math.Min(collection.Count(), Math.Abs(maxIndex))));
    }

    /// <summary>
    /// Gets the symetric difference between all elements in the current and specified collections.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="collections">The collections to get the difference from.</param>
    /// <returns>The symetric difference of all collections.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the either of the collections are <see langword="null"/>.</exception>
    public static IEnumerable<T> Unique<T>(this IEnumerable<T> collection, params IEnumerable<T>[] collections)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(collections, nameof(collections));

        return collection
            .Concat(collections.SelectMany(x => x))
            .Except(collections.SelectMany(x => x).Intersect(collection))
            .Distinct();
    }

    /// <summary>
    /// Checks if the current collection contains at least the specified amount of elements
    /// and exits early if it does.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="minAmount">The minimum amount of elements the <paramref name="collection"/> should contain.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>
    /// <see langword="true"/> if <paramref name="collection"/> has at least the specified
    /// amount of elements, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="minAmount"/> is lower or equal to zero.</exception>
    public static bool AtLeast<T>(this IEnumerable<T> collection, int minAmount)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        if (minAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(minAmount), minAmount, "Amount must be higher than 0.");

        if (collection is ICollection<T> mutableCollection)
            return mutableCollection.Count >= minAmount;
        else if (collection is IReadOnlyCollection<T> immutableCollection)
            return immutableCollection.Count >= minAmount;

        var counter = 0;
        using var enumerator = collection.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (++counter >= minAmount)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the current collection contains at least the specified amount of elements
    /// that satisfy the specified <paramref name="predicate"/> and exits early if it does.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <param name="minAmount">The minimum amount of elements the <paramref name="collection"/> should contain.</param>
    /// <param name="predicate">The predicate each element should satisfy.</param>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <returns>
    /// <see langword="true"/> if <paramref name="collection"/> has at least the specified
    /// amount of elements that satisfy the <paramref name="predicate"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="predicate"/> are <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="minAmount"/> is lower or equal to zero.</exception>
    public static bool AtLeast<T>(this IEnumerable<T> collection, int minAmount, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        if (minAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(minAmount), minAmount, "Amount must be higher than 0.");

        var counter = 0;

        foreach (var element in collection)
        {
            if (predicate(element) && ++counter >= minAmount)
                return true;
        }

        return false;
    }
}