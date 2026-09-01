using Kasane2D.Config;
using Kasane2D.Events;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;
using Kasane2D.Sound.Types;

namespace Kasane2D.MonoGame;

internal class Backend : IBackend
{
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig,
        Action<KasaneEvent<StereoAudioStream>> assignBufferProcessedEvent
        )
    {
        return new MonoGameRunner(main, config, createRenderer, assignInputSystem, audioConfig, assignBufferProcessedEvent);
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

/// <summary>
/// Extensions for the engine builder.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// Registers the backend implementation with the engine builder.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder UseMonoGame(this EngineBuilder builder)
    {
        builder.Backend ??= new Backend();

        return builder;
    }
}