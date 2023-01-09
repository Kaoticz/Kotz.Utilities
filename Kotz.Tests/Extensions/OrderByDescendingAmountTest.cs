namespace Kotz.Tests.Extensions;

public sealed class OrderByDescendingAmountTest
{
    [Theory]
    [InlineData(new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(new int[] { 0, 0, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 0, 0 })]
    [InlineData(new int[] { 1, 1, 1, 1, 0, 0 }, new int[] { 1, 1, 1, 1, 0, 0, })]
    [InlineData(new int[] { 3, 2, 2, 3, 1, 3 }, new int[] { 3, 3, 3, 2, 2, 1 })]
    internal void OrderByAmountSortTest(int[] collection, int[] expected)
    {
        var result = collection
            .OrderByDescendingAmount()
            .ToArray();

        for (var index = 0; index < expected.Length; index++)
            Assert.StrictEqual(expected[index], result[index]);
    }

    [Theory]
    [InlineData(new int[] { })]
    internal void OrderByAmountEmptyTest(int[] collection)
    {
        var result = collection
            .OrderByDescendingAmount()
            .ToArray();

        Assert.Empty(result);
    }

    [Fact]
    internal void OrderByAmountFailTest()
        => Assert.Throws<ArgumentNullException>(() => LinqExt.OrderByDescendingAmount<int>(null!).ToArray());
}