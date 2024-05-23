using System.Text;

namespace Kotz.Tests.Extensions;

public sealed class StringBuilderExtTests
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
    [InlineData("hello", "world", "", false)]
    [InlineData("", "", "helloworld", false)]
    [InlineData("hello", "world", "blep", false)]
    [InlineData("hello", "world", "helloblep", false)]
    [InlineData("hello", "world", "helloworld", true)]
    [InlineData("helloworld", "hello", "helloworld", true)]
    [InlineData("helloworld", "hello", "worldhello", true)]
    internal void ContainsTest(string firstChunk, string secondChunk, string toSearch, bool expected)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(firstChunk);
        stringBuilder.Append(secondChunk);

        Assert.Equal(expected, stringBuilder.Contains(toSearch));
        stringBuilder.Clear();
    }

    [Theory]
    [InlineData("Hello World", "Hello World")]
    [InlineData("Hello World", "Hello World", "")]
    [InlineData("Hello World", "Hell Wrld", "o")]
    [InlineData("Hello World", "Hll Wrld", "o", "e")]
    [InlineData("Hello World", "Hello ", "World")]
    internal void RemoveTest(string input, string expected, params string[] toRemove)
    {
        var stringBuilder = new StringBuilder(input)
            .Remove(toRemove);

        Assert.Equal(expected, stringBuilder.ToString());
    }

    [Fact]
    internal void RemoveExceptionTest()
        => Assert.Throws<ArgumentNullException>(() => new StringBuilder().Remove(null!));

    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    internal void ToStringAndClearTest(string input)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.Equal(input, stringBuilder.ToStringAndClear());
        Assert.True(stringBuilder.Length is 0);
    }

    [Theory]
    [InlineData("hello", 2, 2)]
    internal void ToSubstringAndClearTest(string input, int startIndex, int length)
    {
        var stringBuilder = new StringBuilder(input);

        Assert.Equal(input.Substring(startIndex, length), stringBuilder.ToStringAndClear(startIndex, length));
        Assert.True(stringBuilder.Length is 0);
    }

    [Theory]
    [InlineData("", "", ' ')]
    [InlineData("abc", "abc", ' ')]
    [InlineData("   abc", "abc", ' ')]
    [InlineData("abc   ", "abc", ' ')]
    [InlineData("   abc   ", "abc", ' ')]
    [InlineData("_abc__", "abc", '_')]
    internal void TrimTest(string input, string expected, char trimChar)
    {
        var stringBuilder = new StringBuilder(input)
            .Trim(trimChar);

        Assert.Equal(expected, stringBuilder.ToStringAndClear());
    }

    [Theory]
    [InlineData("", "", ' ')]
    [InlineData("abc", "abc", ' ')]
    [InlineData("   abc", "   abc", ' ')]
    [InlineData("abc  ", "abc", ' ')]
    [InlineData("   abc  ", "   abc", ' ')]
    [InlineData("_abc__", "_abc", '_')]
    internal void TrimEndTest(string input, string expected, char trimChar)
    {
        var stringBuilder = new StringBuilder(input)
            .TrimEnd(trimChar);

        Assert.Equal(expected, stringBuilder.ToStringAndClear());
    }

    [Theory]
    [InlineData("", "", ' ')]
    [InlineData("abc", "abc", ' ')]
    [InlineData("   abc", "abc", ' ')]
    [InlineData("abc   ", "abc   ", ' ')]
    [InlineData("   abc   ", "abc   ", ' ')]
    [InlineData("_abc__", "abc__", '_')]
    internal void TrimStartTest(string input, string expected, char trimChar)
    {
        var stringBuilder = new StringBuilder(input)
            .TrimStart(trimChar);

        Assert.Equal(expected, stringBuilder.ToStringAndClear());
    }
}