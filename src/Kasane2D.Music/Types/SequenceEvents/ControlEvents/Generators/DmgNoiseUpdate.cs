using Kasane2D.Music.Synthesis.Generators;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

/// <summary>
/// Represents parameter updates for a <see cref="DmgNoise"/> generator.
/// </summary>
/// <param name="LongMode">Optional: true if the LFSR should be 16 bits long, false if it should be 8 bits long.</param>
/// <param name="StepCount">Optional: Number of phase iterations before the noise generator shifts the LFSR.</param>
public sealed record class DmgNoiseUpdate(bool? LongMode = null, int? StepCount = null) : GeneratorUpdate;