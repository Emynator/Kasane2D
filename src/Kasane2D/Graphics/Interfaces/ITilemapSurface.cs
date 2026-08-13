using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// A render layer that renders a tilemap to the screen.
/// </summary>
public interface ITilemapSurface : ISurface
{
    /// <summary>
    /// The number of rows and columns of tiles.
    /// </summary>
    public Vec2I Dimensions { get; }
    
    /// <summary>
    /// Size of a single tile in pixels.
    /// </summary>
    public Vec2I TileSize { get; }

    /// <summary>
    /// The atlas used by this tilemap.
    /// </summary>
    public ISpriteAtlas? TileAtlas { get; set; }

    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="tilePosition">Coordinates of the tile.</param>
    /// <param name="value">New atlas index for the tile.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateAtlasIndex(Vec2I tilePosition, Vec2I value);
    
    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="tilePosition">Coordinates of the tile.</param>
    /// <param name="atlasX">Row index of the atlas.</param>
    /// <param name="atlasY">Column index of the atlas.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateAtlasIndex(Vec2I tilePosition, int atlasX, int atlasY);
    
    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="tilePosition">Coordinates of the given tile.</param>
    /// <param name="value">New atlas index for the tile.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    /// <remarks>This function is used to set indices by a single, scalar value instead of x/y offsets in the tilemap.
    /// The scalar indexes the atlas from left to right, top to bottom.</remarks>
    public void UpdateAtlasIndex(Vec2I tilePosition, int value);

    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="positionX">X-coordinate of the tile.</param>
    /// <param name="positionY">Y-coordinate of the tile.</param>
    /// <param name="value">New atlas index for the tile.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateAtlasIndex(int positionX, int positionY, Vec2I value);
    
    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="positionX">X-coordinate of the tile.</param>
    /// <param name="positionY">Y-coordinate of the tile.</param>
    /// <param name="atlasX">Row index of the atlas.</param>
    /// <param name="atlasY">Column index of the atlas.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateAtlasIndex(int positionX, int positionY, int atlasX, int atlasY);
    
    /// <summary>
    /// Changes the atlas index of the given tile.
    /// </summary>
    /// <param name="positionX">X-coordinate of the tile.</param>
    /// <param name="positionY">Y-coordinate of the tile.</param>
    /// <param name="value">New atlas index for the tile.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    /// <remarks>This function is used to set indices by a single, scalar value instead of x/y offsets in the tilemap.
    /// The scalar indexes the atlas from left to right, top to bottom.</remarks>
    public void UpdateAtlasIndex(int positionX, int positionY, int value);

    /// <summary>
    /// Change the horizontal flip property of the given tile.
    /// </summary>
    /// <param name="tilePosition">Coordinates of the given tile.</param>
    /// <param name="value">True if the tile image should be flipped horizontally when rendered.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateHFlip(Vec2I tilePosition, bool value);
    
    /// <summary>
    /// Change the horizontal flip property of the given tile.
    /// </summary>
    /// <param name="positionX">X-coordinate of the tile.</param>
    /// <param name="positionY">Y-coordinate of the tile.</param>
    /// <param name="value">True if the tile image should be flipped horizontally when rendered.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateHFlip(int positionX, int positionY, bool value);
    
    /// <summary>
    /// Change the vertical flip property of the given tile.
    /// </summary>
    /// <param name="tilePosition">Coordinates of the given tile.</param>
    /// <param name="value">True if the tile image should be flipped vertically when rendered.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateVFlip(Vec2I tilePosition, bool value);
    
    /// <summary>
    /// Change the vertical flip property of the given tile.
    /// </summary>
    /// <param name="positionX">X-coordinate of the tile.</param>
    /// <param name="positionY">Y-coordinate of the tile.</param>
    /// <param name="value">True if the tile image should be flipped vertically when rendered.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the tile coordinates are outside the tilemap's range.</exception>
    public void UpdateVFlip(int positionX, int positionY, bool value);
}