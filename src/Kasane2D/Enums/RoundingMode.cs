namespace Kasane2D.Enums;

/// <summary>
/// Rounding mode for floating point vectors.
/// </summary>
public enum RoundingMode
{
    /// <summary>
    /// Rounds down to the next integer smaller than the value.
    /// </summary>
    Floor,
    /// <summary>
    /// Rounds to the nearest integer value according to the rules of mid-point rounding.
    /// </summary>
    Nearest,
    /// <summary>
    /// Rounds up to the next integer larger than the value.
    /// </summary>
    Ceil,
}