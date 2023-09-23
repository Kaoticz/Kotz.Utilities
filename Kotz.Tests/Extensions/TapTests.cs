namespace Kotz.Tests.Extensions;

public sealed class TapTests
{
    [Theory]
    [InlineData(new int[] { })]
    [InlineData(new int[] { 0, 1, 2, 3 })]
    internal void TapTest(IReadOnlyList<int> collection)
    {
        var counter = 0;
        var copy = collection.ToArray();

        collection.Tap(x => Assert.Equal(collection[counter++], x));
        Assert.Equal(copy, collection);
    }
}