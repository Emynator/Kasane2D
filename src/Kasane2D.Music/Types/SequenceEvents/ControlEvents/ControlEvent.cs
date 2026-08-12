using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

/// <summary>
/// Represents a control event in a track.
/// </summary>
/// <param name="VolumeUpdate">Optional: the change to the track volume.</param>
/// <param name="PanUpdate">Optional: the change to the track pan.</param>
/// <param name="EnvelopeUpdate">Optional: the changes to the track's envelope.</param>
/// <param name="GeneratorUpdate">Optional: the changes to the track's generator.</param>
public readonly record struct ControlEvent
    (
    VolumeUpdate VolumeUpdate = default,
    PanUpdate PanUpdate = default,
    EnvelopeUpdate EnvelopeUpdate = default,
    GeneratorUpdate? GeneratorUpdate = null
    )
{
    public static readonly ControlEvent Empty = new ControlEvent();
}