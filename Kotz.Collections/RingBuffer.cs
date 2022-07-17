using Kotz.Collections.Extensions;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kotz.Collections;

/// <summary>
/// Represents a fixed-size collection of <typeparamref name="T"/> objects that can be accessed by index.
/// Writing always moves forward and, when it hits the end of the buffer, it starts writing from the beginning,
/// overwriting old data. The buffer can be manually resized.
/// </summary>
/// <typeparam name="T">The data type of the elements stored in this buffer.</typeparam>
public sealed class RingBuffer<T> : IList<T>, IReadOnlyList<T>
{
    /// <summary>
    /// The backing collection of this data structure.
    /// </summary>
    private readonly List<T> _internalList;

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The index to get from or set to.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is greater than the buffer's capacity or less than 0.</exception>
    public T this[int index]
    {
        get => _internalList[index];
        set => _internalList[index] = value;
    }

    /// <summary>
    /// Returns the amount of indices that are not storing the default value for <typeparamref name="T"/>.
    /// </summary>
    public int Count
        => _internalList.Count(x => !Equals(x, default(T)));

    /// <summary>
    /// Gets or sets the maximum amount of items the internal data structure can hold before
    /// writing cycles back to the beginning.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is less than 0.</exception>
    /// <exception cref="OutOfMemoryException">Occurs when the system has no free memory available for allocation.</exception>
    public int Capacity
    {
        get => _internalList.Capacity;
        set => Resize(value);
    }

    /// <summary>
    /// Determines whether this collection is read-only.
    /// </summary>
    public bool IsReadOnly { get; } = false;

    /// <summary>
    /// Defines the current index of the buffer for writing.
    /// </summary>
    public int CurrentIndex { get; private set; }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    public RingBuffer()
    {
        _internalList = new();
        FillWithDefault(_internalList);
    }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the buffer with.</param>
    public RingBuffer(IEnumerable<T> collection)
        => _internalList = new(collection);

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="capacity">The size of the ring buffer.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="capacity"/> is greater than the buffer's capacity or less than 0.</exception>
    public RingBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), $"Capacity cannot be equal or lower than zero. Value: {capacity}");

        _internalList = new(capacity);
        FillWithDefault(_internalList);
    }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the buffer with.</param>
    /// <param name="capacity">The size of the ring buffer.</param>
    /// <remarks>Capacity takes precedence over the <paramref name="collection"/>'s size.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="capacity"/> is greater than the buffer's capacity or less than 0.</exception>
    public RingBuffer(IEnumerable<T> collection, int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), $"Capacity cannot be equal or lower than zero. Value: {capacity}");

        _internalList = new(collection);
        Resize(capacity);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => _internalList.GetEnumerator();

    public IEnumerator<T> GetEnumerator()
        => _internalList.GetEnumerator();

    /// <summary>
    /// Removes all items from the <see cref="RingBuffer{T}"/> by setting them to
    /// default value of <typeparamref name="T"/>.
    /// </summary>
    public void Clear()
        => Reset(_internalList);

    /// <summary>
    ///  Determines whether the <see cref="RingBuffer{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="RingBuffer{T}"/>.</param>
    /// <returns><see langword="true"/> if item is found, <see langword="false"/> otherwise.</returns>
    public bool Contains(T item)
        => _internalList.Contains(item);

    /// <summary>
    /// Copies the elements of the <see cref="RingBuffer{T}"/> to an <see cref="Array"/>, starting at a particular index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the <see cref="RingBuffer{T}"/>.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentException">
    /// Occurs when the number of elements in the source <see cref="RingBuffer{T}"/> is greater than the available space from <paramref name="arrayIndex"/>
    /// to the end of the destination <paramref name="array"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Occurs when <paramref name="arrayIndex"/> is greater than the length of the <paramref name="array"/>.</exception>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is greater than the buffer's capacity or less than 0.</exception>
    public void CopyTo(T[] array, int arrayIndex)
        => _internalList.CopyTo(array, arrayIndex);

    /// <summary>
    /// Determines the index of a specific item in the <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="RingBuffer{T}"/>.</param>
    /// <returns>The index of the item if it's found in the list, -1 otherwise.</returns>
    public int IndexOf(T item)
        => _internalList.IndexOf(item);

    /// <summary>
    /// Inserts an item to the <see cref="RingBuffer{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which item should be inserted.</param>
    /// <param name="item">The object to insert into the <see cref="RingBuffer{T}"/>.</param>
    /// <remarks>The buffer size remains unchanged.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is greater than the buffer's capacity or less than 0.</exception>
    public void Insert(int index, T item)
        => _internalList[index] = item;

    /// <summary>
    /// Removes the <typeparamref name="T"/> item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is greater than the buffer's capacity or less than 0.</exception>
    public void RemoveAt(int index)
        => _internalList[index] = default!;

    /// <summary>
    /// Adds an item to the <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="RingBuffer{T}"/>.</param>
    public void Add(T item)
    {
        if (CurrentIndex >= _internalList.Capacity)
            CurrentIndex = 0;

        _internalList[CurrentIndex++] = item;
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="RingBuffer{T}"/>.</param>
    /// <returns><see langword="true"/> if item was successfully removed from the <see cref="RingBuffer{T}"/>, <see langword="false"/> otherwise.</returns>
    public bool Remove(T item)
    {
        var index = _internalList.IndexOf(item);

        if (index is not -1)
            _internalList[index] = default!;

        return index is not -1;
    }

    /// <summary>
    /// Removes all elements that match the conditions of the specified predicate.
    /// </summary>
    /// <param name="predicate">Delegate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of elements removed from the <see cref="RingBuffer{T}"/>.</returns>
    public int Remove(Func<T, bool> predicate)
    {
        var amount = 0;
        var listSpan = CollectionsMarshal.AsSpan(_internalList);

        for (var index = 0; index < _internalList.Capacity; index++)
        {
            if (predicate(listSpan[index]))
            {
                listSpan[index] = default!;
                amount++;
            }
        }

        return amount;
    }

    /// <summary>
    /// Resizes this buffer to the specified size.
    /// </summary>
    /// <param name="newSize">The new size of the ring buffer.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/> is less than 0.</exception>
    /// <exception cref="OutOfMemoryException">Occurs when the system has no free memory available for allocation.</exception>
    public void Resize(int newSize)
    {
        if (newSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(newSize), $"Ring buffer cannot be resized to zero or negative values. Value: {newSize}");

        lock (_internalList)
        {
            if (newSize > _internalList.Capacity)
            {
                _internalList.Capacity = newSize;
                FillWithDefault(_internalList);
            }
            else if (newSize < _internalList.Capacity)
            {
                _internalList.RemoveRange(newSize, _internalList.Capacity - newSize);
                _internalList.Capacity = newSize;

                if (CurrentIndex >= newSize)
                    CurrentIndex = newSize - 1;
            }
        }
    }

    /// <summary>
    /// Safely gets the first non-<see langword="null"/> element that meets the criteria of the speficied <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Delegate that defines the conditions of the element to get.</param>
    /// <param name="item">The resulting element.</param>
    /// <returns><see langword="true"/> if the element was successfully fetched, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public bool TryGetValue(Func<T, bool> predicate, [MaybeNullWhen(false)] out T item)
    {
        var index = _internalList.IndexOfNonNull(predicate);
        item = (index is not -1) ? _internalList[index] : default;

        return index is not -1 && !Equals(item, default(T));
    }

    /// <summary>
    /// Fills the specified list with default <typeparamref name="T"/> values up to its capacity.
    /// </summary>
    /// <param name="list">The list to be filled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FillWithDefault(List<T> list)
    {
        lock (_internalList)
        {
            for (var index = list.Count; index < list.Capacity; index++)
                list.Add(default!);
        }
    }

    /// <summary>
    /// Sets all elements of the specified list to the default <typeparamref name="T"/> value.
    /// </summary>
    /// <param name="list">The list to be reset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Reset(List<T> list)
    {
        var listSpan = CollectionsMarshal.AsSpan(list);

        for (var index = 0; index < list.Capacity; index++)
            listSpan[index] = default!;
    }
}