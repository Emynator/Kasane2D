using Kasane2D.Music.Enums;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

public record class SamplerUpdate(Dictionary<Note, MonoAudioStream> SampleAssignments) : GeneratorUpdate;