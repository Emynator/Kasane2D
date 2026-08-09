namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

public record class DmgNoiseUpdate(bool? LongMode = null, int? StepCount = null) : GeneratorUpdate;