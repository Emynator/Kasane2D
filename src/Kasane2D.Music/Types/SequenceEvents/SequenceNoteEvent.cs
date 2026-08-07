using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents;

internal record struct SequenceNoteEvent(Note Note = Note.None, SequenceNoteEventKind Kind = SequenceNoteEventKind.Off);