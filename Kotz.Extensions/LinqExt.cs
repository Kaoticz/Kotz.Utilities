using Kotz.Extensions.InternalUtilities;
using System.ComponentModel;

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
    public static IEnumerable<T> When<T>(this IEnumerable<T> collection, Func<IEnumerable<T>, bool> predicate, Func<IEnumerable<T>, IEnumerable<T>> action)
        => (predicate(collection)) ? action(collection) : collection;

    /// <summary>
    /// Conditionally applies a deferred action to a collection according to the return value of the <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="predicate">The condition to be checked.</param>
    /// <param name="action">The action to be performed if <paramref name="predicate"/> returns <see langword="true"/>.</param>
    /// <param name="elseAction">The action to be performed if <paramref name="predicate"/> returns <see langword="false"/>.</param>
    /// <returns>The collection modified by one of the actions.</returns>
    public static IEnumerable<T> When<T>(this IEnumerable<T> collection, Func<IEnumerable<T>, bool> predicate, Func<IEnumerable<T>, IEnumerable<T>> action, Func<IEnumerable<T>, IEnumerable<T>> elseAction)
        => (predicate(collection)) ? action(collection) : elseAction(collection);

    /// <summary>
    /// Produces a sequence of tuples with elements from the two specified sequences.
    /// </summary>
    /// <typeparam name="T1">The type of the elements of the first input sequence.</typeparam>
    /// <typeparam name="T2">The type of the elements of the second input sequence.</typeparam>
    /// <param name="firstCollection">The first sequence to merge.</param>
    /// <param name="secondCollection">The second sequence to merge.</param>
    /// <returns>A sequence of tuples with elements taken from the first and second sequences, in that order.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either collections are <see langword="null"/>.</exception>
    public static IEnumerable<(T1 First, T2 Second)> Zip<T1, T2>(this IEnumerable<T1> firstCollection, params T2[] secondCollection)
        => firstCollection.Zip<T1, T2>(secondCollection.AsEnumerable());

    /// <summary>
    /// Produces a sequence of tuples with elements from the two specified sequences. If one of the
    /// sequences is larger than the other, the exceeding elements from the larger sequence will be
    /// paired with default values.
    /// </summary>
    /// <typeparam name="T1">The type of the elements of the first input sequence.</typeparam>
    /// <typeparam name="T2">The type of the elements of the second input sequence.</typeparam>
    /// <param name="firstCollection">The first sequence to merge.</param>
    /// <param name="secondCollection">The second sequence to merge.</param>
    /// <param name="firstDefault">The default value for the first sequence.</param>
    /// <param name="secondDefault">The default value for the second sequence.</param>
    /// <returns>A sequence of tuples with elements taken from the first and second sequences, in that order.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either collections are <see langword="null"/>.</exception>
    public static IEnumerable<(T1? First, T2? Second)> ZipOrDefault<T1, T2>(this IEnumerable<T1> firstCollection, IEnumerable<T2> secondCollection, T1? firstDefault = default, T2? secondDefault = default)
    {
        ArgumentNullException.ThrowIfNull(firstCollection, nameof(firstCollection));
        ArgumentNullException.ThrowIfNull(secondCollection, nameof(secondCollection));

        using var firstEnumerator = firstCollection.GetEnumerator();
        using var secondEnumerator = secondCollection.GetEnumerator();

        while (true)
        {
            var canFirstMove = firstEnumerator.MoveNext();
            var canSecondMove = secondEnumerator.MoveNext();

            // If neither collections can be enumerated, exit.
            if (!canFirstMove && !canSecondMove)
                break;

            yield return (
                (canFirstMove) ? firstEnumerator.Current : firstDefault,
                (canSecondMove) ? secondEnumerator.Current : secondDefault
            );
        }
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

        return collection
            .Intersect(targetCollection)
            .Any();
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

        return collection.Any() && targetCollection.Any() && targetCollection.All(x => collection.Contains(x));
    }

    /// <summary>
    /// Adds the <typeparamref name="T1"/> defined in <paramref name="sample"/> to the inner collections
    /// of this <see cref="IEnumerable{T}"/> until all of them reach the same amount of elements.
    /// </summary>
    /// <param name="collection">This collection of collections of <typeparamref name="T1"/>.</param>
    /// <param name="sample">The <typeparamref name="T1"/> object to be added to the inner collections.</param>
    /// <typeparam name="T1">Data type contained in the inner collections.</typeparam>
    /// <typeparam name="T2">The type of collections stored.</typeparam>
    /// <returns>A <see cref="IReadOnlyList{T}"/> of lists with the same size.</returns>
    /// <exception cref="ArgumentNullException">Occurs when either of the parameters is <see langword="null"/>.</exception>
    public static IReadOnlyList<IReadOnlyList<T1>> NestedFill<T1, T2>(this IEnumerable<T2> collection, T1 sample) where T2 : IEnumerable<T1>
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

        return (Utilities.TryGetRandom(collection, random, out var randomElement))
            ? randomElement
            : collection.ElementAt(random.Next(collection.Count()));
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

        return (Utilities.TryGetRandom(collection, random, maxIndex, out var randomElement))
            ? randomElement
            : collection.ElementAt(random.Next(Math.Min(collection.Count(), Math.Abs(maxIndex))));
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

        return (Utilities.TryGetRandom(collection, random, out var randomElement))
            ? randomElement
            : default;
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

        return (Utilities.TryGetRandom(collection, random, maxIndex, out var randomElement))
            ? randomElement
            : default;
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
            throw new ArgumentOutOfRangeException(nameof(minAmount), minAmount, "Amount must be greater than 0.");

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
            throw new ArgumentOutOfRangeException(nameof(minAmount), minAmount, "Amount must be greater than 0.");

        var counter = 0;

        foreach (var element in collection)
        {
            if (predicate(element) && ++counter >= minAmount)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Splits the current <paramref name="collection"/> into multiple collections based on the specified <paramref name="separator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="separator">The value to split the collection on.</param>
    /// <param name="comparer">The comparer for <typeparamref name="T"/>.</param>
    /// <remarks>The <paramref name="separator"/> is not included in the final result.</remarks>
    /// <returns>Subcollections of this <paramref name="collection"/> delimited by the specified <paramref name="separator"/>.</returns>
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, T separator, IEqualityComparer<T>? comparer = default)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        if (!collection.Any())
        {
            yield return collection;
            yield break;
        }

        comparer ??= EqualityComparer<T>.Default;
        var currentIndex = 0;
        var lastMatchIndex = 0;

        foreach (var element in collection)
        {
            if (comparer.Equals(element, separator))
            {
                var result = collection
                    .Skip(lastMatchIndex)
                    .Take(currentIndex - lastMatchIndex);

                if (result.Any())
                    yield return result;

                lastMatchIndex = currentIndex + 1;
            }

            currentIndex++;
        }

        if (currentIndex - lastMatchIndex > 0)
            yield return collection.TakeLast(currentIndex - lastMatchIndex);
    }

    /// <summary>
    /// Sorts the elements of a sequence in ascending order according to how many times they appear in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to their frequency in the sequence.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static IOrderedEnumerable<T> OrderAmount<T>(this IEnumerable<T> collection) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        var result = Utilities.CountElements(collection);
        return collection.OrderBy(x => result[x]);
    }

    /// <summary>
    /// Sorts the elements of a sequence in ascending order according to how many times the selected property appear in the sequence.
    /// </summary>
    /// <typeparam name="T1">The type of the elements.</typeparam>
    /// <typeparam name="T2">The type of the selected property.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="selector">The selector function.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to the frequency of the selected properties.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="selector"/> are <see langword="null"/>.</exception>
    public static IOrderedEnumerable<T1> OrderByAmount<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector) where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var result = Utilities.CountElements(collection, selector);
        return collection.OrderBy(x => result[selector(x)]);
    }

    /// <summary>
    /// Sorts the elements of a sequence in descending order according to how many times they appears in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to their frequency in the sequence.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static IOrderedEnumerable<T> OrderDescendingAmount<T>(this IEnumerable<T> collection) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        var result = Utilities.CountElements(collection);
        return collection.OrderByDescending(x => result[x]);
    }

    /// <summary>
    /// Sorts the elements of a sequence in descending order according to how many times the selected property appears in the sequence.
    /// </summary>
    /// <typeparam name="T1">The type of the elements.</typeparam>
    /// <typeparam name="T2">The type of the selected property.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="selector">The selector function.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to the frequency of the selected properties.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="selector"/> are <see langword="null"/>.</exception>
    public static IOrderedEnumerable<T1> OrderByDescendingAmount<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector) where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var result = Utilities.CountElements(collection, selector);
        return collection.OrderByDescending(x => result[selector(x)]);
    }

    /// <summary>
    /// Executes an <paramref name="action"/> on every element of this <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="action">The action to be performed.</param>
    /// <remarks>This method has poor performance and should only be used for testing purposes!</remarks>
    /// <returns>This <paramref name="collection"/> unaltered.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="action"/> are <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)] 
    public static IEnumerable<T> Tap<T>(this IEnumerable<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        foreach (var element in collection)
            action(element);

        return collection;
    }

    /// <summary>
    /// Returns the maximum value in a generic sequence or <see langword="default"/> if the sequence is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>The maximum value in the sequence or the default value for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? MaxOrDefault<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return collection
            .DefaultIfEmpty()
            .Max();
    }

    /// <summary>
    /// Returns the maximum value in a generic sequence according to a specified key <paramref name="selector"/>
    /// function or <see langword="default"/> if the sequence is empty.
    /// </summary>
    /// <typeparam name="T1">The type of the elements.</typeparam>
    /// <typeparam name="T2">The type of the selected key.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="selector">A function to extract the key for each element.</param>
    /// <returns>The maximum value in the sequence or the default value for <typeparamref name="T1"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="selector"/> are <see langword="null"/>.</exception>
    public static T1? MaxByOrDefault<T1, T2>(this IEnumerable<T1> collection, Func<T1?, T2> selector)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return collection
            .DefaultIfEmpty()
            .MaxBy(selector);
    }

    /// <summary>
    /// Returns the minimum value in a generic sequence or <see langword="default"/> if the sequence is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>The minimum value in the sequence or the default value for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? MinOrDefault<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return collection
            .DefaultIfEmpty()
            .Min();
    }

    /// <summary>
    /// Returns the minimum value in a generic sequence according to a specified key <paramref name="selector"/>
    /// function or <see langword="default"/> if the sequence is empty.
    /// </summary>
    /// <typeparam name="T1">The type of the elements.</typeparam>
    /// <typeparam name="T2">The type of the selected key.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <param name="selector">A function to extract the key for each element.</param>
    /// <returns>The minimum value in the sequence or the default value for <typeparamref name="T1"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> or <paramref name="selector"/> are <see langword="null"/>.</exception>
    public static T1? MinByOrDefault<T1, T2>(this IEnumerable<T1> collection, Func<T1?, T2> selector)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return collection
            .DefaultIfEmpty()
            .MinBy(selector);
    }
}