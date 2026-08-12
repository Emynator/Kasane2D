using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

/// <summary>
/// Represents parameter updates for a <see cref="Sampler"/>
/// </summary>
/// <param name="SampleAssignments">The map samples to note assignments.</param>
public sealed record class SamplerUpdate(Dictionary<Note, MonoAudioStream> SampleAssignments) : GeneratorUpdate;