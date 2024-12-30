# Kotz.Utilities

Defines the following types:

- **KotzUtilities**: static class with a wide range of helper methods.
    - AddPathToPATHEnvar: Adds a directory path to the PATH environment variable.
    - HasWritePermissionAt: Checks if this application can write to the specified directory.
    - ProgramExists: Checks if a program exists at the specified absolute path or the PATH environment variable.
    - StartProcess: Starts the specified program in the background.
    - TryCreate: Safely creates an object with the specified factory method.
    - TryDeleteFSO: Safely deletes a file or directory.
    - TryDeleteFile: Safely deletes a file.
    - TryDeleteDirectory: Safely deletes a directory.
    - TryMoveFSO: Safely moves a file or directory.
    - TryMoveFile: Safely moves a file.
    - TryMoveDirectory: Safely moves a directory.