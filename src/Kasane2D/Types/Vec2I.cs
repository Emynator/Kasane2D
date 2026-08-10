using Kasane2D.Enums;

namespace Kasane2D.Types;

/// <summary>
/// Represents a 2-dimensional integer vector.
/// </summary>
public record struct Vec2I
{
    /// <summary>
    /// Creates a new vector.
    /// </summary>
    /// <param name="x">The X value.</param>
    /// <param name="y">The Y value.</param>
    public Vec2I(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    /// <summary>
    /// Zero vector.
    /// </summary>
    public static Vec2I Zero => new Vec2I(0, 0);
    
    /// <summary>
    /// Unit vector pointing up.
    /// </summary>
    public static Vec2I Up => new Vec2I(0, -1);
    
    /// <summary>
    /// Unit vector pointing down.
    /// </summary>
    public static Vec2I Down => new Vec2I(0, 1);
    
    /// <summary>
    /// Unit vector pointing left.
    /// </summary>
    public static Vec2I Left => new Vec2I(-1, 0);
    
    /// <summary>
    /// Unit vector pointing right.
    /// </summary>
    public static Vec2I Right => new Vec2I(1, 0);
    
    /// <summary>
    /// The X-value.
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// The Y-value.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Creates a copy of this vector.
    /// </summary>
    /// <returns>The created copy.</returns>
    public Vec2I Copy()
    {
        return new Vec2I(X, Y);
    }

    /// <summary>
    /// Converts this vector to a floating point vector.
    /// </summary>
    /// <returns>The resulting floating point vector.</returns>
    public Vec2F ToVec2F()
    {
        return new(X, Y);
    }

    /// <summary>
    /// Calculates the dot product of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The result of the dot product.</returns>
    public int Dot(Vec2I other)
    {
        return X * other.X + Y * other.Y;
    }

    /// <summary>
    /// Calculates the 2D cross product of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The result of the 2D cross product.</returns>
    public int Cross(Vec2I other)
    {
        return X * other.Y - Y * other.X;
    }

    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>The length of the vector rounded to nearest integer.</returns>
    public int Length()
    {
        return (int)Math.Round(Math.Sqrt(X * X + Y * Y));
    }

    /// <summary>
    /// Calculates the length of the vector squared.
    /// </summary>
    /// <returns>The length squared of the vector.</returns>
    public int LengthSquared()
    {
        return X * X + Y * Y;
    }

    /// <summary>
    /// Creates a unit vector pointing in the same direction as this vector.
    /// </summary>
    /// <returns>The resulting unit vector rounded to the nearest integer values.</returns>
    public Vec2I Normalized()
    {
        var result = Copy();
        result /= Length();
        
        return result;
    }

    /// <summary>
    /// Performs a component wise multiplication of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The resulting vector.</returns>
    /// <remarks>Component wise means result.X = this.X * other.X and result.Y = this.Y * other.Y.</remarks>
    public Vec2I CompWiseMul(Vec2I other)
    {
        var result = Copy();
        result.X *= other.X;
        result.Y *= other.Y;
        
        return result;
    }

    /// <summary>
    /// Performs a component wise division of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The resulting vector.</returns>
    /// <remarks>Component wise means result.X = this.X / other.X and result.Y = this.Y / other.Y.</remarks>
    public Vec2I CompWiseDiv(Vec2I other)
    {
        var result = Copy();
        result.X /= other.X;
        result.Y /= other.Y;
        
        return result;
    }

    /// <summary>
    /// Performs a linear interpolation between this and other by t.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <param name="t">The t value.</param>
    /// <returns>The resulting vector rounded to nearest integer values.</returns>
    /// <remarks>t should be between 0.0f and 1.0f.</remarks>
    public Vec2I Lerp(Vec2I other, float t)
    {
        return ToVec2F().Lerp(other.ToVec2F(), t).ToVec2I(RoundingMode.Nearest);
    }

    public void operator += (Vec2I rhs)
    {
        X += rhs.X;
        Y += rhs.Y;
    }

    public void operator += (int rhs)
    {
        X += rhs;
        Y += rhs;
    }

    public void operator -= (Vec2I rhs)
    {
        X -= rhs.X;
        Y -= rhs.Y;
    }

    public void operator -= (int rhs)
    {
        X -= rhs;
        Y -= rhs;
    }

    public void operator *= (int rhs)
    {
        X *= rhs;
        Y *= rhs;
    }
    
    public void operator /= (int rhs)
    {
        X /= rhs;
        Y /= rhs;
    }

    public static Vec2I operator +(Vec2I lhs, Vec2I rhs)
    {
        var result = lhs.Copy();
        result += rhs;

        return result;
    }

    public static Vec2I operator +(Vec2I lhs, int rhs)
    {
        var result = lhs.Copy();
        result += rhs;

        return result;
    }
    
    public static Vec2I operator -(Vec2I lhs, Vec2I rhs)
    {
        var result = lhs.Copy();
        result -= rhs;

        return result;
    }
    
    public static Vec2I operator -(Vec2I lhs, int rhs)
    {
        var result = lhs.Copy();
        result -= rhs;

        return result;
    }
    
    public static Vec2I operator *(Vec2I lhs, int rhs)
    {
        var result = lhs.Copy();
        result *= rhs;

        return result;
    }
    
    public static Vec2I operator /(Vec2I lhs, int rhs)
    {
        var result = lhs.Copy();
        result /= rhs;

        return result;
    }

    public static bool operator <(Vec2I lhs, Vec2I rhs)
    {
        return lhs.X < rhs.X && lhs.Y < rhs.Y;
    }

    public static bool operator >(Vec2I lhs, Vec2I rhs)
    {
        return lhs.X > rhs.X && lhs.Y > rhs.Y;
    }
    
    public static bool operator <= (Vec2I lhs, Vec2I rhs)
    {
        return lhs.X <= rhs.X && lhs.Y <= rhs.Y;
    }

    public static bool operator >=(Vec2I lhs, Vec2I rhs)
    {
        return lhs.X >= rhs.X && lhs.Y >= rhs.Y;
    }
}