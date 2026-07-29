using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

public class SpriteSlot
{
    private readonly ISpriteLayer layer;

    internal SpriteSlot(ISpriteLayer layer, int index)
    {
        this.layer = layer;
        Index = index;
    }
    
    public bool Freed { get; internal set; } = false;

    public Vec2I Size => layer.SpriteSize;

    public Vec2I Position
    {
        get => layer.Sprites[Index].Position;
        set => layer.Sprites[Index].Position = value;
    }

    public Rect Rect => new(Position, Size);

    public ISpriteAtlas? SpriteAtlas
    {
        get => layer.Sprites[Index].SpriteAtlas;
        set => layer.Sprites[Index].SpriteAtlas = value;
    }

    public Vec2I AtlasIndex
    {
        get => layer.Sprites[Index].AtlasIndex;
        set => layer.Sprites[Index].AtlasIndex = value;
    }

    public bool IsActive
    {
        get => layer.Sprites[Index].IsActive;
        set => layer.Sprites[Index].IsActive = value;
    }

    public bool HFlip
    {
        get => layer.Sprites[Index].HFlip;
        set => layer.Sprites[Index].HFlip = value;
    }

    public bool VFlip
    {
        get => layer.Sprites[Index].VFlip;
        set => layer.Sprites[Index].VFlip = value;
    }
    
    internal int Index { get; }
}