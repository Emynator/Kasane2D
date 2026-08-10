namespace Kasane2D.Types;

/// <summary>
/// Represents a finite line.
/// </summary>
/// <param name="Start">The start point of the line.</param>
/// <param name="End">The end point of the line.</param>
public readonly record struct Line(Vec2F Start, Vec2F End)
{
    /// <summary>
    /// Checks if the line intersects with another line.
    /// </summary>
    /// <param name="other">The other line to check.</param>
    /// <returns>True if the lines intersect, false if not.</returns>
    public bool Intersects(Line other)
    {
        var direction = Start - End;
        var otherDirection = other.Start - other.End;
        var div = direction.Cross(otherDirection);
        var diff = Start - other.Start;
        if (MathF.Abs(div) < 1e-6)
        {
            return MathF.Abs(direction.Cross(diff)) < 1e-6;
        }
        
        var s = direction.Cross(diff);
        var t = otherDirection.Cross(diff);

        return div > 0.0f
            ? s >= 0.0f && s <= div && t >= 0.0f && t <= div
            : s <= 0.0f && s >= div && t <= 0.0f && t >= div;
    }
}