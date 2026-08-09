using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;

namespace Kasane2D.Music.Types;

public record class TrackPattern
    (
    string TrackName,
    ControlEvent InitialSettings,
    IReadOnlyCollection<NoteEvent> NoteEvents,
    IReadOnlyCollection<ControlEvent> ControlEvents
    );