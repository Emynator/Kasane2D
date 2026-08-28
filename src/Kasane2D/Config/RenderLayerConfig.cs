using Kasane2D.Types;

namespace Kasane2D.Config;

/// <summary>
/// Type of layer configuration.
/// </summary>
public enum LayerType
{
    /// <summary>
    /// Tilemap layer that renders a tilemap.
    /// </summary>
    Tilemap,

    /// <summary>
    /// Sprite layer that renders sprites.
    /// </summary>
    Sprite,

    /// <summary>
    /// Texture layer intended for custom free-form rendering.
    /// </summary>
    Texture,

    /// <summary>
    /// Overlay layer that has a child surface that can be moved around the screen.
    /// </summary>
    Overlay,
}

/// <summary>
/// Configuration of a single rendering layer.
/// </summary>
public class RenderLayerConfig
{
    /// <summary>
    /// Name of the layer.
    /// </summary>
    /// <remarks>The name is used to later retrieve the layer from user code.</remarks>
    public required string Name { get; init; }

    /// <summary>
    /// Type of the layer.
    /// </summary>
    public required LayerType Type { get; init; }

    /// <summary>
    /// Sort order for the layer.
    /// </summary>
    /// <remarks>If no sort order is specified in the layer configuration, all layers are sorted in a first come, first
    /// served manner. This means the first layer in the configuration is the bottom most one, the second layer is the
    /// one right above that, and so on. If a sort order is provided, the renderer tries to stick to the requested
    /// order as best as possible with 0 being the lowest layer and higher numbers being higher layers. All layers with
    /// a layer number smaller than 0 are inserted into empty spaces in order. E.g. if a layer 0 and a layer 2 exist,
    /// the third layer with -1 is inserted as layer 1. If multiple layers share the same sort order, they are once
    /// again sorted in first come, first served manner. Meaning if two layers are configured as layer 1, the first
    /// actually becomes layer 1 and the second one becomes layer 2.</remarks>
    public int SortingOrder { get; init; } = -1;

    /// <summary>
    /// Optional: width and height in pixels of the tiles in this layer. Uses global default value if not provided.
    /// <seealso cref="GraphicsConfiguration.DefaultTileSize"/>
    /// </summary>
    /// <remarks>Value is ignored if layer has no tile sizes to configure.</remarks>
    public Vec2I? TileSize { get; init; }

    /// <summary>
    /// Optional: width and height in pixels of the sprites in this layer. Uses global default value if not provided.
    /// <seealso cref="GraphicsConfiguration.DefaultSpriteSize"/>
    /// </summary>
    /// <remarks>Value is ignored if layer has no tile sizes to configure.</remarks>
    public Vec2I? SpriteSize { get; init; }

    /// <summary>
    /// Optional: Dimensions of this layer. Uses global default value if not provided.
    /// <seealso cref="GraphicsConfiguration.DefaultTilemapDimensions"/>
    /// <seealso cref="GraphicsConfiguration.DefaultSurfaceSize"/>
    /// </summary>
    /// <remarks>In tilemap surfaces, the dimensions refers to the number of tile rows and columns. In texture
    /// surfaces, dimensions refers to the width and height of the surface in pixels.</remarks>
    public Vec2I? Dimensions { get; init; }

    /// <summary>
    /// Optional: number of the sprites in this layer. Uses global default value if not provided.
    /// <seealso cref="GraphicsConfiguration.DefaultSpriteCount"/>
    /// </summary>
    /// <remarks>Value is ignored if layer has no sprite count to configure.</remarks>
    public int? SpriteCount { get; init; }

    /// <summary>
    /// Optional: configuration of the child layer.
    /// </summary>
    /// <remarks>Value is ignored if layer has no child to configure.</remarks>
    public RenderLayerConfig? ChildLayer { get; init; }

    internal bool Verify()
    {
        return Type switch
        {
            LayerType.Tilemap => true,
            LayerType.Sprite => true,
            LayerType.Texture => true,
            LayerType.Overlay => ChildLayer is not null
                && ChildLayer.Verify()
                && ChildLayer.Type is not (LayerType.Sprite or LayerType.Overlay),
            _ => false,
        };
    }
}