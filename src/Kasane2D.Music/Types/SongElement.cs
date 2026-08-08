namespace Kasane2D.Music.Types;

public record class SongElement(string PatternName, int RepeatCount = 1, SongElement? Next = null);