using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types;

public record class SongPattern
    (
    string Name,
    TimeSignature TimeSignature,
    int Bpm,
    int Length,
    StepSize StepSize,
    IReadOnlyCollection<TrackPattern> TrackPatterns
    );