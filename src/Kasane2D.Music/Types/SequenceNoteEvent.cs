using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types;

internal record struct SequenceNoteEvent(Note Note = Note.None, SequenceNoteEventKind Kind = SequenceNoteEventKind.Off);