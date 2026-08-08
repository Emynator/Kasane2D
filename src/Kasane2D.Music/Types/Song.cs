namespace Kasane2D.Music.Types;

public record class Song
    (
    string Name,
    Dictionary<string, SongPattern> Patterns,
    Dictionary<string, SongElement> Sections
    );