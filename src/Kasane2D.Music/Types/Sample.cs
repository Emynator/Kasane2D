using Kasane2D.Music.Enums;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Types;

public readonly record struct Sample(Note AssignedNote, MonoAudioStream SampleData);