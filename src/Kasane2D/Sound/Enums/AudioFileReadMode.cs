namespace Kasane2D.Sound.Enums;

/// <summary>
/// Determines how an audio file will be read from disk.
/// </summary>
public enum AudioFileReadMode
{
    /// <summary>
    /// The entire file is read into memory at once.
    /// </summary>
    Preload,
    /// <summary>
    /// The file is streamed from disk on usage and not loaded into memory.
    /// </summary>
    Stream,
    /// <summary>
    /// The file is not preloaded into memory at once and streamed from disk as it is used. Unlike streamed files, already
    /// read data will be kept in memory instead of discarded.
    /// </summary>
    CachedStream,
}