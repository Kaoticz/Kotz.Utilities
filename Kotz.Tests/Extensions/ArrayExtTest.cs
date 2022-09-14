using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class ArrayExtTest
{
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
}