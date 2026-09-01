namespace Kasane2D.Music.Types;

internal record class ProcessedSong
    (
    string Name,
    Dictionary<string, ProcessedSongPattern> Patterns,
    Dictionary<string, SongElement> Sections
    );