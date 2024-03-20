namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [InlineData(default(int), new int[] { })]
    [InlineData(-1, new int[] { -1, 1, 2, 3, 4, 5 })]
    [InlineData(-1, new int[] { 5, 1, 2, 3, 4, -1 })]
    [InlineData(-1, new int[] { 1, -1, 2, -1, 4, 5 })]
    internal void MinOrDefaultTests(int expected, IReadOnlyList<int> collection)
        => Assert.Equal(expected, collection.MinOrDefault());

    [Theory]
    [InlineData(default(int), new int[] { })]
    [InlineData(-1, new int[] { -1, 1, 2, 3, 4, 5 })]
    [InlineData(-1, new int[] { 5, 1, 2, 3, 4, -1 })]
    [InlineData(-1, new int[] { 1, -1, 2, -1, 4, 5 })]
    internal void MinByOrDefaultTests(int expected, IReadOnlyList<int> collection)
        => Assert.Equal(expected, collection.MinByOrDefault(x => x));
}