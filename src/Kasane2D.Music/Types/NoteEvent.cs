using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types;

public record struct NoteEvent(int Bar = -1, int Step = -1, NoteEventKind Kind = NoteEventKind.None, Note Note = Note.None);