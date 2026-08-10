namespace Kasane2D.Graphics.Types;

/// <summary>
/// Represents a RGBA-color.
/// </summary>
public struct Color
{
    /// <summary>
    /// Create a new, empty color.
    /// </summary>
    public Color()
    {
    }

    /// <summary>
    /// Red component of the color.
    /// </summary>
    public byte R { get; set; } = 0;

    /// <summary>
    /// Green component of the color.
    /// </summary>
    public byte G { get; set; } = 0;

    /// <summary>
    /// Blue component of the color.
    /// </summary>
    public byte B { get; set; } = 0;

    /// <summary>
    /// Alpha component of the color.
    /// </summary>
    public byte A { get; set; } = 0;
    
    /// <summary>
    /// Pure black.
    /// </summary>
    public static Color Black = new()
    {
        R = 0,
        G = 0,
        B = 0,
        A = 255,
    };
    
    /// <summary>
    /// Pure white.
    /// </summary>
    public static Color White = new()
    {
        R = 255,
        G = 255,
        B = 255,
        A = 255,
    };
    
    /// <summary>
    /// Full transparency.
    /// </summary>
    public static Color Transparent = new()
    {
        R = 0,
        G = 0,
        B = 0,
        A = 0,
    };
    
    /// <summary>
    /// Pure red.
    /// </summary>
    public static Color Red = new()
    {
        R = 255,
        G = 0,
        B = 0,
        A = 255,
    };
    
    /// <summary>
    /// Pure green.
    /// </summary>
    public static Color Green = new()
    {
        R = 0,
        G = 255,
        B = 0,
        A = 255,
    };
    
    /// <summary>
    /// Pure blue.
    /// </summary>
    public static Color Blue = new()
    {
        R = 0,
        G = 0,
        B = 255,
        A = 255,
    };
}