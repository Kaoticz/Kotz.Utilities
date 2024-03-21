namespace Kotz.Tests.Extensions;

public sealed class OrderAmountTests
{
    [Theory]
    [InlineData(new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData(new int[] { 0, 0, 1, 1, 1, 1 }, new int[] { 0, 0, 1, 1, 1, 1 })]
    [InlineData(new int[] { 1, 1, 1, 1, 0, 0 }, new int[] { 0, 0, 1, 1, 1, 1 })]
    [InlineData(new int[] { 3, 2, 2, 3, 1, 3 }, new int[] { 1, 2, 2, 3, 3, 3 })]
    internal void OrderAmountSortTest(int[] collection, int[] expected)
    {
        var result = collection
            .OrderAmount()
            .ToArray();

        for (var index = 0; index < expected.Length; index++)
            Assert.StrictEqual(expected[index], result[index]);
    }

    [Theory]
    [InlineData(new int[] { })]
    internal void OrderAmountEmptyTest(int[] collection)
    {
        var result = collection
            .OrderAmount()
            .ToArray();

        Assert.Empty(result);
    }

    [Fact]
    internal void OrderAmountFailTest()
        => Assert.Throws<ArgumentNullException>(() => LinqExt.OrderAmount<int>(null!).ToArray());
}
