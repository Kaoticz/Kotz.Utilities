using Kotz.Extensions;
using System.Text;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class StringBuilderExtTest
{
    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    internal void ToStringAndClear(string input)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.True(input.Equals(stringBuilder.ToStringAndClear(), StringComparison.Ordinal));
        Assert.True(stringBuilder.Length is 0);
    }

    [Theory]
    [InlineData("hello", 2, 2)]
    internal void ToSubstringAndClear(string input, int startIndex, int length)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.True(input.Substring(startIndex, length).Equals(stringBuilder.ToStringAndClear(startIndex, length), StringComparison.Ordinal));
        Assert.True(stringBuilder.Length is 0);
    }
}
