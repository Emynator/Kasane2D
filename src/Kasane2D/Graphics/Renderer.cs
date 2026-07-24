using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.Graphics;

internal class Renderer : IRenderer
{
    private readonly IRasterizer rasterizer;
    private readonly Dictionary<string, ISurface> surfaces = new();
    private readonly Dictionary<string, ISpriteLayer> spriteLayers = new();
    private bool initialized = false;

    public Renderer(IRasterizer rasterizer)
    {
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

    private void ConfigureLayer(RenderLayerConfig config)
    {
        switch (config.Type)
        {
            case LayerType.Tilemap:
                if (config.TileSize is not null && config.Dimensions is not null)
                {
                    surfaces.Add
                    (
                        config.Name,
                        rasterizer.CreateTilemapSurface(config.TileSize.Value, config.Dimensions.Value)
                    );
                }
                else if (config.TileSize is not null)
                {
                    surfaces.Add(config.Name, rasterizer.CreateTilemapSurface(config.TileSize.Value));
                }
                else
                {
                    surfaces.Add(config.Name, rasterizer.CreateTilemapSurface());
                }
                break;

            case LayerType.Sprite:
                if (config.SpriteSize is not null)
                {
                    spriteLayers.Add
                    (
                        config.Name,
                        rasterizer.CreateSpriteLayer(config.SpriteSize.Value, config.SpriteCount!.Value)
                    );
                }
                else
                {
                    spriteLayers.Add(config.Name, rasterizer.CreateSpriteLayer(config.SpriteCount!.Value));
                }
                break;
        }
    }
}