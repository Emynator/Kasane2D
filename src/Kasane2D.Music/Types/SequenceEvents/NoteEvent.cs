using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents;

/// <summary>
/// Represents a note event in a track.
/// </summary>
/// <param name="Kind">The kind of note event.</param>
/// <param name="Note">The note value.</param>
public record struct NoteEvent(NoteEventKind Kind = NoteEventKind.None, Note Note = Note.None);