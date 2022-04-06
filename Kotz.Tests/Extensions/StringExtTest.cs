using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class StringExtTest
{
    [Theory]
    [InlineData("hello", "hello", StringComparison.Ordinal, true)]
    [InlineData("hello", "hel", StringComparison.Ordinal, true)]
    [InlineData("", "", StringComparison.Ordinal, true)]
    [InlineData("hello", "HEllo", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("hello", "HEl", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("hello", "hEl", StringComparison.Ordinal, true)]
    [InlineData("hello", "h", StringComparison.Ordinal, true)]
    [InlineData("hello", null, StringComparison.Ordinal, false)]
    [InlineData("hello", "HEllo", StringComparison.Ordinal, false)]
    internal void EqualsOrStartsWithTest(string caller, string calle, StringComparison comparisonType, bool result)
        => Assert.Equal(result, caller.EqualsOrStartsWith(calle, comparisonType));

    [Theory]
    [InlineData("test.yaml", "test")]
    [InlineData("test", "test")]
    [InlineData("test.a", "test")]
    [InlineData("test.not.yaml", "test.not")]
    [InlineData("", "")]
    internal void RemoveExtensionTest(string caller, string result)
        => Assert.Equal(result, caller.RemoveExtension());

    [Theory]
    [InlineData(5, "hello", "hello")]
    [InlineData(5, "avocado", "avoca")]
    [InlineData(0, "banana", "")]
    [InlineData(5, null, null)]
    internal void MaxLengthTest(int length, string caller, string result)
        => Assert.Equal(result, caller.MaxLength(length));

    [Theory]
    [InlineData(11, "banana cheesecake", "banana[...]", "[...]")]
    [InlineData(11, "banana chee", "banana chee", "[...]")]
    [InlineData(11, "banana chees", "banana[...]", "[...]")]
    [InlineData(0, "banana", "", "[...]")]
    [InlineData(5, "", "", "[...]")]
    [InlineData(5, "a", "a", "[...]")]
    [InlineData(5, "a", "a", null)]
    internal void MaxLengthWithAppendTest(int length, string caller, string result, string append)
        => Assert.Equal(result, caller.MaxLength(length, append));

    [Theory]
    [InlineData(5, "", "     ")]
    [InlineData(5, "hello", " hello")]
    [InlineData(6, "hello", " hello")]
    [InlineData(7, "hello", " hello ")]
    [InlineData(10, "hello", " hello    ")]
    internal void HardPadTest(int targetLength, string caller, string result)
        => Assert.Equal(result, caller.HardPad(targetLength));

    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData(" hello", " hello")]
    [InlineData("hello there", "Hello there")]
    [InlineData("a", "A")]
    [InlineData("", "")]
    internal void CapitalizeTest(string caller, string result)
        => Assert.Equal(result, caller.Capitalize());

    [Theory]
    [InlineData("hello", "hello")]
    [InlineData("hello there", "hello-there")]
    [InlineData("HeLlo THere", "hello-there")]
    [InlineData("Long Channel Name", "long-channel-name")]
    internal void ToTextChannelNameTest(string caller, string result)
        => Assert.Equal(result, caller.ToTextChannelName());

    [Theory]
    [InlineData("hello", "hello")]
    [InlineData("hello there", "hello there")]
    [InlineData("heLlo tHere", "he_llo t_here")]
    [InlineData("LongChannelName", "long_channel_name")]
    [InlineData("Long Channel Name", "long channel name")]
    [InlineData("Double  space!", "double  space!")]
    [InlineData("ALL CAPS", "all caps")]
    [InlineData("SOMECaps", "some_caps")]
    [InlineData("has_Underscore", "has_underscore")]
    [InlineData("has_ Underscore", "has_underscore")]
    [InlineData("has_ underscore", "has_underscore")]
    internal void ToSnakeCaseTest(string caller, string result)
        => Assert.Equal(result, caller.ToSnakeCase());

    [Theory]
    [InlineData("hello", 'l', 2)]
    [InlineData("hello", 'a', 0)]
    [InlineData("hello there", 'e', 3)]
    [InlineData("this has three spaces", ' ', 3)]
    internal void OccurrencesTest(string caller, char target, int result)
        => Assert.Equal(result, caller.Occurrences(target));

    [Theory]
    [InlineData(7, "hello", "banana", "avocado")]
    [InlineData(5, "hello", "", "test")]
    [InlineData(0, "", "", "")]
    internal void MaxElementLengthTest(int result, params string[] collection)
        => Assert.Equal(result, collection.MaxElementLength());

    [Theory]
    [InlineData("emoji_name", "emoji_name")]
    [InlineData(":emoji_name:", "emoji_name")]
    [InlineData(":emoji!_name:", "emoji_name")]
    [InlineData("!?:emoji_name:", "emoji_name")]
    internal void SanitizeEmojiNameTest(string caller, string result)
        => Assert.Equal(result, caller.SanitizeEmojiName());

    [Theory]
    [InlineData("test", "test")]
    [InlineData("", "")]
    [InlineData(".test", "test")]
    [InlineData("_test", "test")]
    [InlineData("t!est", "t!est")]
    [InlineData("?!test", "test")]
    [InlineData("@test", "test")]
    [InlineData("test!", "test!")]
    internal void SanitizeUsernameTest(string caller, string result)
        => Assert.Equal(result, caller.SanitizeUsername());

    [Theory]
    [InlineData("12345", "12345")]
    [InlineData("", "")]
    [InlineData("123abc45", "12345")]
    [InlineData("!1_2a3&4%5", "12345")]
    [InlineData("Nothing", "")]
    [InlineData("111e111", "111111")]
    internal void GetDigitsTest(string caller, string result)
        => Assert.Equal(result, caller.GetDigits());

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
    internal void FirstOccurrenceOfTest(string caller, char character, int match, int result)
        => Assert.Equal(result, caller.FirstOccurrenceOf(character, match));

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
    internal void LastOccurrenceOfTest(string caller, char character, int match, int result)
        => Assert.Equal(result, caller.LastOccurrenceOf(character, match));

    [Theory]
    [InlineData("!hello", "!hello", StringComparison.Ordinal, true)]
    [InlineData("!hellow", "!hello", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!hellow", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!hello there", StringComparison.Ordinal, true)]
    [InlineData("!hello there", "!hello", StringComparison.Ordinal, true)]
    [InlineData("!hello", "!heLLO", StringComparison.Ordinal, false)]
    [InlineData("!hello", "!heLLo", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("!hello", "!hellow there", StringComparison.Ordinal, false)]
    internal void HasFirstWordOfTest(string caller, string callee, StringComparison comparisonType, bool result)
        => Assert.Equal(result, caller.HasFirstWordOf(callee, comparisonType));
}