using Kotz.Collections.Extensions;
using System.Buffers;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Kotz.Collections;

/// <summary>
/// Represents an array rented from the <see cref="ArrayPool{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the items to be stored.</typeparam>
/// <remarks>
/// Use this for short-lived arrays that exceed 1000 bytes in size or methods whose Gen0 allocation exceeds 1000 bytes. <br />
/// Call <see cref="RentedArray{T}.Dispose"/> to return the array to the <see cref="ArrayPool{T}"/>.
/// </remarks>
public sealed class RentedArray<T> : IList<T>, IReadOnlyList<T>, IDisposable
{
    private T[] _internalArray;

    public bool IsReadOnly { get; } = false;

    /// <summary>
    /// The size of this <see cref="RentedArray{T}"/>.
    /// </summary>
    public int Count { get; private set; }

    public T this[int index]
    {
        get => _internalArray[index];
        set => _internalArray[index] = value;
    }

    /// <summary>
    /// Creates an array from the <see cref="ArrayPool{T}"/> buffer.
    /// </summary>
    /// <param name="collection">Collection of <typeparamref name="T"/> to be saved to the array.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public RentedArray(IEnumerable<T> collection)
    {
        if (collection is null)
            throw new ArgumentNullException(nameof(collection), "Collection must not be null.");

        var counter = 0;
        Count = (collection.TryGetNonEnumeratedCount(out var amount))
            ? amount
            : collection.Count();

        _internalArray = (Count is 0)
            ? Array.Empty<T>()
            : ArrayPool<T>.Shared.Rent(Count);

        foreach (var element in collection)
            _internalArray[counter++] = element;
    }

    public IEnumerator<T> GetEnumerator()
        => _internalArray.Cast<T>().Take(Count).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _internalArray.Take(Count).GetEnumerator();

    /// <summary>
    /// This method is not supported. Use <see cref="RentedArray{T}.Insert(int, T)"/> instead.
    /// </summary>
    /// <inheritdoc />
    public void Add(T item)
        => throw new NotSupportedException($"{nameof(RentedArray<T>)} does not support this method. Use \"{nameof(RentedArray<T>.Insert)}\" instead.");

    /// <summary>
    /// Copies all the elements of the current one-dimensional array to the specified
    /// one-dimensional array starting at the specified destination array index. The
    /// index is specified as a 32-bit integer.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
    /// <param name="arrayIndex">A 32-bit integer that represents the index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Occurs when the <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the index is less than the lower bound of the array.</exception>
    /// <exception cref="RankException">Occurs when the source <paramref name="array"/> is multidimensional.</exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="array"/> is multidimensional. -or- The number of elements in the source array is greater than
    /// the available number of elements from index to the end of the destination array.
    /// </exception>
    /// <exception cref="ArrayTypeMismatchException">
    /// Occurs when the type of the source <paramref name="array"/> cannot be cast automatically to the type of the destination array.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// Occurs when At least one element in the source System.Array cannot be cast to the type of destination array.
    /// </exception>
    public void CopyTo(T[] array, int arrayIndex)
        => _internalArray.CopyTo(array, arrayIndex);

    /// <summary>
    /// Inserts an item to the <see cref="RentedArray{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The index to insert to.</param>
    /// <param name="item">The item to be inserted.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> exceeds the array's length.</exception>
    public void Insert(int index, T item)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index), $"Index ({index}) exceeds size of array ({Count}).");

        _internalArray[index] = item;
    }

    /// <summary>
    /// Removes an item from the <see cref="RentedArray{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The index to remove from.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> exceeds the array's length.</exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index), $"Index ({index}) exceeds size of array ({Count}).");

        _internalArray[index] = default!;
    }

    /// <summary>
    /// Removes all items from this <see cref="RentedArray{T}"/>.
    /// </summary>
    public void Clear()
    {
        for (var index = 0; index < Count; index++)
            _internalArray[index] = default!;
    }

    /// <summary>
    /// Determines whether this <see cref="RentedArray{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns><see langword="true"/> if item is found, <see langword="false"/> otherwise.</returns>
    public bool Contains(T item)
    {
        for (var index = 0; index < Count; index++)
        {
            if (Equals(_internalArray[index], item))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the first occurrence of the specified object from the <see cref="RentedArray{T}"/>.
    /// </summary>
    /// <param name="item">The object to remove.</param>
    /// <returns><see langword="true"/> if the item was successfully removed, <see langword="false"/> otherwise.</returns>
    public bool Remove(T item)
    {
        for (var index = 0; index < Count; index++)
        {
            if (Equals(_internalArray[index], item))
            {
                _internalArray[index] = default!;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines the index of the specified item in the <see cref="RentedArray{T}"/>.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns>The index of the item, -1 otherwise.</returns>
    public int IndexOf(T item)
    {
        for (var index = 0; index < Count; index++)
        {
            if (Equals(_internalArray[index], item))
                return index;
        }

        return -1;
    }

    /// <summary>
    /// Safely gets the item at the speficied <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index where the item is at.</param>
    /// <param name="item">The resulting element.</param>
    /// <returns><see langword="true"/> if the element was successfully fetched, <see langword="false"/> otherwise.</returns>
    public bool TryGetValue(int index, [MaybeNullWhen(false)] out T item)
    {
        if (index < 0 || index >= Count)
        {
            item = default;
            return false;
        }

        item = _internalArray[index];
        return true;
    }

    /// <summary>
    /// Safely gets the first non-<see langword="null"/> item that meets the criteria of the speficied <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Delegate that defines the conditions of the item to get.</param>
    /// <param name="item">The resulting item.</param>
    /// <returns><see langword="true"/> if the item was successfully fetched, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public bool TryGetValue(Func<T, bool> predicate, [MaybeNullWhen(false)] out T item)
    {
        var index = _internalArray.IndexOfNonNull(predicate);
        item = (index is not -1) ? _internalArray[index] : default;

        return index is not -1 && !Equals(item, default(T));
    }

    /// <summary>
    /// Returns the <see cref="RentedArray{T}"/> to the shared <see cref="ArrayPool{T}"/>.
    /// </summary>
    public void Dispose()
    {
        if (Count is not 0)
        {
            ArrayPool<T>.Shared.Return(_internalArray, true);
            _internalArray = Array.Empty<T>();
            Count = 0;
        }

        GC.SuppressFinalize(this);
    }
}