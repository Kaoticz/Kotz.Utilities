using System.ComponentModel;

namespace Kotz.Tests.Extensions.Utilities;

public sealed class StartProgramTests
{
    [Fact]
    internal void StartProgramSuccessTest()
    {
        using var process1 = KotzUtilities.StartProcess("echo", "Hello from xUnit!", [(_, _) => { }], []);
        using var process2 = KotzUtilities.StartProcess("echo", ["Hello from xUnit!"], [(_, _) => { }], []);

        Assert.NotNull(process1);
        Assert.NotNull(process2);
    }

    [Fact]
    internal void StartProgramFailTest()
    {
        Assert.Throws<Win32Exception>(() => KotzUtilities.StartProcess("idonotexist"));
        Assert.Throws<Win32Exception>(() => KotzUtilities.StartProcess("idonotexist", ["args"]));
        Assert.Throws<ArgumentException>(() => KotzUtilities.StartProcess("", (string?)null!));
        Assert.Throws<ArgumentException>(() => KotzUtilities.StartProcess("", Enumerable.Empty<string>()));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.StartProcess(null!, ""));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.StartProcess(null!, Enumerable.Empty<string>()));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.StartProcess("idonotexist", (string?)null!));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.StartProcess("idonotexist", (IEnumerable<string>?)null!));
    }
}