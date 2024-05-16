namespace Kotz.Tests.Extensions.Utilities;

public sealed class TryDeleteFileTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    internal void TryDeleteFileSuccessTest(bool createFile, bool expected)
    {
        var filePath = CreateFilePath(createFile);

        Assert.Equal(createFile, File.Exists(filePath));
        Assert.Equal(expected, KotzUtilities.TryDeleteFile(filePath));

        if (createFile)
            Assert.Equal(!createFile, File.Exists(filePath));
    }

    [Fact]
    internal void TryDeleteFileFailTest()
    {
        Assert.Throws<ArgumentException>(() => KotzUtilities.TryDeleteFile(string.Empty));
        Assert.Throws<ArgumentNullException>(() => KotzUtilities.TryDeleteFile(null!));
    }

    /// <summary>
    /// Creates a random file path.
    /// </summary>
    /// <param name="createFile"><see langword="true"/> if the file should be created, <see langword="false"/> otherwise.</param>
    /// <returns>The path to the file.</returns>
    internal static string CreateFilePath(bool createFile)
    {
        var filePath = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".tmp");

        if (createFile)
            File.Create(filePath).Dispose();

        return filePath;
    }
}