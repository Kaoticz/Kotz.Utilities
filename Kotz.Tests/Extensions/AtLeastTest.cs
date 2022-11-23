using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class AtLeastTest
{
    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 1)]
    [MemberData(nameof(GetSampleArray), 10, 3)]
    [MemberData(nameof(GetSampleArray), 10, 10)]
    internal void AtLeastTrueTest(int[] sample, int amount)
        => Assert.True(sample.AtLeast(amount));

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 2)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    [MemberData(nameof(GetSampleArray), 10, 20)]
    internal void AtLeastFalseTest(int[] sample, int amount)
        => Assert.False(sample.AtLeast(amount));

    [Theory]
    [MemberData(nameof(GetSampleArray), 10, 0)]
    [MemberData(nameof(GetSampleArray), 10, -1)]
    internal void AtLeastFailTest(int[] sample, int amount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => sample.AtLeast(amount));
        Assert.Throws<ArgumentOutOfRangeException>(() => sample.AtLeast(amount, x => x % 2 is 0));
    }

    [Fact]
    internal void AtLeastNullFailTest()
    {
        int[]? sample = null!;

        Assert.Throws<ArgumentNullException>(() => sample.AtLeast(1));
        Assert.Throws<ArgumentNullException>(() => sample.AtLeast(1, x => x % 2 is 0));
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 1)]
    [MemberData(nameof(GetSampleArray), 10, 3)]
    [MemberData(nameof(GetSampleArray), 10, 5)]
    internal void AtLeastPredicateTrueTest(int[] sample, int amount)
        => Assert.True(sample.AtLeast(amount, x => x % 2 is 0));

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 2)]
    [MemberData(nameof(GetSampleArray), 10, 6)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    internal void AtLeastPredicateFalseTest(int[] sample, int amount)
        => Assert.False(sample.AtLeast(amount, x => x % 2 is 0));

    /// <summary>
    /// Gets an int array and the desired amount for a theory test.
    /// </summary>
    /// <param name="arraySize">The size of the array.</param>
    /// <param name="amount">The desired amount.</param>
    /// <returns>object[] { int[], int }</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="arraySize"/> is less than 0.</exception>
    public static IEnumerable<object[]> GetSampleArray(int arraySize, int amount)
        => ArrayExtTest.GetSampleArray(arraySize, amount);
}