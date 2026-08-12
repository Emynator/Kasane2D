namespace Kasane2D.Music.Enums;

/// <summary>
/// The kind of sequence note event.
/// </summary>
public enum NoteEventKind
{
    /// <summary>
    /// No note event is happening.
    /// </summary>
    None,
    /// <summary>
    /// Play a note in this step.
    /// </summary>
    Begin,
    /// <summary>
    /// Hold the note of the previous step.
    /// </summary>
    Hold,
}