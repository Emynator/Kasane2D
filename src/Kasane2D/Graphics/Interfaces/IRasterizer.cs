using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface IRasterizer
{
    public ITextureManager TextureManager { get; }

    public ISurface CreateSurface();
    
    public ISurface CreateSurface(Vec2I dimensions);

    public ITilemapSurface CreateTilemapSurface();
    
    public ITilemapSurface CreateTilemapSurface(Vec2I tileSize);
    
    public ITilemapSurface CreateTilemapSurface(Vec2I tileSize, Vec2I dimensions);
    
    public ITextureSurface CreateTextureSurface();
    
    public ITextureSurface CreateTextureSurface(Vec2I dimensions);

    public ISpriteLayer CreateSpriteLayer(int spriteCount);
    
    public ISpriteLayer CreateSpriteLayer(Vec2I spriteSize, int spriteCount);

    public void Rasterize();
    
    public void BeginDraw(ITextureSurface target);

    public void EndDraw();

    public void Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null);
    
    public void Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null);

    public void Draw(Rect rect, Color color);
    
    public void Draw(Line line, int thickness, Color color);

    public void Draw(Bezier bezier, int thickness, Color color, int precision);
}