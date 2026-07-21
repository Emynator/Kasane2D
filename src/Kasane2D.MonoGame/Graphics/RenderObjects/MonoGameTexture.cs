using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

public class MonoGameTexture : ITexture
{
    public MonoGameTexture(Texture2D texture)
    {
        Texture = texture;
        Size = new(texture.Width, texture.Height);
    }
    
    public Vec2I Size { get; }
    
    public Texture2D Texture { get; }
}