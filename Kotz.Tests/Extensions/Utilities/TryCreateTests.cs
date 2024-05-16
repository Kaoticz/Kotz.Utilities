using System.Text;

namespace Kotz.Tests.Extensions.Utilities;

public sealed class TryCreateTests
{
    [Fact]
    internal void TryCreateSuccessTest()
    {
        Assert.True(KotzUtilities.TryCreate(() => new StringBuilder(), out var actualResult, out var exception));
        Assert.IsType<StringBuilder>(actualResult);
        Assert.Null(exception);
    }

    [Fact]
    internal void TryCreateFailTest()
    {
        Assert.False(KotzUtilities.TryCreate<StringBuilder>(() => throw new InvalidOperationException(), out var actualResult, out var exception));
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Null(actualResult);
    }
}