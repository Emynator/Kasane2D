using Kasane2D.Graphics.Primitives;
using Kasane2D.Primitives;

namespace Kasane2D.Graphics.Interfaces;

public interface IRasterizer
{
    public ITextureManager TextureManager { get; }

    public ISurface CreateSurface();

    public ITilemapSurface CreateTilemapSurface();

    public ISpriteLayer CreateSpriteLayer(int spriteCount);
    
    public ISpriteLayer CreateSpriteLayer(Vec2I spriteSize, int spriteCount);

    public void Rasterize();
}