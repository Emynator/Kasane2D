using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

internal record struct SequenceControlEvent
    (
    VolumeUpdate VolumeUpdate = default,
    PanUpdate PanUpdate = default,
    EnvelopeUpdate EnvelopeUpdate = default,
    GeneratorUpdate? GeneratorUpdate = null
    );