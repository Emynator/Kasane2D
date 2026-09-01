using Kasane2D.Events;
using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Interfaces;

/// <summary>
/// Represents a single track of an <see cref="ISynthEngine"/>
/// </summary>
public interface ITrack
{
    /// <summary>
    /// The name of the track.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Event that triggers whenever a note is played on this track.
    /// </summary>
    public KasaneEvent<Note> NotePlayEvent { get; }
    
    /// <summary>
    /// Event that triggers whenever a note is released on this track.
    /// </summary>
    public KasaneEvent NoteReleaseEvent { get; }
}