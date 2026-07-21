using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;

namespace Kasane2D.Graphics.Interfaces;

public interface ISpriteLayer
{
    public Vec2I SpriteSize { get; }
    
    public Sprite[] Sprites { get; }
}