using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Kotz.Extensions;

/// <summary>
/// A collection of utility methods.
/// </summary>
public static class KotzUtilities
{
    private static readonly string _programVerifier = (OperatingSystem.IsWindows()) ? "where" : "which";
    private static readonly string _envPathSeparator = (OperatingSystem.IsWindows()) ? ";" : ":";
    private static readonly EnvironmentVariableTarget _envTarget = (OperatingSystem.IsWindows())
        ? EnvironmentVariableTarget.User
        : EnvironmentVariableTarget.Process;

    /// <summary>
    /// Adds a directory path to the PATH environment variable.
    /// </summary>
    /// <param name="directoryUri">The absolute path to a directory.</param>
    /// <remarks>
    /// On Windows, this method needs to be called once and the dependencies will be available for the user forever. <br />
    /// On Unix systems, it's only possible to add to the PATH on a process basis, so this method needs to be called
    /// at least once everytime the application is executed.
    /// </remarks>
    /// <returns><see langword="true"/> if <paramref name="directoryUri"/> got successfully added to the PATH envar, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool AddPathToPATHEnvar(string directoryUri)
    {
        ArgumentException.ThrowIfNullOrEmpty(directoryUri, nameof(directoryUri));

        if (File.Exists(directoryUri))
            throw new ArgumentException("Parameter must point to a directory, not a file.", nameof(directoryUri));

        var envPathValue = Environment.GetEnvironmentVariable("PATH", _envTarget) ?? string.Empty;

        // If directoryPath is already in the PATH envar, don't add it again.
        if (envPathValue.Contains(directoryUri, StringComparison.Ordinal))
            return false;

        var newPathEnvValue = envPathValue + _envPathSeparator + directoryUri;

        // Add path to Windows' user envar, so it persists across reboots.
        if (OperatingSystem.IsWindows())
            Environment.SetEnvironmentVariable("PATH", newPathEnvValue, EnvironmentVariableTarget.User);

        // Add path to the current process' envar, so the program can see the dependencies.
        Environment.SetEnvironmentVariable("PATH", newPathEnvValue, EnvironmentVariableTarget.Process);

        return true;
    }

    /// <summary>
    /// Checks if this application can write to the specified directory.
    /// </summary>
    /// <param name="directoryUri">The absolute path to a directory.</param>
    /// <returns><see langword="true"/> if writing is allowed, <see langword="false"/> otherwise.</returns>
    /// <exception cref="PathTooLongException" />
    /// <exception cref="DirectoryNotFoundException" />
    public static bool HasWritePermissionAt(ReadOnlySpan<char> directoryUri)
    {
        var tempFileUri = Path.Join(directoryUri, Path.GetRandomFileName() + ".tmp");

        try
        {
            using var fileStream = File.Create(tempFileUri);
            return true;
        }
        catch (Exception ex) when (ex is not PathTooLongException and not DirectoryNotFoundException)
        {
            return false;
        }
        finally
        {
            TryDeleteFile(tempFileUri);
        }
    }

    /// <summary>
    /// Checks if a program exists at the specified absolute path or the PATH environment variable.
    /// </summary>
    /// <param name="programName">The name of the program.</param>
    /// <returns><see langword="true"/> if the program exists, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool ProgramExists(string programName)
    {
        ArgumentException.ThrowIfNullOrEmpty(programName, nameof(programName));

        using var process = StartProcess(_programVerifier, programName, [(_, _) => { }], [(_, _) => { }]);
        process.WaitForExit();

        return process.ExitCode is 0;
    }


    /// <summary>
    /// Starts the specified program in the background.
    /// </summary>
    /// <param name="program">
    /// The name of the program in the PATH environment variable,
    /// or the absolute path to its executable.
    /// </param>
    /// <param name="arguments">The arguments to the program.</param>
    /// <param name="stdoutRedirectHandlers">Defines the handlers for redirected Standard Output data.</param>
    /// <param name="stderrRedirectHandlers">Defines the handlers for redirected Standard Error data.</param>
    /// <remarks>
    /// The <paramref name="arguments"/> parameter is not escaped, you can either escape it yourself or use
    /// <see cref="StartProcess(string, IEnumerable{string}, IReadOnlyCollection{DataReceivedEventHandler}?, IReadOnlyCollection{DataReceivedEventHandler}?)"/>
    /// instead.
    /// </remarks>
    /// <returns>The process of the specified program.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="Win32Exception">Occurs when <paramref name="program"/> does not exist.</exception>
    /// <exception cref="InvalidOperationException">Occurs when the process fails to execute.</exception>
    public static Process StartProcess(
            string program,
            string arguments = "",
            IReadOnlyCollection<DataReceivedEventHandler>? stdoutRedirectHandlers = default,
            IReadOnlyCollection<DataReceivedEventHandler>? stderrRedirectHandlers = default
        )
    {
        ArgumentException.ThrowIfNullOrEmpty(program, nameof(program));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));

        var process = Process.Start(new ProcessStartInfo()
        {
            FileName = program,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = stdoutRedirectHandlers is { Count: not 0 },
            RedirectStandardError = stderrRedirectHandlers is { Count: not 0 }
        }) ?? throw new InvalidOperationException($"Failed spawing process for: {program} {arguments}");

        return EnableProcessEvents(process, stdoutRedirectHandlers, stderrRedirectHandlers);
    }

    /// <summary>
    /// Starts the specified program in the background.
    /// </summary>
    /// <param name="program">
    /// The name of the program in the PATH environment variable,
    /// or the absolute path to its executable.
    /// </param>
    /// <param name="arguments">The arguments to the program.</param>
    /// <param name="stdoutRedirectHandlers">Defines the handlers for redirected Standard Output data.</param>
    /// <param name="stderrRedirectHandlers">Defines the handlers for redirected Standard Error data.</param>
    /// <remarks>The <paramref name="arguments"/> get automatically escaped.</remarks>
    /// <returns>The process of the specified program.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="Win32Exception">Occurs when <paramref name="program"/> does not exist.</exception>
    /// <exception cref="InvalidOperationException">Occurs when the process fails to execute.</exception>
    public static Process StartProcess(
            string program,
            IEnumerable<string> arguments,
            IReadOnlyCollection<DataReceivedEventHandler>? stdoutRedirectHandlers = default,
            IReadOnlyCollection<DataReceivedEventHandler>? stderrRedirectHandlers = default
        )
    {
        ArgumentException.ThrowIfNullOrEmpty(program, nameof(program));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));

        var processInfo = new ProcessStartInfo()
        {
            FileName = program,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = stdoutRedirectHandlers is { Count: not 0 },
            RedirectStandardError = stderrRedirectHandlers is { Count: not 0 }
        };

        foreach (var argument in arguments)
            processInfo.ArgumentList.Add(argument);

        var process =  Process.Start(processInfo)
            ?? throw new InvalidOperationException($"Failed spawing process for: {program} {string.Join(' ', processInfo.ArgumentList)}");

        return EnableProcessEvents(process, stdoutRedirectHandlers, stderrRedirectHandlers);
    }

    /// <summary>
    /// Safely creates a <typeparamref name="T"/> object with the specified <paramref name="factory"/> method.
    /// </summary>
    /// <param name="factory">The factory method that creates the object.</param>
    /// <param name="result">The created object or <see langword="default"/> if creation does not succeed.</param>
    /// <param name="exception">The exception thrown by the <paramref name="factory"/> or <see langword="null"/> if creation of the object succeeds.</param>
    /// <typeparam name="T">The type of the object being created.</typeparam>
    /// <returns><see langword="true"/> if the object was successfully created, <see langword="false"/> otherwise.</returns>
    public static bool TryCreate<T>(Func<T> factory, [MaybeNullWhen(false)] out T result, [MaybeNullWhen(true)] out Exception exception)
    {
        try
        {
            result = factory();
            exception = default;
            return true;
        }
        catch (Exception ex)
        {
            result = default;
            exception = ex;
            return false;
        }
    }

    /// <summary>
    /// Safely deletes a file or directory.
    /// </summary>
    /// <param name="fsoUri">The absolute path to the File System Object.</param>
    /// <param name="isRecursive">
    /// <see langword="true"/> to remove directories, subdirectories, and files in the path, <see langword="false"/> otherwise.
    /// This is only used when deleting a directory.
    /// </param>
    /// <returns><see langword="true"/> if the File System Object was deleted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="IOException" />
    /// <exception cref="DirectoryNotFoundException" />
    /// <exception cref="NotSupportedException" />
    /// <exception cref="PathTooLongException" />
    /// <exception cref="UnauthorizedAccessException" />
    public static bool TryDeleteFSO(string fsoUri, bool isRecursive = true)
        => TryDeleteFile(fsoUri) || TryDeleteDirectory(fsoUri, isRecursive);

    /// <summary>
    /// Safely deletes a file.
    /// </summary>
    /// <param name="fileUri">The absolute path to the file.</param>
    /// <returns><see langword="true"/> if the file was deleted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool TryDeleteFile(string fileUri)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileUri, nameof(fileUri));

        if (!File.Exists(fileUri))
            return false;

        try
        {
            File.Delete(fileUri);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Safely deletes a directory.
    /// </summary>
    /// <param name="directoryUri">The absolute path to the directory.</param>
    /// <param name="isRecursive">
    /// <see langword="true"/> to remove directories, subdirectories, and files in the path, <see langword="false"/> otherwise.
    /// </param>
    /// <returns><see langword="true"/> if the directory was deleted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool TryDeleteDirectory(string directoryUri, bool isRecursive = true)
    {
        ArgumentException.ThrowIfNullOrEmpty(directoryUri, nameof(directoryUri));

        if (!Directory.Exists(directoryUri))
            return false;

        try
        {
            Directory.Delete(directoryUri, isRecursive);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Safely moves a file or directory.
    /// </summary>
    /// <param name="source">The path to the source file or directory.</param>
    /// <param name="destination">The path to the destination file or directory.</param>
    /// <returns><see langword="true"/> if the file system object got successfully moved, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool TryMoveFSO(string source, string destination)
        => TryMoveFile(source, destination) || TryMoveDirectory(source, destination);

    /// <summary>
    /// Safely moves a file.
    /// </summary>
    /// <param name="source">The path to the source file.</param>
    /// <param name="destination">The path to the destination file.</param>
    /// <returns><see langword="true"/> if the file got successfully moved, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool TryMoveFile(string source, string destination)
    {
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(destination, nameof(destination));

        if (!File.Exists(source))
            return false;

        try
        {
            var directoryUri = Directory.GetParent(destination)
                ?? Directory.CreateDirectory(Directory.GetDirectoryRoot(destination));

            if (!directoryUri.Exists)
                directoryUri.Create();

            File.Move(source, destination);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Safely moves a directory.
    /// </summary>
    /// <param name="source">The path to the source directory.</param>
    /// <param name="destination">The path to the destination directory.</param>
    /// <returns><see langword="true"/> if the directory got successfully moved, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    public static bool TryMoveDirectory(string source, string destination)
    {
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(destination, nameof(destination));

        if (!Directory.Exists(source))
            return false;

        try
        {
            var directoryUri = Directory.GetParent(destination)
                ?? Directory.CreateDirectory(Directory.GetDirectoryRoot(destination));

            if (!directoryUri.Exists)
                directoryUri.Create();

            Directory.Move(source, destination);
            return true;
        }
        catch (IOException)
        {
            try
            {
                return CopyMoveDirectory(source, destination, true);
            }
            catch
            {
                TryDeleteDirectory(destination, true);
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Enables event raising for the specified handlers, if there are any,
    /// </summary>
    /// <param name="process">The process to enable events.</param>
    /// <param name="stdoutRedirectHandlers">The handlers for redirected Standard Output data.</param>
    /// <param name="stderrRedirectHandlers">The handlers for redirected Standard Error data.</param>
    /// <returns>The <paramref name="process"/> with events enabled or not if no handlers are provided.</returns>
    private static Process EnableProcessEvents(
            Process process,
            IReadOnlyCollection<DataReceivedEventHandler>? stdoutRedirectHandlers,
            IReadOnlyCollection<DataReceivedEventHandler>? stderrRedirectHandlers
        )
    {
        if (stdoutRedirectHandlers is { Count: not 0 })
        {
            process.EnableRaisingEvents = true;

            foreach (var handler in stdoutRedirectHandlers)
                process.OutputDataReceived += handler;

            process.BeginOutputReadLine();
        }

        if (stderrRedirectHandlers is { Count: not 0 })
        {
            process.EnableRaisingEvents = true;

            foreach (var handler in stderrRedirectHandlers)
                process.ErrorDataReceived += handler;

            process.BeginErrorReadLine();
        }

        return process;
    }

    /// <summary>
    /// Moves a directory from <paramref name="source"/> to <paramref name="destination"/>.
    /// </summary>
    /// <param name="source">The path to the source directory.</param>
    /// <param name="destination">The path to the destination directory.</param>
    /// <param name="deleteSourceFSOs">Determines whether the files and directories in <paramref name="source"/> should be deleted or not.</param>
    /// <remarks>Use this method to circumvent this issue: https://github.com/dotnet/runtime/issues/31149</remarks>
    /// <returns><see langword="true"/> if the directory got moved, <see langword="false"/> otherwise.</returns>
    /// <exception cref="IOException">Occurs when one of the files to be moved is still in use.</exception>
    /// <exception cref="PathTooLongException" />
    /// <exception cref="UnauthorizedAccessException" />
    private static bool CopyMoveDirectory(string source, string destination, bool deleteSourceFSOs)
    {
        if (!Directory.Exists(source) || !Uri.IsWellFormedUriString(destination, UriKind.RelativeOrAbsolute))
            return false;

        var sourceDir = Directory.CreateDirectory(source);
        var destinationDir = (Directory.Exists(destination))
            ? Directory.CreateDirectory(Path.Join(destination, Path.GetFileName(source)))
            : Directory.CreateDirectory(destination);

        foreach (var fileUri in Directory.EnumerateFiles(source))
            File.Copy(fileUri, Path.Join(destinationDir.FullName, Path.GetFileName(fileUri)));

        foreach (var directoryUri in Directory.EnumerateDirectories(source))
            CopyMoveDirectory(directoryUri, Path.Join(destinationDir.FullName, Path.GetFileName(directoryUri)), false);

        if (deleteSourceFSOs)
        {
            // If 'source' is not the root of the drive, delete it.
            if (sourceDir.FullName != Path.GetPathRoot(source))
                sourceDir.Delete(true);
            else
            {
                // Else, delete all file system objects directly.
                foreach (var fileUri in Directory.EnumerateFiles(source))
                    File.Delete(fileUri);

                foreach (var directoryUri in Directory.EnumerateDirectories(source))
                    Directory.Delete(directoryUri, true);
            }
        }

        return true;
    }
}