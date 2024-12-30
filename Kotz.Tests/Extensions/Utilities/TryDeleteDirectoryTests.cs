using Kotz.Utilities;

namespace Kotz.Tests.Extensions.Utilities;

public sealed class TryDeleteDirectoryTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryDeleteDirectorySuccessTest(bool createDirectory, bool expected)
    {
        var directoryPath = CreateDirectoryPath(createDirectory);

        Assert.Equal(createDirectory, Directory.Exists(directoryPath));
        Assert.Equal(expected, KotzUtilities.TryDeleteDirectory(directoryPath));

        if (createDirectory)
            Assert.Equal(!createDirectory, Directory.Exists(directoryPath));
    }

    [Fact]
    internal void TryDeleteDirectoryFailTest()
    {
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryDeleteDirectory(string.Empty));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryDeleteDirectory(null!));
    }

    /// <summary>
    /// Creates a random directory path.
    /// </summary>
    /// <param name="createDirectory"><see langword="true"/> if the directory should be created, <see langword="false"/> otherwise.</param>
    /// <returns>The path to the directory.</returns>
    internal static string CreateDirectoryPath(bool createDirectory)
    {
        var directoryPath = Path.Join(Path.GetTempPath(), Path.GetRandomFileName());

        if (createDirectory)
            Directory.CreateDirectory(directoryPath).Create();

        return directoryPath;
    }
}