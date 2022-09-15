using Kotz.Extensions;
using Xunit;
using Xunit.Sdk;

namespace Kotz.Tests.Extensions;

public sealed class ArrayExtTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(10)]
    internal void AsReadOnlySpanTest(int amount)
    {
        var sample = Enumerable.Range(0, amount).ToArray();
        var sampleSpan = sample.AsReadOnlySpan();

        IterationCheck(sample, sampleSpan);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(10, 4, 5)]
    [InlineData(10, 5, 5)]
    [InlineData(10, 9, 1)]
    internal void AsReadOnlySpanSliceTest(int amount, int startIndex, int length)
    {
        var sample = Enumerable.Range(0, amount).ToArray();
        var sampleSpan = sample.AsReadOnlySpan(startIndex, length);

        IterationCheck(sample.AsSpan().Slice(startIndex, length), sampleSpan);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(0, 11)]
    [InlineData(10, 10)]
    internal void AsReadOnlySpanFailTest(int startIndex, int length)
    {
        var sample = Enumerable.Range(0, 10).ToArray();
        Assert.Throws<ArgumentOutOfRangeException>(() => sample.AsReadOnlySpan(startIndex, length));
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 0)]
    [MemberData(nameof(GetSampleArray), 10, 0)]
    [MemberData(nameof(GetSampleArray), 10, 5)]
    [MemberData(nameof(GetSampleArray), 10, 9)]
    [MemberData(nameof(GetSampleArray), 50, 49)]
    internal void TryGetValueTrueTest(int[] sample, int index)
    {
        Assert.True(sample.TryGetValue(index, out var result));
        Assert.Equal(sample[index], result);

        // Test all elements
        for (var currentIndex = 0; currentIndex < sample.Length; currentIndex++)
        {
            Assert.True(sample.TryGetValue(currentIndex, out var currentElement));
            Assert.Equal(sample[currentIndex], currentElement);
        }
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 0, 0)]
    [MemberData(nameof(GetSampleArray), 0, 1)]
    [MemberData(nameof(GetSampleArray), 10, -1)]
    [MemberData(nameof(GetSampleArray), 10, -10)]
    [MemberData(nameof(GetSampleArray), 10, 10)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    internal void TryGetValueFalseTest(int[] sample, int index)
    {
        Assert.False(sample.TryGetValue(index, out var result));
        Assert.Equal(default, result);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 0)]
    [MemberData(nameof(GetSampleArray), 10, 0)]
    [MemberData(nameof(GetSampleArray), 10, 5)]
    [MemberData(nameof(GetSampleArray), 10, 9)]
    [MemberData(nameof(GetSampleArray), 50, 49)]
    internal void TryGetValuePredicateTrueTest(int[] sample, int index)
    {
        Assert.True(sample.TryGetValue(x => x == index, out var result));
        Assert.Equal(index, result);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 0, 0)]
    [MemberData(nameof(GetSampleArray), 0, 1)]
    [MemberData(nameof(GetSampleArray), 10, -1)]
    [MemberData(nameof(GetSampleArray), 10, -10)]
    [MemberData(nameof(GetSampleArray), 10, 10)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    internal void TryGetValuePredicateFalseTest(int[] sample, int index)
    {
        Assert.False(sample.TryGetValue(x => x == index, out var result));
        Assert.Equal(default, result);
    }

    /// <summary>
    /// Gets an int array and the desired index for a theory test.
    /// </summary>
    /// <param name="arraySize">The size of the array.</param>
    /// <param name="index">The desired index.</param>
    /// <returns>object[] { int[], int }</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="arraySize"/> is less than 0.</exception>
    public static IEnumerable<object[]> GetSampleArray(int arraySize, int index)
    {
        if (arraySize < 0)
            throw new ArgumentOutOfRangeException(nameof(arraySize), arraySize, "Array size cannot be less than 0.");
        else if (arraySize is 0)
            yield return new object[] { Array.Empty<int>(), index };
        else
        {
            var sample = Enumerable
                .Range(0, arraySize)
                .ToArray();

            yield return new object[] { sample, index };
        }
    }

    /// <summary>
    /// Iterates the provided collections and checks if they contain the exact same elements
    /// in the same order.
    /// </summary>
    /// <param name="reference">The reference collection.</param>
    /// <param name="sample">The sample collection.</param>
    /// <typeparam name="T">The type of data in the collections.</typeparam>
    /// <exception cref="EqualException" />
    private static void IterationCheck<T>(ReadOnlySpan<T> reference, ReadOnlySpan<T> sample)
    {
        Assert.Equal(reference.Length, sample.Length);

        for (var index = 0; index < reference.Length; index++)
            Assert.Equal(reference[index], sample[index]);
    }
}