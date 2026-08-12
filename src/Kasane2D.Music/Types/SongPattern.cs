using Kasane2D.Music.Enums;

namespace Kasane2D.Music.Types;

/// <summary>
/// Represents a single pattern used in the synth engine.
/// </summary>
/// <param name="Name">The name of the pattern.</param>
/// <param name="TimeSignature">The time signature of the pattern.</param>
/// <param name="Bpm">The BPM of the pattern.</param>
/// <param name="Length">The length of the pattern in bars.</param>
/// <param name="StepSize">The step size used for note values.</param>
/// <param name="TrackPatterns">The collection of individual track patterns for each track in the synth engine.</param>
public record class SongPattern
    (
    string Name,
    TimeSignature TimeSignature,
    int Bpm,
    int Length,
    StepSize StepSize,
    IReadOnlyCollection<TrackPattern> TrackPatterns
    );