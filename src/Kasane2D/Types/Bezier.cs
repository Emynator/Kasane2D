namespace Kasane2D.Types;

/// <summary>
/// Represents a beziér curve with a single control point.
/// </summary>
/// <param name="Start">The start point of the curve.</param>
/// <param name="End">The end point of the curve.</param>
/// <param name="Control">The location of the control point.</param>
public readonly record struct Bezier(Vec2F Start, Vec2F End, Vec2F Control)
{
    /// <summary>
    /// Calculates a linear interpolation along the curve by the factor t.
    /// </summary>
    /// <param name="t">The factor to interpolate by.</param>
    /// <returns>The resulting point along the curve.</returns>
    /// <remarks>t should be between 0.0f and 1.0f.</remarks>
    public Vec2F Interpolate(float t)
    {
        var a = Start.Lerp(Control, t);
        var b = Control.Lerp(End, t);

        return a.Lerp(b, t);
    }
}