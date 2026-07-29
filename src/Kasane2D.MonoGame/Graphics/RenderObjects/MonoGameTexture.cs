using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework.Graphics;
using MgColor = Microsoft.Xna.Framework.Color;
using KasaneColor = Kasane2D.Graphics.Types.Color;

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

    public KasaneColor[,] GetData()
    {
        var data = new MgColor[Texture.Width * Texture.Height];
        Texture.GetData(data);

        var result = new KasaneColor[Texture.Width, Texture.Height];
        var i = 0;
        for (var y = 0; y < Texture.Height; y++)
        {
            for (var x = 0; x < Texture.Width; x++)
            {
                result[x, y] = data[i].ToKasaneColor();
                i++;
            }
            i++;
        }

        return result;
    }

    public void SetData(KasaneColor[,] data)
    {
        var value = new MgColor[Texture.Width * Texture.Height];
        if (data.Length != value.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(data));
        }
        
        for (var y = 0; y < Texture.Height; y++)
        {
            for (var x = 0; x < Texture.Width; x++)
            {
                value[y * Texture.Width + x] = data[x, y].ToMgColor();
            }
        }
        
        Texture.SetData(value);
    }
}