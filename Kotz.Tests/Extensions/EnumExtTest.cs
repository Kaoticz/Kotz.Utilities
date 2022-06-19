using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

[Flags]
internal enum TestEnum
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
    D = 1 << 3,
    E = 1 << 4,
    All = A | B | C | D | E
}

public sealed class EnumExtTest
{
    [Theory]
    /* Individual Tests */
    [InlineData(TestEnum.A, TestEnum.A, true)]
    [InlineData(TestEnum.B, TestEnum.B, true)]
    [InlineData(TestEnum.C, TestEnum.C, true)]
    [InlineData(TestEnum.D, TestEnum.D, true)]
    [InlineData(TestEnum.E, TestEnum.E, true)]
    [InlineData(TestEnum.A, TestEnum.B, false)]
    [InlineData(TestEnum.B, TestEnum.C, false)]
    [InlineData(TestEnum.C, TestEnum.D, false)]
    [InlineData(TestEnum.D, TestEnum.E, false)]
    [InlineData(TestEnum.E, TestEnum.A, false)]
    [InlineData(TestEnum.None, TestEnum.None, false)]
    [InlineData(TestEnum.None, TestEnum.A, false)]
    [InlineData(TestEnum.A, TestEnum.None, false)]
    /* One to Many Tests */
    [InlineData(TestEnum.A, TestEnum.A | TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, true)]
    [InlineData(TestEnum.B, TestEnum.None | TestEnum.B | TestEnum.C | TestEnum.D, true)]
    [InlineData(TestEnum.C, TestEnum.A | TestEnum.B | TestEnum.C, true)]
    [InlineData(TestEnum.D, TestEnum.D | TestEnum.E, true)]
    [InlineData(TestEnum.E, TestEnum.A | TestEnum.E | TestEnum.None, true)]
    [InlineData(TestEnum.None, TestEnum.A | TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, false)]
    [InlineData(TestEnum.A, TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, false)]
    [InlineData(TestEnum.B, TestEnum.None | TestEnum.C | TestEnum.D, false)]
    [InlineData(TestEnum.C, TestEnum.A | TestEnum.B, false)]
    [InlineData(TestEnum.D, TestEnum.A | TestEnum.E, false)]
    [InlineData(TestEnum.E, TestEnum.D | TestEnum.None, false)]
    /* Many to One Tests */
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, TestEnum.A, true)]
    [InlineData(TestEnum.None | TestEnum.B | TestEnum.C | TestEnum.D, TestEnum.B, true)]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C, TestEnum.C, true)]
    [InlineData(TestEnum.D | TestEnum.E, TestEnum.D, true)]
    [InlineData(TestEnum.E | TestEnum.None, TestEnum.D | TestEnum.E, true)]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, TestEnum.None, false)]
    [InlineData(TestEnum.B | TestEnum.C | TestEnum.D | TestEnum.E, TestEnum.A, false)]
    [InlineData(TestEnum.None | TestEnum.C | TestEnum.D, TestEnum.B, false)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.C, false)]
    [InlineData(TestEnum.A | TestEnum.E, TestEnum.D, false)]
    [InlineData(TestEnum.D | TestEnum.None | TestEnum.C, TestEnum.E, false)]
    /* Many to Many Tests */
    [InlineData(TestEnum.None | TestEnum.A | TestEnum.B, TestEnum.B | TestEnum.C | TestEnum.D, true)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A | TestEnum.C | TestEnum.D, true)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A | TestEnum.B | TestEnum.D, true)]
    [InlineData(TestEnum.None | TestEnum.B, TestEnum.A | TestEnum.C | TestEnum.None, false)]
    [InlineData(TestEnum.None | TestEnum.B, TestEnum.A | TestEnum.C | TestEnum.D, false)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.C | TestEnum.D | TestEnum.E, false)]
    internal void HasOneFlagTests<T>(T caller, T calle, bool result) where T : struct, Enum
        => Assert.Equal(result, caller.HasOneFlag(calle));

    [Theory]
    [InlineData(TestEnum.A, "A")]
    [InlineData(TestEnum.None | TestEnum.B, "B")]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C, "A")]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C, "A", "B")]
    [InlineData(TestEnum.A, "A", "B", "C")]
    internal void ToFlagStringTestTrue<T>(T enumGroup, params string[] responses) where T : struct, Enum
    {
        var actualStrings = enumGroup.ToStrings().ToArray();
        Assert.Contains(responses, x => actualStrings.Contains(x));
    }

    [Theory]
    [InlineData(TestEnum.A, "B")]
    [InlineData(TestEnum.None | TestEnum.B, "A", "None")]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C, "D")]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.C, "D", "E")]
    [InlineData(TestEnum.A, "None", "B", "C")]
    internal void ToFlagStringTestFalse<T>(T enumGroup, params string[] responses) where T : struct, Enum
    {
        var actualStrings = enumGroup.ToStrings().ToArray();
        Assert.DoesNotContain(responses, x => actualStrings.Contains(x));
    }

    [Theory]
    [InlineData(TestEnum.A, TestEnum.A)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A, TestEnum.B)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.None, TestEnum.A, TestEnum.B)]
    [InlineData(TestEnum.A | TestEnum.B | TestEnum.D, TestEnum.A, TestEnum.B, TestEnum.D)]
    internal void ToFlagsTest(TestEnum enumGroup, params TestEnum[] flags) // Generic params are not supported
        => Assert.Equal(enumGroup, flags.ToFlags());

    [Theory]
    [InlineData(TestEnum.None, TestEnum.A, TestEnum.A)]
    [InlineData(TestEnum.A, TestEnum.A, TestEnum.None)]
    [InlineData(TestEnum.A, TestEnum.None, TestEnum.A)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A, TestEnum.B)]
    [InlineData(TestEnum.A, TestEnum.B, TestEnum.A | TestEnum.B)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A | TestEnum.B, TestEnum.None)]
    [InlineData(TestEnum.A | TestEnum.C, TestEnum.B, TestEnum.A | TestEnum.B | TestEnum.C)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.C | TestEnum.D, TestEnum.A | TestEnum.B | TestEnum.C | TestEnum.D)]
    internal void ToggleFlagTest<T>(T enumGroup, T toToggle, T expectedValue) where T : struct, Enum
        => Assert.Equal(expectedValue, enumGroup.ToggleFlag(toToggle));

    [Theory]
    [InlineData(TestEnum.None, TestEnum.None)]
    [InlineData(TestEnum.A, TestEnum.A)]
    [InlineData(TestEnum.A | TestEnum.B, TestEnum.A, TestEnum.B)]
    [InlineData(TestEnum.A | TestEnum.C | TestEnum.B, TestEnum.A, TestEnum.B, TestEnum.C)]
    [InlineData(TestEnum.All, TestEnum.A, TestEnum.B, TestEnum.C, TestEnum.D, TestEnum.E)]
    internal void ToValuesTest(TestEnum enumGroup, params TestEnum[] expectedValue)
    {
        Assert.Equal(
            expectedValue,
            enumGroup.ToValues()
                .Where(x => x is not TestEnum.All)
                .When(x => x.Count() != 1 || x.FirstOrDefault() is not TestEnum.None, x => x.Where(x => x is not TestEnum.None))
                .ToArray()
        );
    }
}