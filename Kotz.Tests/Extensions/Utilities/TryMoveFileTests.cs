using Kotz.Utilities;

namespace Kotz.Tests.Extensions.Utilities;

public sealed class TryMoveFileTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryMoveFileRenameSuccessTests(bool createFile, bool expected)
    {
        var oldPath = TryDeleteFileTests.CreateFilePath(createFile);
        var newPath = TryDeleteFileTests.CreateFilePath(false);

        Assert.Equal(createFile, File.Exists(oldPath));
        Assert.Equal(expected, KotzUtilities.TryMoveFile(oldPath, newPath));

        if (!createFile)
            return;

        Assert.Equal(!createFile, File.Exists(oldPath));
        Assert.False(KotzUtilities.TryDeleteFile(oldPath));
        Assert.True(KotzUtilities.TryDeleteFile(newPath));
        Assert.False(File.Exists(newPath));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryMoveFileSuccessTests(bool createFile, bool expected)
    {
        var oldPath = TryDeleteFileTests.CreateFilePath(createFile);
        var newPath = Path.Join(TryDeleteDirectoryTests.CreateDirectoryPath(false), Path.GetFileName(oldPath));

        Assert.Equal(createFile, File.Exists(oldPath));
        Assert.Equal(expected, KotzUtilities.TryMoveFile(oldPath, newPath));

        if (!createFile)
            return;

        Assert.Equal(!createFile, File.Exists(oldPath));
        Assert.False(KotzUtilities.TryDeleteFile(oldPath));
        Assert.True(KotzUtilities.TryDeleteFile(newPath));
        Assert.False(File.Exists(newPath));
    }

    [Fact]
    internal void TryMoveFileFailTests()
    {
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryMoveFile(string.Empty, "not empty"));
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryMoveFile("not empty", string.Empty));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryMoveFile(null!, "not empty"));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryMoveFile("not empty", null!));
    }
}