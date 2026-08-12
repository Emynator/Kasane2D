using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;

namespace Kasane2D.Music.Types;

/// <summary>
/// Represents the track pattern of a single track.
/// </summary>
/// <param name="TrackName">The track name this pattern will be played on.</param>
/// <param name="InitialSettings">Control event to define the initial control values for the track.</param>
/// <param name="NoteEvents">The notes of the pattern.</param>
/// <param name="ControlEvents">The control events of the pattern.</param>
/// <remarks>The length of ControlEvents must be equal to the number of control events expected for this pattern.
/// <seealso cref="TimeSignature.GetSequenceStepsPerBar"/></remarks>
public record class TrackPattern
    (
    string TrackName,
    ControlEvent InitialSettings,
    IReadOnlyCollection<NoteEvent> NoteEvents,
    ControlEvent[] ControlEvents
    );