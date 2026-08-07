namespace Kasane2D.Music.Types;

public record class TrackPattern
    (
    string TrackName,
    IReadOnlyCollection<NoteEvent> NoteEvents,
    IReadOnlyCollection<ControlEvent> ControlEvents
    );