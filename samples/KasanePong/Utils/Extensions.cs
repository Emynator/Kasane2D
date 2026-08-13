using Kasane2D.Types;

namespace KasanePong.Utils;

public static class Extensions
{
    public static Vec2I ToAtlasIndex(this string character)
    {
        return character.ToLowerInvariant() switch
        {
            "a" => new(0, 2),
            "b" => new(1, 2),
            "c" => new(2, 2),
            "d" => new(3, 2),
            "e" => new(4, 2),
            "f" => new(5, 2),
            "g" => new(6, 2),
            "h" => new(7, 2),
            "i" => new(0, 3),
            "j" => new(1, 3),
            "k" => new(2, 3),
            "l" => new(3, 3),
            "m" => new(4, 3),
            "n" => new(5, 3),
            "o" => new(6, 3),
            "p" => new(7, 3),
            "q" => new(0, 4),
            "r" => new(1, 4),
            "s" => new(2, 4),
            "t" => new(3, 4),
            "u" => new(4, 4),
            "v" => new(5, 4),
            "w" => new(6, 4),
            "x" => new(7, 4),
            "y" => new(0, 5),
            "z" => new(1, 5),
            "!" => new(2, 5),
            _ => new(0, 0),
        };
    }
}