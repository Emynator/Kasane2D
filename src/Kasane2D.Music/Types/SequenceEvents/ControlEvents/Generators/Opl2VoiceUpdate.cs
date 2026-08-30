using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

public sealed record class Opl2VoiceUpdate
    (
    OperatorUpdate? Operator0 = null,
    OperatorUpdate? Operator1 = null,
    float? ModulationDepth = null,
    bool? IsAdditive = null
    ) : GeneratorUpdate;

public readonly record struct OperatorUpdate
    (
    EnvelopeUpdate EnvelopeUpdate,
    OplWaveType? WaveType,
    float? FeedbackAmount,
    double? Frequency,
    bool? IsFixed
    );