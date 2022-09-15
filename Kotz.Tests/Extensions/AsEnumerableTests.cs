using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class AsEnumerableTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    internal void AsEnumerableSpanTest(int amount)
    {
        var sample = Enumerable.Range(0, amount).ToArray();
        var sampleSpan = sample.AsSpan();
        var spanEnumerable = sampleSpan.AsEnumerable();

        if (amount is 0)
            Assert.Empty(spanEnumerable);
        else
            Assert.Contains(spanEnumerable.ToArray(), x => sample.Contains(x));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    internal void AsEnumerableReadOnlySpanTest(int amount)
    {
        var sample = Enumerable.Range(0, amount).ToArray();
        var sampleSpan = sample.AsReadOnlySpan();
        var spanEnumerable = sampleSpan.AsEnumerable();

        if (amount is 0)
            Assert.Empty(spanEnumerable);
        else
            Assert.Contains(spanEnumerable.ToArray(), x => sample.Contains(x));
    }
}