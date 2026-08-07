using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types.SequenceEvents;

public record class BasicOscillatorUpdate
    (
    BasicWave? NewWaveShape = null,
    double? NewDutyCycle = null
    ) : GeneratorUpdate;