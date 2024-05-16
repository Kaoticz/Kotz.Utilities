namespace Kotz.Tests.Extensions.Utilities;

public sealed class TryMoveDirectoryTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryMoveDirectoryRenameSuccessTests(bool createdirectory, bool expected)
    {
        var oldPath = TryDeleteDirectoryTests.CreateDirectoryPath(createdirectory);
        var newPath = TryDeleteDirectoryTests.CreateDirectoryPath(false);

        Assert.Equal(createdirectory, Directory.Exists(oldPath));
        Assert.Equal(expected, KotzUtilities.TryMoveDirectory(oldPath, newPath));

        if (!createdirectory)
            return;

        Assert.Equal(!createdirectory, Directory.Exists(oldPath));
        Assert.False(KotzUtilities.TryDeleteDirectory(oldPath));
        Assert.True(KotzUtilities.TryDeleteDirectory(newPath));
        Assert.False(Directory.Exists(newPath));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryMoveDirectorySuccessTests(bool createDirectory, bool expected)
    {
        var oldPath = TryDeleteDirectoryTests.CreateDirectoryPath(createDirectory);
        var newPath = Path.Join(TryDeleteDirectoryTests.CreateDirectoryPath(false), Path.GetFileName(oldPath));

        Assert.Equal(createDirectory, Directory.Exists(oldPath));
        Assert.Equal(expected, KotzUtilities.TryMoveDirectory(oldPath, newPath));

        if (!createDirectory)
            return;

        Assert.Equal(!createDirectory, Directory.Exists(oldPath));
        Assert.False(KotzUtilities.TryDeleteDirectory(oldPath));
        Assert.True(KotzUtilities.TryDeleteDirectory(newPath));
        Assert.False(File.Exists(newPath));
    }

    [Fact]
    internal void TryMoveDirectoryVolumeTest()
    {
        // Define the directory tree
        var oldRootDirPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()); // Root directory
        var oldDir1Path = Path.Join(oldRootDirPath, Path.GetRandomFileName());              // In: oldRootDirPath > oldDir1Path
        var oldFile1Path = Path.Join(oldRootDirPath, Path.GetRandomFileName() + ".tmp");    // Inside: oldRootDirPath
        var oldDir2Path = Path.Join(oldRootDirPath, Path.GetRandomFileName());              // In: oldRootDirPath > oldDir2Path
        var oldFile2Path = Path.Join(oldDir2Path, Path.GetRandomFileName() + ".tmp");       // Inside: oldRootDirPath > oldDir2Path

        // Create the directory tree
        Directory.CreateDirectory(oldDir1Path);
        Directory.CreateDirectory(oldDir2Path);
        File.Create(oldFile1Path).Dispose();
        File.Create(oldFile2Path).Dispose();

        // Move to a different volume (ie. "/tmp")
        var newRootDirPath = Path.Join(Path.GetTempPath(), Path.GetFileName(oldRootDirPath));
        Assert.True(KotzUtilities.TryMoveDirectory(oldRootDirPath, newRootDirPath));

        // Check if move was successful
        Assert.False(Directory.Exists(oldRootDirPath));
        Assert.True(Directory.Exists(newRootDirPath));
        Assert.True(Directory.Exists(Path.Join(newRootDirPath, Path.GetFileName(oldDir1Path))));
        Assert.True(Directory.Exists(Path.Join(newRootDirPath, Path.GetFileName(oldDir2Path))));
        Assert.True(File.Exists(Path.Join(newRootDirPath, Path.GetFileName(oldFile1Path))));
        Assert.True(File.Exists(Path.Join(newRootDirPath, Path.GetFileName(oldDir2Path), Path.GetFileName(oldFile2Path))));

        // Cleanup
        Directory.Delete(newRootDirPath, true);
        Assert.False(Directory.Exists(newRootDirPath));
    }

    [Fact]
    internal void TryMoveDirectoryFailTests()
    {
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryMoveDirectory(string.Empty, "not empty"));
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryMoveDirectory("not empty", string.Empty));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryMoveDirectory(null!, "not empty"));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryMoveDirectory("not empty", null!));
    }
}