using System.Text;
using Kasane2D.Config;
using Kasane2D.Graphics;
using Kasane2D.Interfaces;

namespace Kasane2D;

public sealed class EngineBuilder
{
    public IBackend? Backend { get; set; }

    internal GraphicsConfiguration? GraphicsConfig { get; set; }

    internal ICollection<RenderLayerConfig>? RendererConfig { get; set; }

    internal AudioConfiguration? AudioConfig { get; set; }

    internal EngineMain? Main { get; set; }
}

public static class EngineBuilderExtensions
{
    public static EngineBuilder ConfigureGraphics(this EngineBuilder builder, GraphicsConfiguration config)
    {
        builder.GraphicsConfig = config;

        return builder;
    }

    public static EngineBuilder ConfigureRenderer(this EngineBuilder builder, ICollection<RenderLayerConfig> config)
    {
        builder.RendererConfig = config;

        return builder;
    }

    public static EngineBuilder ConfigureAudio(this EngineBuilder builder, AudioConfiguration? config = null)
    {
        builder.AudioConfig = config ?? new();

        return builder;
    }

    public static EngineBuilder WithMain<T>(this EngineBuilder builder) where T : EngineMain
    {
        builder.Main = Activator.CreateInstance<T>();

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

        if (builder.GraphicsConfig is null)
        {
            throw new InvalidOperationException("Graphics not configured.");
        }

        if (builder.RendererConfig is null)
        {
            throw new InvalidOperationException("Renderer not configured.");
        }

        var runner = builder.Backend.CreateRunner
        (
            builder.Main,
            builder.GraphicsConfig,
            rasterizer =>
            {
                builder.Main.Rasterizer = rasterizer;
                builder.Main.InternalRenderer = new Renderer(builder.GraphicsConfig, rasterizer);
            },
            inputSystem => builder.Main.InternalInputSystem = inputSystem,
            builder.AudioConfig
        );
        builder.Main.EngineRunner = runner;

        if (builder.AudioConfig is not null)
        {
            if (!builder.Backend.IsSampleRateSupported(builder.AudioConfig.SampleRate))
            {
                var sampleRateList = builder
                    .Backend
                    .GetSupportedSampleRates()
                    .Aggregate(new StringBuilder(), (sb, val) => sb.Append($", {val}"), sb => sb.ToString())[2..];

                throw new InvalidOperationException
                (
                    $"Backend does not support sample rate {
                        builder.AudioConfig.SampleRate
                    }. Supported sample rates are: {
                        sampleRateList
                    }"
                );
            }
            
            builder.Main.InternalSoundSystem = new(builder.AudioConfig);
        }

        return new(runner, () => builder.Main.InternalRenderer?.Init(builder.RendererConfig));
    }
}