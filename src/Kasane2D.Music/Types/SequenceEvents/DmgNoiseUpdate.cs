namespace Kasane2D.Music.Types.SequenceEvents;

public record class DmgNoiseUpdate(bool? LongMode = null, int? StepCount = null) : GeneratorUpdate;