namespace Kasane2D.Music.Types;

/// <summary>
/// Defines how a pattern is used in a song section.
/// </summary>
/// <param name="PatternName">The pattern belonging to this element.</param>
/// <param name="RepeatCount">Optional: How many times the pattern should repeat before moving to the next element.
/// Default is 1.</param>
/// <param name="Next">Optional: The song element playing after this one. Default is null.</param>
public record class SongElement(string PatternName, int RepeatCount = 1, SongElement? Next = null)
{
    /// <summary>
    /// Gets the song element playing after this one.
    /// </summary>
    public SongElement? Next { get; set; }
}