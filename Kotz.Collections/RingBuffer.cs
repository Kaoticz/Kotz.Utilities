using Kotz.Collections.Debugger;
using Kotz.Collections.Extensions;
using System.Collections;
using System.Diagnostics;
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
[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
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
    /// Gets or sets the size of the internal data structure.
    /// </summary>
    public int Count
        => _internalList.Count;

    /// <summary>
    /// Determines whether this collection is read-only.
    /// </summary>
    bool ICollection<T>.IsReadOnly { get; }

    /// <summary>
    /// Defines the current index of the buffer for writing.
    /// </summary>
    public int CurrentIndex { get; private set; }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    public RingBuffer()
    {
        _internalList = [];
        FillWithDefault(_internalList);
    }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the buffer with.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="collection"/> is <see langword="null"/>.</exception>
    public RingBuffer(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        if (!collection.Any())
        {
            _internalList = new(0);
            return;
        }

        _internalList = new(collection);
        var count = _internalList.Count;

        FillWithDefault(_internalList);
        Resize(count);
    }

    /// <summary>
    /// Initializes a <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <param name="capacity">The size of the ring buffer.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="capacity"/> is greater than the buffer's capacity or less than 0.</exception>
    public RingBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity cannot be equal or lower than zero.");

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
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity cannot be equal or lower than zero.");

        _internalList = new(collection);

        FillWithDefault(_internalList);
        Resize(capacity);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a nullable type, <see langword="null"/> values may be enumerated.</remarks>
    /// <returns>An enumerator for the <see cref="RingBuffer{T}"/>.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => _internalList.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="RingBuffer{T}"/>.
    /// </summary>
    /// <remarks>If <typeparamref name="T"/> is a nullable type, <see langword="null"/> values may be enumerated.</remarks>
    /// <returns>An enumerator for the <see cref="RingBuffer{T}"/>.</returns>
    public IEnumerator<T> GetEnumerator()
        => _internalList.GetEnumerator();

    /// <summary>
    /// Creates a new <see cref="Span{T}"/> over this ring buffer.
    /// </summary>
    /// <remarks>
    /// Do not resize the <see cref="RingBuffer{T}"/> while the <see cref="Span{T}"/> is in use. <br />
    /// If <typeparamref name="T"/> is a nullable type, this span may contain <see langword="null"/> values.
    /// </remarks>
    /// <returns>The <see cref="Span{T}"/> representation of the ring buffer.</returns>
    public Span<T> AsSpan()
        => CollectionsMarshal.AsSpan(_internalList);

    /// <summary>
    /// Creates a new <see cref="ReadOnlySpan{T}"/> over this ring buffer.
    /// </summary>
    /// <remarks>
    /// Do not resize the <see cref="RingBuffer{T}"/> while the <see cref="ReadOnlySpan{T}"/> is in use. <br />
    /// If <typeparamref name="T"/> is a nullable type, this span may contain <see langword="null"/> values.
    /// </remarks>
    /// <returns>The <see cref="ReadOnlySpan{T}"/> representation of the ring buffer.</returns>
    public ReadOnlySpan<T> AsReadOnlySpan()
        => CollectionsMarshal.AsSpan(_internalList);

    /// <summary>
    /// Removes all items from the <see cref="RingBuffer{T}"/> by setting them to
    /// the default value of <typeparamref name="T"/>.
    /// </summary>
    public void Clear()
        => CollectionsMarshal.AsSpan(_internalList).Clear();

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
    /// <remarks>If <typeparamref name="T"/> is a reference type, the copied array may contain <see langword="null"/> values.</remarks>
    /// <exception cref="ArgumentException">
    /// Occurs when the number of elements in the source <see cref="RingBuffer{T}"/> is greater than the available space from <paramref name="arrayIndex"/>
    /// to the end of the destination <paramref name="array"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Occurs when <paramref name="arrayIndex"/> is greater than the length of the <paramref name="array"/>.</exception>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="arrayIndex"/> is greater than the buffer's capacity or less than 0.</exception>
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
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="newSize"/> is less than 0.</exception>
    /// <exception cref="OutOfMemoryException">Occurs when the system has no free memory available for allocation.</exception>
    public void Resize(int newSize)
    {
        if (newSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(newSize), newSize, "Ring buffer cannot be resized to zero or negative values.");

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
    /// Safely gets the first non-<see langword="null"/> element that meets the criteria of the specified <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Delegate that defines the conditions of the element to get.</param>
    /// <param name="item">The resulting element.</param>
    /// <returns><see langword="true"/> if the element was successfully fetched, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public bool TryGetValue(Func<T, bool> predicate, [MaybeNullWhen(false)] out T item)
    {
        var index = _internalList.IndexOfNonNull(predicate);
        item = (index is not -1) ? _internalList[index] : default;

        return index is not -1 && item is not null;
    }

    /// <summary>
    /// Fills the specified list with default <typeparamref name="T"/> values up to its capacity.
    /// </summary>
    /// <param name="list">The list to be filled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FillWithDefault(List<T> list)
    {
        lock (list)
        {
            for (var index = list.Count; index < list.Capacity; index++)
                list.Add(default!);
        }
    }
}