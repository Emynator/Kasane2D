using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents;

/// <summary>
/// Represents a note event in a track.
/// </summary>
/// <param name="Bar">The bar number of the note event.</param>
/// <param name="Step">The step number in the bar of the note event.</param>
/// <param name="Kind">The kind of note event.</param>
/// <param name="Note">The note value.</param>
public record struct NoteEvent(int Bar = -1, int Step = -1, NoteEventKind Kind = NoteEventKind.None, Note Note = Note.None);