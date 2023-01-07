using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Kotz.Extensions;

/// <summary>
/// Provides "To" extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class ToLinqExt
{
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
    /// Saves an <see cref="IEnumerable{T}"/> collection to a queue.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>A <see cref="Queue{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return new(collection);
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to a concurrent queue.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>A <see cref="ConcurrentQueue{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static ConcurrentQueue<T> ToConcurrentQueue<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return new(collection);
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to an immutable queue.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>An <see cref="ImmutableQueue{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static ImmutableQueue<T> ToImmutableQueue<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return ImmutableQueue.CreateRange(collection);
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to a priority queue.
    /// </summary>
    /// <typeparam name="T1">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <typeparam name="T2">Data type for the element's priority.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <param name="prioritySelector">A function that defines the priority value of an element.</param>
    /// <returns>A <see cref="PriorityQueue{TElement, TPriority}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection or the priority selector are <see langword="null"/>.</exception>
    public static PriorityQueue<T1, T2> ToPriorityQueue<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> prioritySelector)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(prioritySelector, nameof(prioritySelector));

        var result = new PriorityQueue<T1, T2>();

        foreach (var element in collection)
            result.Enqueue(element, prioritySelector(element));

        return result;
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to a stack.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>A <see cref="Stack{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static Stack<T> ToStack<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return new(collection);
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to a concurrent stack.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>A <see cref="ConcurrentStack{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static ConcurrentStack<T> ToConcurrentStack<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return new(collection);
    }

    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to an immutable stack.
    /// </summary>
    /// <typeparam name="T">Data type contained in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">This IEnumerable collection.</param>
    /// <returns>An <see cref="ImmutableStack{T}"/> with the elements from the <paramref name="collection"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static ImmutableStack<T> ToImmutableStack<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return ImmutableStack.CreateRange(collection);
    }
}