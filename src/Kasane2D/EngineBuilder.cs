using Kasane2D.Config;
using Kasane2D.Graphics;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Interfaces;

namespace Kasane2D;

public sealed class EngineBuilder
{
    public IBackend? Backend { get; set; }
    
    public IRasterizer? Rasterizer { get; set; }
    
    public IRenderer? Renderer { get; set; }
    
    public EngineMain? Main { get; set; }
}

public static class EngineBuilderExtensions
{
    public static EngineBuilder ConfigureGraphics(this EngineBuilder builder, GraphicsConfiguration config)
    {
        if (builder.Backend is null)
        {
            throw new InvalidOperationException("Backend not configured.");
        }

        builder.Rasterizer ??= builder.Backend.CreateRasterizer(config);
        
        return builder;
    }

    public static EngineBuilder ConfigureRenderer(this EngineBuilder builder, ICollection<RenderLayerConfig> config)
    {
        if (builder.Rasterizer is null)
        {
            throw new InvalidOperationException("Graphics not configured.");
        }

        builder.Renderer ??= new Renderer(builder.Rasterizer);

        return builder;
    }
    
    public static EngineBuilder WithMain<T>(this EngineBuilder builder) where T : EngineMain
    {
        builder.Main ??= Activator.CreateInstance<T>();
        
        return builder;
    }
        
    public static Engine Build(this EngineBuilder builder)
    {
        if (builder.Backend is null)
        {
            throw new InvalidOperationException("Backend not configured.");
        }

        if (builder.Main is null)
        {
            throw new InvalidOperationException("EngineMain not configured.");
        }
        
        builder.Main.Rasterizer = builder.Rasterizer;
        builder.Main.InternalRenderer = builder.Renderer;
        var runner = builder.Backend.CreateRunner(builder.Main);
        builder.Main.EngineRunner = runner;
        
        return new(runner);
    }
}