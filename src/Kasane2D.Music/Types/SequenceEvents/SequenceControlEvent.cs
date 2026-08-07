namespace Kasane2D.Music.Types.SequenceEvents;

internal record struct SequenceControlEvent
    (
    VolumeUpdate VolumeUpdate = default,
    PanUpdate PanUpdate = default,
    EnvelopeUpdate EnvelopeUpdate = default,
    GeneratorUpdate? GeneratorUpdate = null
    );