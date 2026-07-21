using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class TilemapSurface : MonoGameSurface, ITilemapSurface
{
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly RenderTarget2D viewportSurface;
    private readonly RenderTarget2D surface;
    private readonly Tile[,] tiles;
    private List<Vec2I> tilesToUpdate = [];
    private bool atlasChanged = false;

    public TilemapSurface
        (
        GraphicsDevice device,
        SpriteBatch spriteBatch,
        Vec2I dimensions,
        Vec2I tileSize,
        Vec2I viewportSize
        ) : base(new(dimensions.X * tileSize.X, dimensions.Y * tileSize.Y), viewportSize)
    {
        this.device = device;
        this.spriteBatch = spriteBatch;
        Dimensions = dimensions;
        TileSize = tileSize;

        viewportSurface = new RenderTarget2D(device, viewportSize.X, viewportSize.Y);
        surface = new RenderTarget2D(device, dimensions.X * tileSize.X, dimensions.Y * tileSize.Y);
        tiles = new Tile[dimensions.X, dimensions.Y];

        for (var x = 0; x < dimensions.X; x++)
        {
            for (var y = 0; y < dimensions.Y; y++)
            {
                tiles[x, y] = new();
            }
        }
    }

    public Vec2I Dimensions { get; }

    public Vec2I TileSize { get; }

    public SpriteAtlas? Atlas
    {
        get;
        set
        {
            if (value is null)
            {
                field = null;
                return;
            }

            if (value.SpriteSize != TileSize)
            {
                throw new ArgumentException("Incompatible tile size!");
            }

            field = value;
        }
    }

    public ISpriteAtlas? TileAtlas
    {
        get => Atlas;
        set
        {
            atlasChanged = true;
            
            if (value is null)
            {
                Atlas = null;
                return;
            }

            if (value is not SpriteAtlas atlas)
            {
                throw new ArgumentException("Incompatible atlas!");
            }

            Atlas = atlas;
        }
    }

    public override void Dispose()
    {
        viewportSurface.Dispose();
        
        base.Dispose();
    }

    public void UpdateAtlasIndex(Vec2I tilePosition, Vec2I value)
    {
        tiles[tilePosition.X, tilePosition.Y].AtlasIndex = value;
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateHFlip(Vec2I tilePosition, bool value)
    {
        tiles[tilePosition.X, tilePosition.Y].HFlip = value;
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateVFlip(Vec2I tilePosition, bool value)
    {
        tiles[tilePosition.X, tilePosition.Y].VFlip = value;
        tilesToUpdate.Add(tilePosition);
    }

    public override Texture2D GetSurface()
    {
        return viewportSurface;
    }

    public override void Rasterize()
    {
        UpdateSurface();
        
        device.SetRenderTarget(viewportSurface);
        device.Clear(Color.Transparent);
        
        spriteBatch.Begin(samplerState: SamplerState.PointWrap);
        spriteBatch.Draw(surface, Viewport.ViewRect.ToRectangle(), Color.White);
        spriteBatch.End();
    }

    private void UpdateSurface()
    {
        if (atlasChanged)
        {
            RenderSurface();
            tilesToUpdate = [];

            return;
        }
        
        if (tilesToUpdate.Count == 0)
        {
            return;
        }

        if (Atlas is null)
        {
            return;
        }
        
        var updates = tilesToUpdate.Distinct().ToList();
        
        device.SetRenderTarget(surface);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        foreach (var update in updates)
        {
            var dst = new Rectangle(update.CompWiseMul(TileSize).ToPoint(), TileSize.ToPoint());
            var src = Atlas.GetSrcRect(tiles[update.X, update.Y].AtlasIndex);
            
            spriteBatch.Draw(Atlas.MonoGameTexture.Texture, dst, src, Color.White);
        }
        
        spriteBatch.End();
        tilesToUpdate = [];
    }

    private void RenderSurface()
    {
        device.SetRenderTarget(surface);

        if (Atlas is null)
        {
            device.Clear(Color.Transparent);
            
            return;
        }
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        for (var x = 0; x < Dimensions.X; x++)
        {
            for (var y = 0; y < Dimensions.Y; y++)
            {
                var loc = new Vec2I(x, y).CompWiseMul(TileSize);
                var dst = new Rectangle(loc.ToPoint(), TileSize.ToPoint());
                var src = Atlas.GetSrcRect(tiles[x, y].AtlasIndex);
                
                spriteBatch.Draw(Atlas.MonoGameTexture.Texture, dst, src, Color.White);
            }
        }
        
        spriteBatch.End();
    }
}