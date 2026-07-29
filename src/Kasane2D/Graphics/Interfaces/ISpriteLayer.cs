using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ISpriteLayer
{
    public Vec2I SpriteSize { get; }
    
    public RenderSprite[] Sprites { get; }
}