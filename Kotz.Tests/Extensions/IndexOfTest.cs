namespace Kotz.Tests.Extensions;

public sealed class IndexOfTest
{
    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 0)]
    [MemberData(nameof(GetSampleArray), 10, 0)]
    [MemberData(nameof(GetSampleArray), 10, 5)]
    [MemberData(nameof(GetSampleArray), 10, 9)]
    [MemberData(nameof(GetSampleArray), 50, 49)]
    internal void IndexOfTrueTest(int[] sample, int index)
    {
        var result = sample.IndexOf(x => x == index);
        Assert.Equal(index, result);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 0, 0)]
    [MemberData(nameof(GetSampleArray), 0, 1)]
    [MemberData(nameof(GetSampleArray), 10, -1)]
    [MemberData(nameof(GetSampleArray), 10, -10)]
    [MemberData(nameof(GetSampleArray), 10, 10)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    internal void IndexOfFalseTest(int[] sample, int index)
    {
        var result = sample.IndexOf(x => x == index);
        Assert.Equal(-1, result);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 1, 0)]
    [MemberData(nameof(GetSampleArray), 10, 0)]
    [MemberData(nameof(GetSampleArray), 10, 5)]
    [MemberData(nameof(GetSampleArray), 10, 9)]
    [MemberData(nameof(GetSampleArray), 50, 49)]
    internal void LastIndexOfTrueTest(int[] sample, int index)
    {
        var result = sample.LastIndexOf(x => x == index);
        Assert.Equal(index, result);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 0, 0)]
    [MemberData(nameof(GetSampleArray), 0, 1)]
    [MemberData(nameof(GetSampleArray), 10, -1)]
    [MemberData(nameof(GetSampleArray), 10, -10)]
    [MemberData(nameof(GetSampleArray), 10, 10)]
    [MemberData(nameof(GetSampleArray), 10, 11)]
    internal void LastIndexOfFalseTest(int[] sample, int index)
    {
        var result = sample.LastIndexOf(x => x == index);
        Assert.Equal(-1, result);
    }

    /// <summary>
    /// Wrapper method to make an external static method work with xUnit.
    /// </summary>
    public static IEnumerable<object[]> GetSampleArray(int arraySize, int index)
        => ArrayExtTest.GetSampleArray(arraySize, index);
}