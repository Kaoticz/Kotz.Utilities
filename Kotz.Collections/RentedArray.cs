using Kotz.Collections.Debugger;
using Kotz.Collections.Extensions;
using System.Buffers;
using System.Collections;
using System.Diagnostics;
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
[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
public sealed class RentedArray<T> : IList<T>, IReadOnlyList<T>, IDisposable
{
    private T[] _internalArray;

    /// <summary>
    /// Determines whether this collection is read-only.
    /// </summary>
    bool ICollection<T>.IsReadOnly { get; }

    /// <summary>
    /// The size of this <see cref="RentedArray{T}"/>.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The index to get from or set to.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is greater than the array's capacity or less than 0.</exception>
    public T this[int index]
    {
        get => _internalArray[index];
        set => _internalArray[index] = (index >= Count) ? throw new ArgumentOutOfRangeException(nameof(index), index, "Index out of range.") : value;
    }

    /// <summary>
    /// Gets a slice of this <see cref="RentedArray{T}"/>.
    /// </summary>
    /// <returns>The specified slice of this <see cref="RentedArray{T}"/>.</returns>
    /// <remarks>This allocates a new <see cref="RentedArray{T}"/>. If you want to avoid allocations, call <see cref="AsSpan"/> first then get a slice of that.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the <paramref name="range"/> goes out of bounds of the <see cref="RentedArray{T}"/>.</exception>
    public RentedArray<T> this[Range range]
        => new(_internalArray[range]);

    /// <summary>
    /// Creates an array from the <see cref="ArrayPool{T}"/> buffer.
    /// </summary>
    /// <param name="collection">Collection of <typeparamref name="T"/> to be saved to the array.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public RentedArray(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        Count = (collection.TryGetNonEnumeratedCount(out var amount))
            ? amount
            : collection.Count();

        _internalArray = (Count is 0)
            ? []
            : ArrayPool<T>.Shared.Rent(Count);

        var counter = 0;

        foreach (var element in collection)
            _internalArray[counter++] = element;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="RentedArray{T}"/>.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a reference type, <see langword="null"/> values may be enumerared.</remarks>
    /// <returns>An enumerator for the <see cref="RentedArray{T}"/>.</returns>
    public IEnumerator<T> GetEnumerator()
        => _internalArray.Take(Count).GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="RentedArray{T}"/>.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a reference type, <see langword="null"/> values may be enumerared.</remarks>
    /// <returns>An enumerator for the <see cref="RentedArray{T}"/>.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => _internalArray.Take(Count).GetEnumerator();

    /// <summary>
    /// Creates a new <see cref="Span{T}"/> over this array.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a reference type, the span may contain <see langword="null"/> values.</remarks>
    /// <returns>The <see cref="Span{T}"/> representation of the array.</returns>
    public Span<T> AsSpan()
        => _internalArray.AsSpan()[..Count];

    /// <summary>
    /// Creates a new <see cref="ReadOnlySpan{T}"/> over this array.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a reference type, the span may contain <see langword="null"/> values.</remarks>
    /// <returns>The <see cref="ReadOnlySpan{T}"/> representation of the array.</returns>
    public ReadOnlySpan<T> AsReadOnlySpan()
        => _internalArray.AsSpan()[..Count];

    /// <summary>
    /// This method is not supported. Use <see cref="RentedArray{T}.Insert(int, T)"/> instead.
    /// </summary>
    /// <exception cref="NotSupportedException" />
    void ICollection<T>.Add(T item)
        => throw new NotSupportedException($"{nameof(RentedArray<T>)} does not support this method. Use \"{nameof(RentedArray<T>.Insert)}\" instead.");

    /// <summary>
    /// Copies all the elements of the current one-dimensional array to the specified
    /// one-dimensional array starting at the specified destination array index. The
    /// index is specified as a 32-bit integer.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
    /// <param name="arrayIndex">A 32-bit integer that represents the index in array at which copying begins.</param>
    /// <remarks>If <typeparamref name="T"/> is a reference type, the copied array may contain <see langword="null"/> values.</remarks>
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
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));

        if (Count is 0)
            return;
        else if (arrayIndex < 0 || arrayIndex >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, $"Index ({arrayIndex}) is out of bounds (array's size: {Count}).");

        var span = AsReadOnlySpan();
        var targetSpan = array.AsSpan()[arrayIndex..];

        if (span.Length > targetSpan.Length)
            throw new ArgumentException($"Source collection is too big ({span.Length}) for the target collection ({targetSpan.Length}). Provide a bigger collection or decrease the value of arrayIndex ({arrayIndex}).");

        span.CopyTo(targetSpan);
    }

    /// <summary>
    /// Inserts an item to the <see cref="RentedArray{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The index to insert to.</param>
    /// <param name="item">The item to be inserted.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> exceeds the array's length.</exception>
    public void Insert(int index, T item)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index exceeds size of array ({Count}).");

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
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index exceeds size of array ({Count}).");

        _internalArray[index] = default!;
    }

    /// <summary>
    /// Removes all items from this <see cref="RentedArray{T}"/>.
    /// </summary>
    public void Clear()
        => _internalArray.AsSpan().Clear();

    /// <summary>
    /// Determines whether this <see cref="RentedArray{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns><see langword="true"/> if item is found, <see langword="false"/> otherwise.</returns>
    public bool Contains(T item)
    {
        for (var index = 0; index < Count; index++)
        {
            if (EqualityComparer<T>.Default.Equals(_internalArray[index], item))
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
            if (EqualityComparer<T>.Default.Equals(_internalArray[index], item))
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
            if (EqualityComparer<T>.Default.Equals(_internalArray[index], item))
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

        return index is not -1 && item is not null;
    }

    /// <summary>
    /// Returns the <see cref="RentedArray{T}"/> to the shared <see cref="ArrayPool{T}"/>.
    /// </summary>
    public void Dispose()
    {
        InternalDispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Clears and returns the internal rented array to the <see cref="ArrayPool{T}"/>.
    /// </summary>
    private void InternalDispose()
    {
        if (Count is 0)
            return;

        ArrayPool<T>.Shared.Return(_internalArray, true);
        _internalArray = [];
        Count = 0;
    }

    /// <summary>
    /// Disposes and finalizes this rented array.
    /// </summary>
    ~RentedArray()
        => InternalDispose();
}