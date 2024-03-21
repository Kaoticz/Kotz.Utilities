namespace Kotz.Tests.Extensions;

public sealed class OrderDescendingAmountTests
{
    [Theory]
    [InlineData(new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(new int[] { 0, 0, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 0, 0 })]
    [InlineData(new int[] { 1, 1, 1, 1, 0, 0 }, new int[] { 1, 1, 1, 1, 0, 0, })]
    [InlineData(new int[] { 3, 2, 2, 3, 1, 3 }, new int[] { 3, 3, 3, 2, 2, 1 })]
    internal void OrderDescendingAmountSortTest(int[] collection, int[] expected)
    {
        var result = collection
            .OrderDescendingAmount()
            .ToArray();

        for (var index = 0; index < expected.Length; index++)
            Assert.StrictEqual(expected[index], result[index]);
    }

    [Theory]
    [InlineData(new int[] { })]
    internal void OrderDescendingAmountEmptyTest(int[] collection)
    {
        var result = collection
            .OrderDescendingAmount()
            .ToArray();

        Assert.Empty(result);
    }

    [Fact]
    internal void OrderDescendingAmountFailTest()
        => Assert.Throws<ArgumentNullException>(() => LinqExt.OrderDescendingAmount<int>(null!).ToArray());
}