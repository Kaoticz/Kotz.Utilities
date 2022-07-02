using Kotz.Extensions;
using System.Text;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class StringBuilderExtTest
{
    [Theory]
    [InlineData("double__space!", "__", "_", "double_space!")]
    [InlineData("triple___space!", "__", "_", "triple_space!")]
    [InlineData("exclamation!", "!", "", "exclamation")]
    [InlineData("exclamations!!!!!", "!", "", "exclamations")]
    [InlineData("aaaaaa", "a", "", "")]
    internal void ReplaceAllTest(string input, string toReplace, string replacement, string result)
        => Assert.Equal(result, new StringBuilder(input).ReplaceAll(toReplace, replacement).ToString());

    [Theory]    // count = string.Length - startIndex - amount of indices desired after startIndex
    [InlineData("hello hello hello", "l", "", "hello heo heo", 5, 16 - 5)]
    [InlineData("hello hello hello", "l", "", "hello heo hello", 5, 16 - 5 - 5)]
    internal void ReplaceAllWithIndexTest(string input, string toReplace, string replacement, string result, int startIndex, int count)
        => Assert.Equal(result, new StringBuilder(input).ReplaceAll(toReplace, replacement, startIndex, count).ToString());

    [Theory]
    [InlineData("double__space!", "_", "__")]
    [InlineData("triple___space!", "_", "__")]
    internal void ReplaceAllExceptionTest(string input, string toReplace, string replacement)
        => Assert.Throws<ArgumentException>(() => new StringBuilder(input).ReplaceAll(toReplace, replacement));

    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    internal void ToStringAndClear(string input)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.Equal(input, stringBuilder.ToStringAndClear());
        Assert.True(stringBuilder.Length is 0);
    }

    [Theory]
    [InlineData("hello", 2, 2)]
    internal void ToSubstringAndClear(string input, int startIndex, int length)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.Equal(input.Substring(startIndex, length), stringBuilder.ToStringAndClear(startIndex, length));
        Assert.True(stringBuilder.Length is 0);
    }
}
