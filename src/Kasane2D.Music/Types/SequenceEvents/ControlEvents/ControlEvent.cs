using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

public readonly record struct ControlEvent
    (
    int Bar = -1,
    int Step = -1,
    VolumeUpdate VolumeUpdate = default,
    PanUpdate PanUpdate = default,
    EnvelopeUpdate EnvelopeUpdate = default,
    GeneratorUpdate? GeneratorUpdate = null
    );