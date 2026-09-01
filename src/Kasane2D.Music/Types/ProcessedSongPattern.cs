namespace Kasane2D.Music.Types;

internal record class ProcessedSongPattern
    (
    string Name,
    int PatternLength,
    int SamplesPerStep,
    Dictionary<string, Sequence> Sequences
    );