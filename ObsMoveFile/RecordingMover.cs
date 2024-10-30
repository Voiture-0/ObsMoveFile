namespace ObsMoveFile;

/// <summary>
/// Handles moving completed recording files to configured directories based on filename prefix matches.
/// </summary>
public static class RecordingMover
{
    /// <summary>
    /// Moves a file to a directory based on the filename's prefix and the configured output paths.
    /// </summary>
    /// <param name="outputPaths">A dictionary of filename prefixes as keys, and the desired directory to move the file to as values.</param>
    /// <param name="filePath">The path of the current file to move.</param>
    public static void MoveFile(Dictionary<string, string> outputPaths, string filePath)
    {
        // Search for a matching prefix in output paths, then move file to corresponding directory
        foreach (var kvp in outputPaths)
        {
            var filename = Path.GetFileName(filePath);
            if (filename.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase))
            {
                MoveTo(kvp.Value, filePath);
                return;
            }
        }
        Console.WriteLine($"ERROR: Could not find output path for file: {filePath}");
    }

    /// <summary>
    /// Moves the file to the specified directory, creating the directory if it doesn't exist.
    /// </summary>
    private static void MoveTo(string dir, string filePath)
    {
        Directory.CreateDirectory(dir); // Ensure target directory exists

        var filename = Path.GetFileName(filePath);
        var newPath = Path.Combine(dir, filename);
        
        File.Move(filePath, newPath);

        Console.WriteLine($"Moved file from {filePath} to {newPath}");
    }
}
