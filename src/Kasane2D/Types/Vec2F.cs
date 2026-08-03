using Kasane2D.Enums;

namespace Kasane2D.Types;

public record struct Vec2F
{
    public Vec2F(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public static Vec2F Zero => new Vec2F(0.0f, 0.0f);
    
    public static Vec2F Up => new Vec2F(0.0f, -1.0f);
    
    public static Vec2F Down => new Vec2F(0.0f, 1.0f);
    
    public static Vec2F Left => new Vec2F(-1.0f, 0.0f);
    
    public static Vec2F Right => new Vec2F(1.0f, 0.0f);
    
    public float X { get; set; }
    
    public float Y { get; set; }

    public Vec2F Copy()
    {
        return new Vec2F(X, Y);
    }

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

    public float Dot(Vec2F other)
    {
        return X * other.X + Y * other.Y;
    }

    public float Cross(Vec2F other)
    {
        return X * other.Y - Y * other.X;
    }

    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    public float LengthSquared()
    {
        return X * X + Y * Y;
    }

    public Vec2F Normalized()
    {
        var result = Copy();
        result /= Length();
        
        return result;
    }

    public Vec2F Floor()
    {
        var result = Copy();
        result.X = MathF.Floor(X);
        result.Y = MathF.Floor(Y);

        return result;
    }

    public Vec2F Ceil()
    {
        var result = Copy();
        result.X = MathF.Ceiling(X);
        result.Y = MathF.Ceiling(Y);
        
        return result;
    }

    public Vec2F Round()
    {
        var result = Copy();
        result.X = MathF.Round(X);
        result.Y = MathF.Round(Y);
        
        return result;
    }
    
    public Vec2F CompWiseMul(Vec2F other)
    {
        var result = Copy();
        result.X *= other.X;
        result.Y *= other.Y;
        
        return result;
    }

    public Vec2F CompWiseDiv(Vec2F other)
    {
        var result = Copy();
        result.X /= other.X;
        result.Y /= other.Y;
        
        return result;
    }

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