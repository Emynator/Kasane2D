using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

/// <summary>
/// Represents parameter updates for a <see cref="BasicOscillator"/>
/// </summary>
/// <param name="NewWaveShape">Optional: the new wave form of the oscillator.</param>
/// <param name="NewDutyCycle">Optional: the new value for the square wave duty cycle.</param>
public sealed record class BasicOscillatorUpdate
    (
    BasicWaveType? NewWaveShape = null,
    double? NewDutyCycle = null
    ) : GeneratorUpdate;