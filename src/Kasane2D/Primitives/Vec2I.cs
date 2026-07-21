using System.Numerics;

namespace Kasane2D.Primitives;

public record struct Vec2I
{
    public Vec2I(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Vec2I Zero => new Vec2I(0, 0);
    
    public int X { get; set; }
    
    public int Y { get; set; }

    public Vec2I Copy()
    {
        return new Vec2I(X, Y);
    }

    public Vector2 ToVector2()
    {
        return new(X, Y);
    }

    public Vec2F ToVec2F()
    {
        return new(X, Y);
    }

    public int Dot(Vec2I other)
    {
        return X * other.X + Y * other.Y;
    }

    public int Cross(Vec2I other)
    {
        return X * other.Y - Y * other.X;
    }

    public int Length()
    {
        return (int)Math.Round(Math.Sqrt(X * X + Y * Y));
    }

    public Vec2I Normalized()
    {
        var result = Copy();
        result /= Length();
        
        return result;
    }

    public Vec2I CompWiseMul(Vec2I other)
    {
        var result = Copy();
        result.X *= other.X;
        result.Y *= other.Y;
        
        return result;
    }

    public Vec2I CompWiseDiv(Vec2I other)
    {
        var result = Copy();
        result.X /= other.X;
        result.Y /= other.Y;
        
        return result;
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