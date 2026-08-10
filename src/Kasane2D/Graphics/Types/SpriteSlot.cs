using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Types;

/// <summary>
/// Represents a sprite in an <see cref="ISpriteLayer"/>. It is a reference to a slot managed by an <see cref="ISlotManager"/>
/// </summary>
public class SpriteSlot
{
    private readonly ISpriteLayer layer;

    internal SpriteSlot(ISpriteLayer layer, int index)
    {
        this.layer = layer;
        Index = index;
    }
    
    /// <summary>
    /// True if the slot still available, false if it has been returned to the slot manager.
    /// </summary>
    public bool Freed { get; internal set; } = false;

    /// <summary>
    /// See <see cref="RenderSprite.Size"/>
    /// </summary>
    public Vec2I Size => layer.SpriteSize;

    /// <summary>
    /// See <see cref="RenderSprite.Position"/>
    /// </summary>
    public Vec2I Position
    {
        get => layer.Sprites[Index].Position;
        set => layer.Sprites[Index].Position = value;
    }

    /// <summary>
    /// See <see cref="RenderSprite.Rect"/>
    /// </summary>
    public Rect Rect => new(Position, Size);

    /// <summary>
    /// See <see cref="RenderSprite.SpriteAtlas"/>
    /// </summary>
    public ISpriteAtlas? SpriteAtlas
    {
        get => layer.Sprites[Index].SpriteAtlas;
        set => layer.Sprites[Index].SpriteAtlas = value;
    }

    /// <summary>
    /// See <see cref="RenderSprite.AtlasIndex"/>
    /// </summary>
    public Vec2I AtlasIndex
    {
        get => layer.Sprites[Index].AtlasIndex;
        set => layer.Sprites[Index].AtlasIndex = value;
    }

    /// <summary>
    /// See <see cref="RenderSprite.IsActive"/>
    /// </summary>
    public bool IsActive
    {
        get => layer.Sprites[Index].IsActive;
        set => layer.Sprites[Index].IsActive = value;
    }

    /// <summary>
    /// See <see cref="RenderSprite.HFlip"/>
    /// </summary>
    public bool HFlip
    {
        get => layer.Sprites[Index].HFlip;
        set => layer.Sprites[Index].HFlip = value;
    }

    /// <summary>
    /// See <see cref="RenderSprite.VFlip"/>
    /// </summary>
    public bool VFlip
    {
        get => layer.Sprites[Index].VFlip;
        set => layer.Sprites[Index].VFlip = value;
    }
    
    internal int Index { get; }
}