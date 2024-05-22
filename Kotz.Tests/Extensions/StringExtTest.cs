using System.Globalization;

namespace Kotz.Tests.Extensions;

public sealed class StringExtTest
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
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("1234", "1234")]
    [InlineData("u1234", "U1234")]
    [InlineData("Hello", "Hello")]
    [InlineData("world", "World")]
    [InlineData("HelloWorld", "Helloworld")]
    [InlineData("hello world", "Hello World")]
    [InlineData("hello_world", "Hello_World")]
    [InlineData("   uppercase", "   Uppercase")]
    internal void ToTitleCaseTest(string? source, string result)
        => Assert.Equal(source.ToTitleCase(new CultureInfo("en-US")), result);

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("1234", "1234")]
    [InlineData("hello", "Hello")]
    [InlineData("Hello", "Hello")]
    [InlineData("hello there", "HelloThere")]
    [InlineData("Pascal_CASE", "PascalCase")]
    [InlineData("Pascal_CASe", "PascalCaSe")]
    [InlineData("LongChannelName", "LongChannelName")]
    [InlineData("Long Channel Name", "LongChannelName")]
    [InlineData("Double  space!", "DoubleSpace")]
    [InlineData("Triple   space!", "TripleSpace")]
    [InlineData("ALL CAPS", "AllCaps")]
    [InlineData("SOMECaps", "SomeCaps")]
    [InlineData("has_Underscore", "HasUnderscore")]
    [InlineData("has_ Underscore", "HasUnderscore")]
    [InlineData("has_ underscore", "HasUnderscore")]
    [InlineData("snake_case", "SnakeCase")]
    [InlineData("PascalCase", "PascalCase")]
    [InlineData("SCREAMINGCASE", "Screamingcase")]
    [InlineData("SCREAMING_SNAKE", "ScreamingSnake")]
    [InlineData("kebab-case", "KebabCase")]
    [InlineData("Pascal_Snake", "PascalSnake")]
    [InlineData("snake_case:colon", "SnakeCaseColon")]
    [InlineData("snake.dot", "SnakeDot")]
    [InlineData("snake@at", "SnakeAt")]
    [InlineData("snake#hash", "SnakeHash")]
    [InlineData("snake$dollar", "SnakeDollar")]
    [InlineData("snake%percentage", "SnakePercentage")]
    [InlineData("snake&ampersand", "SnakeAmpersand")]
    [InlineData("snake*asterisk", "SnakeAsterisk")]
    [InlineData("__trailingUnderscore_", "TrailingUnderscore")]
    [InlineData("_private_stuff", "PrivateStuff")]
    internal void ToPascalCaseTest(string input, string expected)
        => Assert.Equal(expected, input.ToPascalCase());

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("1234", "1234")]
    [InlineData("hello", "hello")]
    [InlineData("Hello", "hello")]
    [InlineData("hello there", "hello_there")]
    [InlineData("heLlo tHere", "he_llo_t_here")]
    [InlineData("heLLo tHeRE", "he_llo_t_he_re")]
    [InlineData("LongChannelName", "long_channel_name")]
    [InlineData("Long Channel Name", "long_channel_name")]
    [InlineData("Double  space!", "double_space")]
    [InlineData("Triple   space!", "triple_space")]
    [InlineData("ALL CAPS", "all_caps")]
    [InlineData("SOMECaps", "somecaps")]
    [InlineData("has_Underscore", "has_underscore")]
    [InlineData("has_ Underscore", "has_underscore")]
    [InlineData("has_ underscore", "has_underscore")]
    [InlineData("snake_case", "snake_case")]
    [InlineData("PascalCase", "pascal_case")]
    [InlineData("SCREAMINGCASE", "screamingcase")]
    [InlineData("SCREAMING_SNAKE", "screaming_snake")]
    [InlineData("kebab-case", "kebab_case")]
    [InlineData("Pascal_Snake", "pascal_snake")]
    [InlineData("snake_case:colon", "snake_case_colon")]
    [InlineData("snake.dot", "snake_dot")]
    [InlineData("snake@at", "snake_at")]
    [InlineData("snake#hash", "snake_hash")]
    [InlineData("snake$dollar", "snake_dollar")]
    [InlineData("snake%percentage", "snake_percentage")]
    [InlineData("snake&ampersand", "snake_ampersand")]
    [InlineData("snake*asterisk", "snake_asterisk")]
    [InlineData("__trailingUnderscore_", "trailing_underscore")]
    [InlineData("_private_stuff", "private_stuff")]
    internal void ToSnakeCaseTest(string source, string resultNoJoin)
        => Assert.Equal(resultNoJoin, source.ToSnakeCase());

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