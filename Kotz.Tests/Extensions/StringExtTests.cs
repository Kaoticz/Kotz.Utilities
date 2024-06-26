namespace Kotz.Tests.Extensions;

public sealed class StringExtTestss
{
    [Theory]
    [InlineData(5, "hello", "hello")]
    [InlineData(5, "avocado", "avoca")]
    [InlineData(0, "banana", "")]
    [InlineData(0, "", "")]
    [InlineData(5, null, null)]
    internal void MaxLengthTest(int length, string source, string result)
        => Assert.Equal(result, source.MaxLength(length));

    [Theory]
    [InlineData(11, "banana cheesecake", "banana[...]", "[...]")]
    [InlineData(11, "banana chees", "banana[...]", "[...]")]
    [InlineData(11, "banana chee", "banana chee", "[...]")]
    [InlineData(6, "banana cheesecake", "b[...]", "[...]")]
    [InlineData(6, "banana", "banana", "[...]")]
    [InlineData(1, "banana", "b", null)]
    [InlineData(5, "", "", "[...]")]
    [InlineData(5, "a", "a", "[...]")]
    [InlineData(5, "a", "a", null)]
    [InlineData(5, null, null, null)]
    [InlineData(5, null, null, "")]
    internal void MaxLengthWithAppendTest(int maxLength, string source, string result, string append)
        => Assert.Equal(result, source.MaxLength(maxLength, append));

    [Theory]
    [InlineData(0, "banana", "[...]")]
    [InlineData(1, "banana", "[...]")]
    [InlineData(5, "banana", "[...]")]
    internal void MaxLengthWithAppendErrorTest(int maxLength, string source, string append)
        => Assert.Throws<ArgumentOutOfRangeException>(() => source.MaxLength(maxLength, append));

    [Theory]
    [InlineData("hello", 'l', 2)]
    [InlineData("hello", 'a', 0)]
    [InlineData("hello there", 'e', 3)]
    [InlineData("this has three spaces", ' ', 3)]
    internal void OccurrencesTest(string source, char target, int result)
        => Assert.Equal(result, source.Occurrences(target));

    [Theory]
    [InlineData(7, "hello", "banana", "avocado")]
    [InlineData(5, "hello", "", "test")]
    [InlineData(0, "", "", "")]
    internal void MaxElementLengthTest(int result, params string[] collection)
        => Assert.Equal(result, collection.MaxElementLength());

    [Theory]
    [InlineData("12345", "12345")]
    [InlineData("", "")]
    [InlineData("123abc45", "12345")]
    [InlineData("!1_2a3&4%5", "12345")]
    [InlineData("Nothing", "")]
    [InlineData("111e111", "111111")]
    internal void GetDigitsTest(string source, string result)
        => Assert.Equal(result, source.GetDigits());

    [Theory]
    [InlineData("hello", 'a', 0, -1)]
    [InlineData("hello", 'a', 3, -1)]
    [InlineData("hello", 'e', 0, 1)]
    [InlineData("hello", 'l', 0, 2)]
    [InlineData("hello", 'l', 1, 3)]
    [InlineData("hello", 'l', 2, -1)]
    [InlineData("hello", 'h', 0, 0)]
    [InlineData("hello", 'h', 1, -1)]
    [InlineData("hello hello", 'l', 3, 9)]
    [InlineData("hello hello", 'h', 0, 0)]
    [InlineData("hello hello", 'h', 1, 6)]
    internal void FirstOccurrenceOfTest(string source, char character, int match, int result)
        => Assert.Equal(result, source.FirstOccurrenceOf(character, match));

    [Theory]
    [InlineData("hello", 'a', 0, -1)]
    [InlineData("hello", 'a', 3, -1)]
    [InlineData("hello", 'e', 0, 1)]
    [InlineData("hello", 'l', 0, 3)]
    [InlineData("hello", 'l', 1, 2)]
    [InlineData("hello", 'l', 2, -1)]
    [InlineData("hello", 'h', 0, 0)]
    [InlineData("hello", 'h', 1, -1)]
    [InlineData("hello hello", 'l', 3, 2)]
    [InlineData("hello hello", 'h', 0, 6)]
    [InlineData("hello hello", 'h', 1, 0)]
    internal void LastOccurrenceOfTest(string source, char character, int match, int result)
        => Assert.Equal(result, source.LastOccurrenceOf(character, match));

    [Theory]
    [InlineData("!hello", "!hello", StringComparison.Ordinal, true)]
    [InlineData("!hellow", "!hello", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!hellow", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!hello there", StringComparison.Ordinal, true)]
    [InlineData("!hello there", "!hello", StringComparison.Ordinal, true)]
    [InlineData("!hello", "!heLLO", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!heLLo", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("!hello", "!hellow there", StringComparison.Ordinal, false)]
    internal void HasFirstWordOfTest(string source, string callee, StringComparison comparisonType, bool result)
        => Assert.Equal(result, source.HasFirstWordOf(callee, comparisonType));
}