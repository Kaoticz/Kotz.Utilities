using Kotz.Collections;
using Kotz.Extensions;
using System.Runtime.CompilerServices;
using Xunit;

namespace Kotz.Tests.Collections;

public sealed class RingBufferTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(10)]
    [InlineData(null, 1, 2, 3, 4, 5)]
    [InlineData(10, 1, 2, 3, 4, 5)]
    [InlineData(3, 1, 2, 3, 4, 5)]
    internal void InitializationTest(int? capacity, params int?[] collection)
    {
        var ringBuffer = CreateRingBuffer(capacity, collection);

        Assert.True(ringBuffer.CurrentIndex is 0);
        Assert.True(ringBuffer.Count == ringBuffer.Count(x => x != default));

        if (collection == default)
            Assert.True(ringBuffer.All(x => x == default), "Buffer was not filled with default values.");

        if (capacity != default)
            Assert.True(capacity == ringBuffer.Length);

        if (capacity != default && collection == default)
            Assert.Equal(capacity, ringBuffer.Length);

        if (capacity == default && collection != default)
            Assert.Equal(collection.Length, ringBuffer.Length);

        if (capacity != default && collection != default)
        {
            if (capacity > collection.Length)
                Assert.True(ringBuffer.Skip(capacity.Value - collection.Length).All(x => x == default), "Buffer was not filled with default values.");
            else
                Assert.Contains(collection.Take(capacity!.Value), x => ringBuffer.Contains(x));
        }
    }

    [Theory]
    [InlineData(2, true, 1, 2, 3, 4, 5)]
    [InlineData(5, true, 1, 2, 3, 4, 5)]
    [InlineData(1, true, 1, 2, 3, 4, 5)]
    [InlineData(0, false, 1, 2, 3, 4, 5)]
    [InlineData(15, false, 1, 2, 3, 4, 5)]
    [InlineData(null, false, 1, 2, 3, 4, 5)]
    [InlineData(null, false, 1, 2, 3, null, 5)]
    internal void TryGetValueTest(int? value, bool success, params int?[] collection)
    {
        var ringBuffer = CreateRingBuffer(null, collection);
        var isSuccess = ringBuffer.TryGetValue(x => x == value, out var result);

        Assert.Equal(success, isSuccess);

        if (isSuccess)
        {
            var index = ringBuffer.IndexOf(result);
            Assert.StrictEqual(ringBuffer[index], result);
        }
    }

    [Theory]
    [InlineData(5, 1, 2, 3, 4, 5)]
    [InlineData(10, 1, 2, 3, 4, 5)]
    [InlineData(3, 1, 2, 3, 4, 5)]
    internal void AddTest(int capacity, params int?[] collection)
    {
        var ringBuffer = CreateRingBuffer<int?>(capacity);

        foreach (var number in collection)
            ringBuffer.Add(number);

        Assert.Equal(capacity, ringBuffer.Length);

        if (capacity >= collection.Length)
        {
            Assert.Contains(collection, x => ringBuffer.Contains(x));
            Assert.Equal(ringBuffer.Count, collection.Length);
            Assert.Equal(ringBuffer.CurrentIndex, collection.Length);
        }
        else
        {
            Assert.Contains(collection.TakeLast(capacity), x => ringBuffer.Contains(x));
            Assert.NotEqual(ringBuffer.Count, collection.Length);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1, 2)]
    [InlineData(1, 2, 3, 4, 5)]
    [InlineData(1, 2, 3, 4, 5, 6, 7, 8)]
    internal void RemoveTest(params int?[] collection)
    {
        var sample = Enumerable.Range(1, 10).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        foreach (var number in collection)
            ringBuffer.Remove(number!.Value);

        Assert.Equal(ringBuffer.Length, sample.Length);
        Assert.Equal(ringBuffer.Count, sample.Length - collection.Length);
        Assert.DoesNotContain(ringBuffer, x => collection.Contains(x));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1, 2, 3, 4, 5)]
    [InlineData(4, 1, 2, 3, 4, 5, 6, 7, 8)]
    internal void RemoveEvenTest(int result, params int?[] collection)
    {
        var ringBuffer = CreateRingBuffer(null, collection);

        Assert.Equal(result, ringBuffer.Remove(x => x % 2 is 0));
        Assert.True(ringBuffer.All(x => x % 2 is not 0));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    internal void ResizeTest(int newSize)
    {
        var sample = Enumerable.Range(1, 10).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        if (newSize <= 0)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ringBuffer.Resize(newSize));
            return;
        }

        ringBuffer.Resize(newSize);
        Assert.Equal(ringBuffer.Length, newSize);

        if (sample.Length > newSize)
            Assert.DoesNotContain(ringBuffer, x => ringBuffer.Contains(default));
        else
            Assert.Equal(newSize - sample.Length, ringBuffer.AsReadOnlySpan().AsEnumerable().Count(x => x == default));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    internal void RemoveAtTest(int index)
    {
        var sample = Enumerable.Range(1, 10).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        if (index < 0 || index > sample.Length)
            Assert.Throws<ArgumentOutOfRangeException>(() => ringBuffer.RemoveAt(index));
        else if (index < ringBuffer.Length)
        {
            ringBuffer.RemoveAt(index);
            Assert.True(ringBuffer[index] == default);
            Assert.Equal(ringBuffer.Intersect(sample).Count(), sample.Length - 1);
        }

        Assert.Equal(ringBuffer.Length, sample.Length);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    internal void InsertTest(int index)
    {
        var toInsert = 999;
        var sample = Enumerable.Range(1, 10).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        if (index < 0 || index > ringBuffer.Length)
            Assert.Throws<ArgumentOutOfRangeException>(() => ringBuffer.Insert(index, toInsert));
        else if (index < ringBuffer.Length)
        {
            ringBuffer.Insert(index, toInsert);
            Assert.Equal(ringBuffer[index], toInsert);
            Assert.Equal(ringBuffer.Intersect(sample).Count(), sample.Length - 1);
        }

        Assert.Equal(ringBuffer.Length, sample.Length);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    internal void IndexOfTest(int index)
    {
        var sample = Enumerable.Range(0, 9).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        if (index < 0 || index > ringBuffer.Length)
            Assert.Equal(-1, ringBuffer.IndexOf(index));
        else
            Assert.Equal(ringBuffer[index], ringBuffer.IndexOf(index));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    internal void ClearTest(int amount)
    {
        var sample = Enumerable.Range(0, amount).ToArray();
        var ringBuffer = CreateRingBuffer(null, sample);

        ringBuffer.Clear();

        Assert.Equal(amount, ringBuffer.Length);
        Assert.True(ringBuffer.All(x => x == default));
    }

    [Theory]
    [InlineData(1, 5, 2, 8, 9, 10, 10, 3, 6)]
    [InlineData(6, 3, 8, 1, 2, 5, 10, 5, 4)]
    [InlineData(9, 10, 5, 4, 3, 1, 2, 7, 1)]
    [InlineData(10, 9, 8, 7, 6, 5, 4, 3, 2)]
    [InlineData(9, 9, 9, 9, 8, 8, 8, 8, 8)]
    internal void ResizeMultiThreadTest(params int[] sizes)
    {
        var sample = CreateRingBuffer<int>(default);
        var actions = sizes
            .Select(x => (Action)(() => sample.Resize(x))) // Need to cast explicitly, compiler regression
            .ToArray();

        // This throws if a race condition occurs
        Parallel.Invoke(actions);

        Assert.True(sample.Length > 0);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(0)]
    internal void AsSpanTest(int amount)
    {
        var ringBuffer = new RingBuffer<int>(Enumerable.Range(0, amount));
        var span = ringBuffer.AsSpan();

        for (var index = 0; index < ringBuffer.Count; index++)
            Assert.Equal(ringBuffer[index], span[index]);

        Assert.Equal(ringBuffer.Length, span.Length);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(0)]
    internal void AsReadOnlySpanTest(int amount)
    {
        var ringBuffer = new RingBuffer<int>(Enumerable.Range(0, amount));
        var span = ringBuffer.AsReadOnlySpan();

        for (var index = 0; index < ringBuffer.Count; index++)
            Assert.Equal(ringBuffer[index], span[index]);

        Assert.Equal(ringBuffer.Length, span.Length);
    }

    /// <summary>
    /// Generates a new ring buffer.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="capacity">The final capacity of the ring buffer.</param>
    /// <param name="collection">The collection to add to the buffer, up to capacity.</param>
    /// <returns>A <see cref="RingBuffer{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static RingBuffer<T> CreateRingBuffer<T>(int? capacity, IEnumerable<T>? collection = default)
    {
        return (capacity == default && collection == default)
            ? new RingBuffer<T>()
            : (capacity != default && collection != default)
                ? new RingBuffer<T>(collection, capacity!.Value)
                : (capacity == default)
                    ? new RingBuffer<T>(collection ?? Enumerable.Empty<T>())
                    : new RingBuffer<T>(capacity!.Value);
    }
}