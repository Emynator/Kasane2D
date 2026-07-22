using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;

namespace Kasane2D.MonoGame;

public class Backend : IBackend
{
    private MonoGameRunner? runner;
    
    public IRasterizer CreateRasterizer(GraphicsConfiguration config)
    {
        runner ??= new();

        return new Rasterizer(config, runner.GraphicsDevice);
    }

    public IEngineRunner CreateRunner(EngineMain main)
    {
        runner?.Main = main;
        
        return runner ?? throw new InvalidOperationException("Init error?");
    }
}

public static class BuilderExtensions
{
    public static EngineBuilder UseMonoGame(this EngineBuilder builder)
    {
        builder.Backend ??= new Backend();
        
        return builder;
    }
}