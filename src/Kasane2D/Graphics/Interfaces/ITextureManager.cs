using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ITextureManager
{
    public ITexture CreateTexture(Vec2I size);
    
    public ITexture CreateTexture(string filePath);
    
    public void FreeTexture(ITexture texture);
    
    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize);
    
    public ISpriteAtlas CreateSpriteAtlas(Vec2I spriteSize, string filePath);
    
    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize, string filePath);
    
    public void FreeSpriteAtlas(ISpriteAtlas atlas);
}