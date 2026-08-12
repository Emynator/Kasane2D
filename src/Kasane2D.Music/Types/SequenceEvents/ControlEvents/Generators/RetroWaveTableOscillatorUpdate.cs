using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

/// <summary>
/// Represents parameter updates for a <see cref="RetroWaveTableOscillator"/>
/// </summary>
/// <param name="Table">Optional: the new wave table data.</param>
/// <param name="IsByte">Optional: true if the wave table values are 8 bit, false if they are 4 bit.</param>
public sealed record RetroWaveTableOscillatorUpdate(int[] Table, bool IsByte) : GeneratorUpdate;