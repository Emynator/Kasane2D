using Kasane2D.Config;

namespace Kasane2D.Graphics.Interfaces;

public interface IRenderer
{
    public ITextureManager TextureManager { get; }
    
    public void Init(ICollection<RenderLayerConfig> renderLayerConfigs);

    public T GetSurface<T>(string name) where T : ISurface;
    
    public ISpriteLayer GetSpriteLayer(string name);
}