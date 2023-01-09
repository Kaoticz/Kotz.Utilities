namespace Kotz.Tests.Extensions;

public sealed class RotateTests
{
    [Theory]
    [MemberData(nameof(GetSampleArray), 10, 1, 0)]
    [MemberData(nameof(GetSampleArray), 10, 9, 0)]
    [MemberData(nameof(GetSampleArray), 10, 4, 2)]
    [MemberData(nameof(GetSampleArray), 10, 0, 1)]
    [MemberData(nameof(GetSampleArray), 10, 0, 5)]
    [MemberData(nameof(GetSampleArray), 10, 0, 6)]
    [MemberData(nameof(GetSampleArray), 10, 1, 4)]
    [MemberData(nameof(GetSampleArray), 10, 8, 1)]
    [MemberData(nameof(GetSampleArray), 10, 4, 15)]
    [MemberData(nameof(GetSampleArray), 10, 4, 17)]
    [MemberData(nameof(GetSampleArray), 10, 5, 5)]
    [MemberData(nameof(GetSampleArray), 10, 5, 6)]
    [MemberData(nameof(GetSampleArray), 13, 4, 2)]
    [MemberData(nameof(GetSampleArray), 13, 0, 1)]
    [MemberData(nameof(GetSampleArray), 13, 0, 5)]
    [MemberData(nameof(GetSampleArray), 13, 0, 6)]
    [MemberData(nameof(GetSampleArray), 13, 1, 4)]
    [MemberData(nameof(GetSampleArray), 13, 8, 1)]
    [MemberData(nameof(GetSampleArray), 13, 4, 17)]
    [MemberData(nameof(GetSampleArray), 13, 6, 8)]
    internal void RotateTest(int[] sample, int startIndex, int amount)
    {
        var normalizedAmount = amount % sample.Length;

        if (startIndex + amount > sample.Length - startIndex)
            normalizedAmount %= startIndex;

        var startSlice = (normalizedAmount is 0) ? Array.Empty<int>() : sample[0..startIndex];
        var middleSlice = (normalizedAmount is 0) ? Array.Empty<int>() : sample[Math.Min(sample.Length - 1, startIndex + normalizedAmount)..];
        var endSlice = (normalizedAmount is 0) ? Array.Empty<int>() : sample[startIndex..Math.Min(sample.Length - 1, startIndex + normalizedAmount)];
        var sampleSpan = sample.AsSpan();

        // Mutate the collection
        sampleSpan.Rotate(startIndex, amount);

        // Check slices
        CheckSlice(sample, startSlice, 0);
        CheckSlice(sample, middleSlice, startIndex);
        CheckSlice(sample, endSlice, sample.Length - normalizedAmount);
    }

    [Theory]
    [MemberData(nameof(GetSampleArray), 10, -1, 1)]
    [MemberData(nameof(GetSampleArray), 10, 1, -1)]
    [MemberData(nameof(GetSampleArray), 10, 10, 1)]
    [MemberData(nameof(GetSampleArray), 10, 11, 1)]
    internal void RotateFailTest(int[] sample, int startIndex, int amount)
        => Assert.ThrowsAny<ArgumentOutOfRangeException>(() => sample.AsSpan().Rotate(startIndex, amount));

    /// <summary>
    /// Checks if <paramref name="slice"/> is present at <paramref name="collection"/> starting from
    /// the <paramref name="startIndex"/> position.
    /// </summary>
    /// <param name="collection">The collection to be checked.</param>
    /// <param name="slice">The slice to check.</param>
    /// <param name="startIndex">Index where the <paramref name="slice"/> starts in the <paramref name="collection"/>.</param>
    /// <exception cref="EqualException">Occurs when <paramref name="slice"/> is not <paramref name="collection"/> at <paramref name="startIndex"/>.</exception>
    private static void CheckSlice(ReadOnlySpan<int> collection, ReadOnlySpan<int> slice, int startIndex)
    {
        foreach (var element in slice)
            Assert.Equal(element, collection[startIndex++]);
    }

    /// <summary>
    /// Wrapper method to make an external static method work with xUnit.
    /// </summary>
    public static IEnumerable<object[]> GetSampleArray(int arraySize, int index, int amount)
    {
        var resultArray = ArrayExtTest.GetSampleArray(arraySize, index).First();
        yield return new object[] { resultArray[0], index, amount };
    }
}