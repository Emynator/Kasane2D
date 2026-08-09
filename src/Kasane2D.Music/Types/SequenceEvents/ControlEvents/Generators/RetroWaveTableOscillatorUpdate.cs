namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

public record RetroWaveTableOscillatorUpdate(int[] Table, bool IsByte) : GeneratorUpdate;