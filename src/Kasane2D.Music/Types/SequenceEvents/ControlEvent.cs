namespace Kasane2D.Music.Types.SequenceEvents;

public readonly record struct ControlEvent
    (
    int Bar = -1,
    int Step = -1,
    VolumeUpdate VolumeUpdate = default,
    PanUpdate PanUpdate = default,
    EnvelopeUpdate EnvelopeUpdate = default,
    GeneratorUpdate? GeneratorUpdate = null
    );