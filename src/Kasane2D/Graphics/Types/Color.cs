namespace Kasane2D.Graphics.Types;

public struct Color
{
    public Color()
    {
    }

    public byte R { get; set; } = 0;

    public byte G { get; set; } = 0;

    public byte B { get; set; } = 0;

    public byte A { get; set; } = 0;
    
    public static Color Black = new()
    {
        R = 0,
        G = 0,
        B = 0,
        A = 255,
    };
    
    public static Color White = new()
    {
        R = 255,
        G = 255,
        B = 255,
        A = 255,
    };
    
    public static Color Transparent = new()
    {
        R = 0,
        G = 0,
        B = 0,
        A = 0,
    };
    
    public static Color Red = new()
    {
        R = 255,
        G = 0,
        B = 0,
        A = 255,
    };
    
    public static Color Green = new()
    {
        R = 0,
        G = 255,
        B = 0,
        A = 255,
    };
    
    public static Color Blue = new()
    {
        R = 0,
        G = 0,
        B = 255,
        A = 255,
    };
}