using Kasane2D.Music.Interfaces;

namespace Kasane2D.Music.Types;

/// <summary>
/// Represents a song to be managed by an <see cref="IConductor"/>
/// </summary>
/// <param name="Name">Name of the song.</param>
/// <param name="Patterns">All the patterns used in the song.</param>
/// <param name="Sections">The song sections.</param>
public record class Song
    (
    string Name,
    Dictionary<string, SongPattern> Patterns,
    Dictionary<string, SongElement> Sections
    );