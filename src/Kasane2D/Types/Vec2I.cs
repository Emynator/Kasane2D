namespace Kasane2D.Types;

public record struct Vec2I
{
    public Vec2I(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Vec2I Zero => new Vec2I(0, 0);
    
    public static Vec2I Up => new Vec2I(0, -1);
    
    public static Vec2I Down => new Vec2I(0, 1);
    
    public static Vec2I Left => new Vec2I(-1, 0);
    
    public static Vec2I Right => new Vec2I(1, 0);
    
    public int X { get; set; }
    
    public int Y { get; set; }

    public Vec2I Copy()
    {
        return new Vec2I(X, Y);
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

    public Vec2I Lerp(Vec2I other, float t)
    {
        return ToVec2F().Lerp(other.ToVec2F(), t).ToVec2I();
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