using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ITilemapSurface : ISurface
{
    public Vec2I Dimensions { get; }
    
    public Vec2I TileSize { get; }

    public ISpriteAtlas? TileAtlas { get; set; }

    public void UpdateAtlasIndex(Vec2I tilePosition, Vec2I value);

    public void UpdateHFlip(Vec2I tilePosition, bool value);
    
    public void UpdateVFlip(Vec2I tilePosition, bool value);
}