namespace Kotz.Tests.Extensions.Utilities;

public sealed class HasWritePermissionAtTests
{
    [Fact]
    internal void HasWritePermissionAtTrueTest()
        => Assert.True(KotzUtilities.HasWritePermissionAt(Path.GetTempPath()));

    [Fact]
    internal void HasWritePermissionAtFalseTest()
    {
        var directoryUri = OperatingSystem.IsWindows()
            ? Environment.GetFolderPath(Environment.SpecialFolder.System)
            : "/";

        Assert.False(KotzUtilities.HasWritePermissionAt(directoryUri));
    }

    [Fact]
    internal void HasWritePermissionAtFailTest()
    {
        var fakeUri = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Guid.NewGuid().ToString());
        Assert.Throws<DirectoryNotFoundException>(() => KotzUtilities.HasWritePermissionAt(fakeUri));
    }
}