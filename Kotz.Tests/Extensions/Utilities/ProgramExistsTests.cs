using Kotz.Utilities;

namespace Kotz.Tests.Extensions.Utilities;

public sealed class ProgramExistsTests
{
    [Theory]
    [InlineData(true, "echo")]
    [InlineData(false, "abcde")]
    internal void ProgramExistsSuccessTest(bool expected, string command)
        => Assert.Equal(expected, KotzUtilities.ProgramExists(command));

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    internal void ProgramExistsFailTest(string? command)
        => Assert.ThrowsAny<ArgumentException>(() => KotzUtilities.ProgramExists(command!));
}