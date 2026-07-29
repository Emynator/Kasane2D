using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.RenderObjects;

namespace Kasane2D.MonoGame.Graphics.Extensions;

internal static class RenderObjectExtensions
{
    public static MonoGameTexture AsTexture(this ITexture texture)
    {
        return texture as MonoGameTexture ?? throw new InvalidOperationException();
    }

    public static MonoGameSurface AsSurface(this ISurface surface)
    {
        return surface as MonoGameSurface ?? throw new InvalidOperationException();
    }

    public static SpriteAtlas AsAtlas(this ISpriteAtlas atlas)
    {
        return atlas as SpriteAtlas ?? throw new InvalidOperationException();
    }
}