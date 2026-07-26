using System.Numerics;

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

    public Vector2 ToVector2()
    {
        return new(X, Y);
    }

    public Vec2I ToVec2I()
    {
        var res = Copy();
        res.Floor();
        
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

    public Vec2F Normalized()
    {
        var result = Copy();
        result /= Length();
        
        return result;
    }

    public void Floor()
    {
        X = MathF.Floor(X);
        Y = MathF.Floor(Y);
    }

    public void Ceil()
    {
        X = MathF.Ceiling(X);
        Y = MathF.Ceiling(Y);
    }

    public void Round()
    {
        X = MathF.Round(X);
        Y = MathF.Round(Y);
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