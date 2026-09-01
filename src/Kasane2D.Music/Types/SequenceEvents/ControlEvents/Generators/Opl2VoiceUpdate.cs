using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

/// <summary>
/// Represents a parameter update for an <see cref="Opl2Voice"/>
/// </summary>
/// <param name="Modulator">Optional: parameter updates for the modulator operator.</param>
/// <param name="Carrier">Optional: parameter updates for the carrier operator.</param>
/// <param name="ModulationDepth">Optional: update to the modulation depth of modulator to carrier.</param>
/// <param name="IsAdditive">Optional: update to switch between additive and FM synthesis mode.</param>
public sealed record class Opl2VoiceUpdate
    (
    Opl2OperatorUpdate? Modulator = null,
    Opl2OperatorUpdate? Carrier = null,
    float? ModulationDepth = null,
    bool? IsAdditive = null
    ) : GeneratorUpdate;

/// <summary>
/// Represents a parameter update for an operator of the <see cref="Opl2Voice"/>
/// </summary>
/// <param name="EnvelopeUpdate">Optional: the updated envelope parameters.</param>
/// <param name="WaveType">Optional: update of the wave form type.</param>
/// <param name="FeedbackAmount">Optional: update to the amount of operator feedback.</param>
/// <param name="Frequency">Optional: update to the frequency or frequency ratio.</param>
/// <param name="IsFixed">Optional: switch between frequency ratio and fixed frequency mode.</param>
public readonly record struct Opl2OperatorUpdate
    (
    EnvelopeUpdate EnvelopeUpdate,
    OplWaveType? WaveType,
    float? FeedbackAmount,
    double? Frequency,
    bool? IsFixed
    );