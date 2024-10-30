namespace ObsMoveFile;

/// <summary>
/// Holds application settings for the OBS WebSocket connection and file output paths.
/// Populated from appsettings.json.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// The port for your OBS WebSocket Server. Found in Tools > WebSocket Server Settings > Server Port
    /// </summary>
    public required string WebSocketPort { get; set; }
    /// <summary>
    /// The password for your OBS WebSocket Server. Found in Tools > WebSocket Server Settings > Server Password
    /// </summary>
    public required string WebSocketPassword { get; set; }

    /// <summary>
    /// Whether or not to attempt to retry connection on disconnection
    /// </summary>
    public bool RetryConnection { get; set; }
    /// <summary>
    /// How many seconds to wait between connection retries
    /// </summary>
    public int RetryIntervalSeconds { get; set; }

    /// <summary>
    /// An object map of:
    /// Keys of file prefixes
    /// Paths to save files to with the prefix key
    /// </summary>
    public required Dictionary<string, string> OutputPaths { get; set; }
}
