namespace Kotz.Tests.Extensions;

public sealed class SplitTests
{
    [Theory]
    [InlineData(0, new int[] { 1, 1, 0, 2, 2, 2, 0, 3 }, new int[] { 1, 1 }, new int[] { 2, 2, 2 }, new int[] { 3 })]
    [InlineData(0, new int[] { 1, 0, 0, 0, 2, 2, 0, 3 }, new int[] { 1 }, new int[] { 2, 2 }, new int[] { 3 })]
    [InlineData(0, new int[] { 1, 1, 0, 2, 2, 2, 0, 0 }, new int[] { 1, 1 }, new int[] { 2, 2, 2 })]
    [InlineData(0, new int[] { 0, 1, 0, 2, 2, 2, 0, 0 }, new int[] { 1 }, new int[] { 2, 2, 2 })]
    [InlineData(0, new int[] { 1, 0, 2, 0, 3, 3 }, new int[] { 1 }, new int[] { 2 }, new int[] { 3, 3 })]
    [InlineData(0, new int[] { 0, 0, 0, 1, 0, 0, 0, 0 }, new int[] { 1 })]
    [InlineData(0, new int[] { 0, 0, 0, 0, 0, 0, 0, 3 }, new int[] { 3 })]
    [InlineData(0, new int[] { 1 }, new int[] { 1 })]
    [InlineData(0, new int[] { 0 })]
    [InlineData(0, new int[] { }, new int[] { })]
    [InlineData(-1, new int[] { 1, 1, 0, 2, 2, 2, 0, 3 }, new int[] { 1, 1, 0, 2, 2, 2, 0, 3 })]
    internal void SplitTest(int splitValue, IEnumerable<int> sample, params int[][] answer)
    {
        var result = sample
            .Split(splitValue)
            .Select(x => x.ToArray())
            .ToArray();

        Assert.Equal(answer.Length, result.Length);

        for (var collectionsIndex = 0; collectionsIndex < answer.Length; collectionsIndex++)
        {
            for (var subcollectionIndex = 0; subcollectionIndex < result[collectionsIndex].Length; subcollectionIndex++)
                Assert.Equal(answer[collectionsIndex][subcollectionIndex], result[collectionsIndex][subcollectionIndex]);
        }
    }

    [Theory]
    [InlineData(null)]
    internal void SplitNullTest(IEnumerable<int> sample)
        => Assert.Throws<ArgumentNullException>(() => sample.Split(0).ToArray());
}