using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;

namespace Kasane2D.MonoGame;

public class Backend : IBackend
{
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig
        )
    {
        return new MonoGameRunner(main, config, createRenderer, assignInputSystem, audioConfig);
    }

    public bool IsSampleRateSupported(int sampleRate)
    {
        return sampleRate is 44100 or 48000;
    }

    public int[] GetSupportedSampleRates()
    {
        return [44100, 48000];
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