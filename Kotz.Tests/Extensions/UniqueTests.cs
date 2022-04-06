using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Fact]
    internal void UniqueNullTest()
        => Assert.Throws<ArgumentNullException>(() => Enumerable.Empty<int>().Unique(null!));

    [Theory]
    [InlineData(10)]
    [InlineData(0, 1, 2, 3, 4, 5)]
    [InlineData(0)]
    internal void UniqueEmptyTest(int sourceSize, params int[] toExclude)
    {
        var source = sourceSize is 0
            ? Enumerable.Empty<int>()
            : Enumerable.Range(0, sourceSize);

        var result = source.Unique(toExclude);

        if (sourceSize is 0 && toExclude.Length is 0)
            Assert.Empty(result);
        else
            Assert.Equal(Math.Max(source.Count(), toExclude.Length), result.Count());
    }

    [Theory]
    [InlineData(10, 1, 2, 3, 4)]
    [InlineData(10, -1, -2, -3, -4)]
    [InlineData(10, 1, -2, 3, -4)]
    [InlineData(20, 20, 0, 21, 5, 6, 9, 2)]
    internal void UniqueTest(int sourceSize, params int[] toExclude)
    {
        var source = Enumerable.Range(0, sourceSize);
        var result = source.Unique(toExclude);

        foreach (var element in result)
            Assert.True(!source.Contains(element) || !toExclude.Contains(element));
    }

    [Theory]
    [InlineData(10, 1, 2, 3, 4)]
    [InlineData(10, -1, -2, -3, -4)]
    [InlineData(10, 1, -2, 3, -4)]
    [InlineData(20, 20, 0, 21, 5, 6, 9, 2)]
    internal void UniqueMultipleTest(int sourceSize, params int[] toExclude)
    {
        var source = Enumerable.Range(0, sourceSize);
        var result = source.Unique(toExclude.Chunk(1).ToArray());

        foreach (var element in result)
            Assert.True(!source.Contains(element) || !toExclude.Contains(element));
    }
}