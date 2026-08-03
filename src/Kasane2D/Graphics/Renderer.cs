using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics;

internal class Renderer : IRenderer
{
    private readonly IRasterizer rasterizer;
    private readonly Vec2I defaultTileSize;
    private readonly Vec2I defaultTilemapDimensions;
    private readonly Vec2I defaultSurfaceSize;
    private readonly Vec2I defaultSpriteSize;
    private readonly int defaultSpriteCount;
    private readonly Dictionary<string, ISurface> surfaces = new();
    private readonly Dictionary<string, ISpriteLayer> spriteLayers = new();
    private readonly Dictionary<string, ISlotManager> slotManagers = new();
    private bool initialized = false;

    public Renderer(GraphicsConfiguration config, IRasterizer rasterizer)
    {
        defaultTileSize = config.DefaultTileSize;
        defaultTilemapDimensions = config.DefaultTilemapDimensions;
        defaultSurfaceSize = config.DefaultSurfaceSize;
        defaultSpriteSize = config.DefaultSpriteSize;
        defaultSpriteCount = config.DefaultSpriteCount;
        this.rasterizer = rasterizer;
    }

    public ITextureManager TextureManager => rasterizer.TextureManager;

    public void Init(ICollection<RenderLayerConfig> renderLayerConfigs)
    {
        if (initialized)
        {
            return;
        }

        foreach (var config in renderLayerConfigs)
        {
            if (!config.Verify())
            {
                throw new InvalidOperationException($"Config for '{config.Name}' is invalid.");
            }
        }

        var idc = new Queue<RenderLayerConfig>();
        foreach (var config in renderLayerConfigs.Where(l => l.Layer < 0))
        {
            idc.Enqueue(config);
        }

        var configs = renderLayerConfigs
            .Where(l => l.Layer >= 0)
            .OrderBy(l => l.Layer);
        var lastLayer = 0;
        foreach (var config in configs)
        {
            if (config.Layer == lastLayer)
            {
                ConfigureLayer(config);
                continue;
            }

            while (lastLayer + 1 < config.Layer)
            {
                if (idc.TryDequeue(out var fill))
                {
                    ConfigureLayer(fill);
                }

                lastLayer++;
            }

            ConfigureLayer(config);
            lastLayer = config.Layer;
        }

        while (idc.TryDequeue(out var remaining))
        {
            ConfigureLayer(remaining);
        }

        initialized = true;
    }

    public T GetSurface<T>(string name) where T : ISurface
    {
        if (!surfaces.TryGetValue(name, out var surface) || surface is not T result)
        {
            throw new KeyNotFoundException($"Layer '{name}' not found.");
        }

        return result;
    }

    public ISpriteLayer GetSpriteLayer(string name)
    {
        return !spriteLayers.TryGetValue(name, out var layer)
            ? throw new KeyNotFoundException($"Layer '{name}' not found.")
            : layer;
    }

    public ISlotManager GetSlotManager(string layerName)
    {
        if (slotManagers.TryGetValue(layerName, out var slotManager))
        {
            return slotManager;
        }

        if (!spriteLayers.TryGetValue(layerName, out var layer))
        {
            throw new KeyNotFoundException($"Layer '{layerName}' not found.");
        }

        var result = new SlotManager(layer);
        slotManagers.Add(layerName, result);

        return result;
    }

    public void BeginDraw(ITextureSurface target)
    {
        rasterizer.BeginDraw(target);
    }

    public void EndDraw()
    {
        rasterizer.EndDraw();
    }

    public void Draw(ITexture src, Rect? dstReg = null, Rect? srcRect = null)
    {
        rasterizer.Draw(src, dstReg, srcRect);
    }

    public void Draw(ISurface src, Rect? dstReg = null, Rect? srcRect = null)
    {
        rasterizer.Draw(src, dstReg, srcRect);
    }

    public void Draw(Rect rect, Color color)
    {
        rasterizer.Draw(rect, color);
    }

    public void Draw(Line line, int thickness, Color color)
    {
        rasterizer.Draw(line, thickness, color);
    }

    public void Draw(Bezier bezier, int thickness, Color color, int precision = 5)
    {
        rasterizer.Draw(bezier, thickness, color, precision);
    }

    private void ConfigureLayer(RenderLayerConfig config)
    {
        switch (config.Type)
        {
            case LayerType.Tilemap:
                var tileSize = config.TileSize ?? defaultTileSize;
                var dimensions = config.Dimensions ?? defaultTilemapDimensions;
                surfaces.Add(config.Name, rasterizer.CreateTilemapSurface(tileSize, dimensions));

                break;

            case LayerType.Sprite:
                var spriteSize = config.SpriteSize ?? defaultSpriteSize;
                var spriteCount = config.SpriteCount ?? defaultSpriteCount;
                spriteLayers.Add(config.Name, rasterizer.CreateSpriteLayer(spriteSize, spriteCount));
                
                break;

            case LayerType.Texture:
                var size = config.Dimensions ?? defaultSurfaceSize;
                surfaces.Add(config.Name, rasterizer.CreateTextureSurface(size));
                
                break;
        }
    }
}