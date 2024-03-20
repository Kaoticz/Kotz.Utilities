namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [InlineData(default(int), new int[] { })]
    [InlineData(5, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(5, new int[] { 5, 1, 2, 3, 4, 0 })]
    [InlineData(5, new int[] { 0, 1, 5, 3, 5, 4 })]
    internal void MaxOrDefaultTests(int expected, IReadOnlyList<int> collection)
        => Assert.Equal(expected, collection.MaxOrDefault());

    [Theory]
    [InlineData(default(int), new int[] { })]
    [InlineData(5, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(5, new int[] { 5, 1, 2, 3, 4, 0 })]
    [InlineData(5, new int[] { 0, 1, 5, 3, 5, 4 })]
    internal void MaxByOrDefaultTests(int expected, IReadOnlyList<int> collection)
        => Assert.Equal(expected, collection.MaxByOrDefault(x => x));
}