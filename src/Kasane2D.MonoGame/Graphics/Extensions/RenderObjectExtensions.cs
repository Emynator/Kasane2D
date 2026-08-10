using Kasane2D.Exceptions.Backend;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.RenderObjects;

namespace Kasane2D.MonoGame.Graphics.Extensions;

internal static class RenderObjectExtensions
{
    public static MonoGameTexture AsTexture(this ITexture texture)
    {
        return texture as MonoGameTexture ?? throw new IncompatibleBackendDataException(nameof(ITexture));
    }

    public static MonoGameSurface AsSurface(this ISurface surface)
    {
        return surface as MonoGameSurface ?? throw new IncompatibleBackendDataException(nameof(ISurface));
    }

    public static SpriteAtlas AsAtlas(this ISpriteAtlas atlas)
    {
        return atlas as SpriteAtlas ?? throw new IncompatibleBackendDataException(nameof(ISpriteAtlas));
    }
}