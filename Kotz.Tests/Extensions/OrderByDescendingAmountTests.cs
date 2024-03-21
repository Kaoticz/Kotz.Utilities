namespace Kotz.Tests.Extensions;

public sealed class OrderByDescendingAmountTestss
{
    [Theory]
    [InlineData(new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(new int[] { 0, 0, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 0, 0 })]
    [InlineData(new int[] { 1, 1, 1, 1, 0, 0 }, new int[] { 1, 1, 1, 1, 0, 0, })]
    [InlineData(new int[] { 3, 2, 2, 3, 1, 3 }, new int[] { 3, 3, 3, 2, 2, 1 })]
    internal void OrderByDescendingAmountSortTest(int[] collection, int[] expected)
    {
        var result = collection
            .OrderByDescendingAmount(x => x)
            .ToArray();

        for (var index = 0; index < expected.Length; index++)
            Assert.StrictEqual(expected[index], result[index]);
    }

    [Theory]
    [InlineData(new int[] { })]
    internal void OrderByDescendingAmountEmptyTest(int[] collection)
    {
        var result = collection
            .OrderByDescendingAmount(x => x)
            .ToArray();

        Assert.Empty(result);
    }

    [Fact]
    internal void OrderByDescendingAmountFailTest()
        => Assert.Throws<ArgumentNullException>(() => LinqExt.OrderByDescendingAmount(null!, (int x) => x).ToArray());
}