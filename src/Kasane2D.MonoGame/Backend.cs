using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;

namespace Kasane2D.MonoGame;

public class Backend : IBackend
{
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer
        )
    {
        return new MonoGameRunner(main, config, createRenderer);
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