using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Graphics.Extensions;
using Kasane2D.Graphics.Types;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class TilemapSurface : MonoGameSurface, ITilemapSurface
{
    private readonly string systemKey;
    private readonly GraphicsDevice device;
    private readonly SpriteBatch spriteBatch;
    private readonly RenderTarget2D viewportSurface;
    private readonly RenderTarget2D surface;
    private readonly RenderTile[,] tiles;
    private List<Vec2I> tilesToUpdate = [];
    private bool atlasChanged = false;

    public TilemapSurface
        (
        string name,
        GraphicsDevice device,
        SpriteBatch spriteBatch,
        Vec2I dimensions,
        Vec2I tileSize,
        Vec2I viewportSize
        ) : base(new(dimensions.X * tileSize.X, dimensions.Y * tileSize.Y), viewportSize)
    {
        systemKey = $"Backend::GraphicsSystem::Surface::{name}::";
        this.device = device;
        this.spriteBatch = spriteBatch;
        Dimensions = dimensions;
        TileSize = tileSize;

        viewportSurface = new RenderTarget2D(device, viewportSize.X, viewportSize.Y);
        surface = new RenderTarget2D(device, dimensions.X * tileSize.X, dimensions.Y * tileSize.Y);
        tiles = new RenderTile[dimensions.X, dimensions.Y];

        for (var x = 0; x < dimensions.X; x++)
        {
            for (var y = 0; y < dimensions.Y; y++)
            {
                tiles[x, y] = new(tileSize);
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

    public void UpdateAtlasIndex(Vec2I tilePosition, int atlasX, int atlasY)
    {
        tiles[tilePosition.X, tilePosition.Y].AtlasIndex = new(atlasX, atlasY);
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateAtlasIndex(Vec2I tilePosition, int value)
    {
        if (Atlas is null)
        {
            throw new InvalidOperationException("Atlas is null!");
        }

        var x = value % Atlas.Dimensions.X;
        var y = value / Atlas.Dimensions.X;
        tiles[tilePosition.X, tilePosition.Y].AtlasIndex = new(x, y);
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateAtlasIndex(int positionX, int positionY, Vec2I value)
    {
        tiles[positionX, positionY].AtlasIndex = value;
        tilesToUpdate.Add(new(positionX, positionY));
    }

    public void UpdateAtlasIndex(int positionX, int positionY, int atlasX, int atlasY)
    {
        tiles[positionX, positionY].AtlasIndex = new(atlasX, atlasY);
        tilesToUpdate.Add(new(positionX, positionY));
    }

    public void UpdateAtlasIndex(int positionX, int positionY, int value)
    {
        if (Atlas is null)
        {
            throw new InvalidOperationException("Atlas is null!");
        }

        var x = value % Atlas.Dimensions.X;
        var y = value / Atlas.Dimensions.X;
        tiles[positionX, positionY].AtlasIndex = new(x, y);
        tilesToUpdate.Add(new(positionX, positionY));
    }

    public void UpdateHFlip(Vec2I tilePosition, bool value)
    {
        tiles[tilePosition.X, tilePosition.Y].HFlip = value;
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateHFlip(int positionX, int positionY, bool value)
    {
        tiles[positionX, positionY].HFlip = value;
        tilesToUpdate.Add(new(positionX, positionY));
    }

    public void UpdateVFlip(Vec2I tilePosition, bool value)
    {
        tiles[tilePosition.X, tilePosition.Y].VFlip = value;
        tilesToUpdate.Add(tilePosition);
    }

    public void UpdateVFlip(int positionX, int positionY, bool value)
    {
        tiles[positionX, positionY].VFlip = value;
        tilesToUpdate.Add(new(positionX, positionY));
    }

    public override Texture2D GetSurface()
    {
        return viewportSurface;
    }

    public override void Rasterize()
    {
        UpdateSurface();

        Engine.Monitor.StartMeasurement($"{systemKey}Rasterize");
        
        device.SetRenderTarget(viewportSurface);
        device.Clear(Color.Transparent);

        spriteBatch.Begin(samplerState: SamplerState.PointWrap);
        spriteBatch.Draw
        (
            surface,
            new Rectangle(new Point(0, 0), Viewport.Size.ToPoint()),
            Viewport.ViewRect.ToRectangle(),
            Color.White
        );
        spriteBatch.End();
        
        Engine.Monitor.FinishMeasurement($"{systemKey}Rasterize");
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
        
        Engine.Monitor.StartMeasurement($"{systemKey}UpdateSurface");

        var updates = tilesToUpdate.Distinct().ToList();

        device.SetRenderTarget(surface);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var update in updates)
        {
            var dst = new Rectangle(update.CompWiseMul(TileSize).ToPoint(), TileSize.ToPoint());
            var src = Atlas.GetSrcRect(tiles[update.X, update.Y].AtlasIndex);
            var effects = SpriteEffects.None;
            if (tiles[update.X, update.Y].HFlip)
            {
                effects |= SpriteEffects.FlipHorizontally;
            }
            if (tiles[update.X, update.Y].VFlip)
            {
                effects |= SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw
            (
                Atlas.MonoGameTexture.Texture,
                dst,
                src,
                Color.White,
                0.0f,
                Vector2.Zero,
                effects,
                1.0f
            );
        }

        spriteBatch.End();
        tilesToUpdate = [];
        
        Engine.Monitor.FinishMeasurement($"{systemKey}UpdateSurface");
    }

    private void RenderSurface()
    {
        Engine.Monitor.StartMeasurement($"{systemKey}RenderSurface");
        
        device.SetRenderTarget(surface);
        device.Clear(Color.Transparent);

        if (Atlas is null)
        {
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
                var effects = SpriteEffects.None;
                if (tiles[x, y].HFlip)
                {
                    effects |= SpriteEffects.FlipHorizontally;
                }
                if (tiles[x, y].VFlip)
                {
                    effects |= SpriteEffects.FlipVertically;
                }

                spriteBatch.Draw
                (
                    Atlas.MonoGameTexture.Texture,
                    dst,
                    src,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    effects,
                    1.0f
                );
            }
        }

        spriteBatch.End();
        
        Engine.Monitor.FinishMeasurement($"{systemKey}RenderSurface");
    }
}