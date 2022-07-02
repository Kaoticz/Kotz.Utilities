using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class RandomElementTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    internal void RandomElementTest(int maxLength)
    {
        var sample = Enumerable.Range(0, maxLength);
        var element = sample.RandomElement();

        Assert.Contains(element, sample);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    internal void RandomElementOrDefaultTest(int maxLength)
    {
        var sample = Enumerable.Range(0, maxLength);
        var element = sample.RandomElementOrDefault();

        if (sample.Any())
            Assert.Contains(element, sample);
        else
            Assert.True(element is default(int));
    }

    [Fact]
    internal void RandomElementExceptionTests()
    {
        IEnumerable<int> sample = null!;

        Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Empty<int>().RandomElement());
        Assert.Throws<ArgumentNullException>(() => sample.RandomElement());
        Assert.Throws<ArgumentNullException>(() => sample.RandomElementOrDefault());
    }
}