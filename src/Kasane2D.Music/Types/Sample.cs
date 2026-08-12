using Kasane2D.Music.Enums;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Types;

/// <summary>
/// Represents a sample to be assigned to a sampler.
/// </summary>
/// <param name="AssignedNote">Note this sample belongs to.</param>
/// <param name="SampleData">The sample data.</param>
public readonly record struct Sample(Note AssignedNote, MonoAudioStream SampleData);