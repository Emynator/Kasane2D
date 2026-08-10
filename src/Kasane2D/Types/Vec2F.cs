using Kasane2D.Enums;

namespace Kasane2D.Types;

/// <summary>
/// Represents a 2-dimensional floating point vector.
/// </summary>
public record struct Vec2F
{
    /// <summary>
    /// Creates a new vector.
    /// </summary>
    /// <param name="x">The X value.</param>
    /// <param name="y">The Y value.</param>
    public Vec2F(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    /// <summary>
    /// Zero vector.
    /// </summary>
    public static Vec2F Zero => new Vec2F(0.0f, 0.0f);
    
    /// <summary>
    /// Unit vector pointing up.
    /// </summary>
    public static Vec2F Up => new Vec2F(0.0f, -1.0f);
    
    /// <summary>
    /// Unit vector pointing down.
    /// </summary>
    public static Vec2F Down => new Vec2F(0.0f, 1.0f);
    
    /// <summary>
    /// Unit vector pointing left.
    /// </summary>
    public static Vec2F Left => new Vec2F(-1.0f, 0.0f);
    
    /// <summary>
    /// Unit vector pointing right.
    /// </summary>
    public static Vec2F Right => new Vec2F(1.0f, 0.0f);
    
    /// <summary>
    /// The X-value.
    /// </summary>
    public float X { get; set; }
    
    /// <summary>
    /// The Y-value.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Creates a copy of this vector.
    /// </summary>
    /// <returns>The created copy.</returns>
    public Vec2F Copy()
    {
        return new Vec2F(X, Y);
    }

    /// <summary>
    /// Converts this vector to an integer vector using the provided rounded mode.
    /// </summary>
    /// <param name="mode">Optional: the rounding mode to use. Default is <see cref="RoundingMode.Floor"/>.</param>
    /// <returns>The resulting integer vector.</returns>
    public Vec2I ToVec2I(RoundingMode mode = RoundingMode.Floor)
    {
        var res = mode switch
        {
            RoundingMode.Floor => Floor(),
            RoundingMode.Nearest => Round(),
            RoundingMode.Ceil => Ceil(),
            _ => Floor(),
        };
        
        return new Vec2I((int)res.X, (int)res.Y);
    }

    /// <summary>
    /// Calculates the dot product of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The result of the dot product.</returns>
    public float Dot(Vec2F other)
    {
        return X * other.X + Y * other.Y;
    }

    /// <summary>
    /// Calculates the 2D cross product of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The result of the 2D cross product.</returns>
    public float Cross(Vec2F other)
    {
        return X * other.Y - Y * other.X;
    }

    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    /// Calculates the length of the vector squared.
    /// </summary>
    /// <returns>The length squared of the vector.</returns>
    public float LengthSquared()
    {
        return X * X + Y * Y;
    }

    /// <summary>
    /// Creates a unit vector pointing in the same direction as this vector.
    /// </summary>
    /// <returns>The resulting unit vector.</returns>
    public Vec2F Normalized()
    {
        var result = Copy();
        result /= Length();
        
        return result;
    }

    /// <summary>
    /// Creates a vector with the values of this vector rounded down.
    /// </summary>
    /// <returns>The resulting vector.</returns>
    public Vec2F Floor()
    {
        var result = Copy();
        result.X = MathF.Floor(X);
        result.Y = MathF.Floor(Y);

        return result;
    }

    /// <summary>
    /// Creates a vector with the values of this vector rounded up.
    /// </summary>
    /// <returns>The resulting vector.</returns>
    public Vec2F Ceil()
    {
        var result = Copy();
        result.X = MathF.Ceiling(X);
        result.Y = MathF.Ceiling(Y);
        
        return result;
    }

    /// <summary>
    /// Creates a vector with the values of this vector rounded up or down in regard to mid-point rounding.
    /// </summary>
    /// <returns>The resulting vector.</returns>
    public Vec2F Round()
    {
        var result = Copy();
        result.X = MathF.Round(X);
        result.Y = MathF.Round(Y);
        
        return result;
    }
    
    /// <summary>
    /// Performs a component wise multiplication of this vector and other.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The resulting vector.</returns>
    /// <remarks>Component wise means result.X = this.X * other.X and result.Y = this.Y * other.Y.</remarks>
    public Vec2F CompWiseMul(Vec2F other)
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
    public Vec2F CompWiseDiv(Vec2F other)
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
    /// <returns>The resulting vector.</returns>
    /// <remarks>t should be between 0.0f and 1.0f.</remarks>
    public Vec2F Lerp(Vec2F other, float t)
    {
        return new Vec2F(float.Lerp(X, other.X, t), float.Lerp(Y, other.Y, t));
    }

    public void operator += (Vec2F rhs)
    {
        X += rhs.X;
        Y += rhs.Y;
    }

    public void operator += (float rhs)
    {
        X += rhs;
        Y += rhs;
    }

    public void operator -= (Vec2F rhs)
    {
        X -= rhs.X;
        Y -= rhs.Y;
    }

    public void operator -= (float rhs)
    {
        X -= rhs;
        Y -= rhs;
    }

    public void operator *= (float rhs)
    {
        X *= rhs;
        Y *= rhs;
    }
    
    public void operator /= (float rhs)
    {
        X /= rhs;
        Y /= rhs;
    }

    public static Vec2F operator +(Vec2F lhs, Vec2F rhs)
    {
        var result = lhs.Copy();
        result += rhs;

        return result;
    }

    public static Vec2F operator +(Vec2F lhs, float rhs)
    {
        var result = lhs.Copy();
        result += rhs;

        return result;
    }
    
    public static Vec2F operator -(Vec2F lhs, Vec2F rhs)
    {
        var result = lhs.Copy();
        result -= rhs;

        return result;
    }
    
    public static Vec2F operator -(Vec2F lhs, float rhs)
    {
        var result = lhs.Copy();
        result -= rhs;

        return result;
    }
    
    public static Vec2F operator *(Vec2F lhs, float rhs)
    {
        var result = lhs.Copy();
        result *= rhs;

        return result;
    }
    
    public static Vec2F operator /(Vec2F lhs, float rhs)
    {
        var result = lhs.Copy();
        result /= rhs;

        return result;
    }

    public static bool operator <(Vec2F lhs, Vec2F rhs)
    {
        return lhs.X < rhs.X && lhs.Y < rhs.Y;
    }

    public static bool operator >(Vec2F lhs, Vec2F rhs)
    {
        return lhs.X > rhs.X && lhs.Y > rhs.Y;
    }
    
    public static bool operator <= (Vec2F lhs, Vec2F rhs)
    {
        return lhs.X <= rhs.X && lhs.Y <= rhs.Y;
    }

    public static bool operator >=(Vec2F lhs, Vec2F rhs)
    {
        return lhs.X >= rhs.X && lhs.Y >= rhs.Y;
    }
}