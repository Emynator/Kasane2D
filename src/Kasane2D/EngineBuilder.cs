using System.Text;
using Kasane2D.Config;
using Kasane2D.Graphics;
using Kasane2D.Interfaces;

namespace Kasane2D;

/// <summary>
/// Configures and builds the core engine before usage.
/// </summary>
public sealed class EngineBuilder
{
    /// <summary>
    /// Backend implementations attach here.
    /// </summary>
    /// <remarks>Intended for custom backend implementations. Not intended to be accessed by user code.</remarks>
    public IBackend? Backend { get; set; }

    internal GraphicsConfiguration? GraphicsConfig { get; set; }

    internal ICollection<RenderLayerConfig>? RendererConfig { get; set; }

    internal AudioConfiguration? AudioConfig { get; set; }

    internal PerformanceMonitorConfiguration? PerformanceMonitorConfig { get; set; }

    internal EngineMain? Main { get; set; }
}

/// <summary>
/// Extension methods for the Engine Builder
/// </summary>
public static class EngineBuilderExtensions
{
    /// <summary>
    /// Required: Configures the engine's graphics system.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <param name="config">The config.</param>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder ConfigureGraphics(this EngineBuilder builder, GraphicsConfiguration? config)
    {
        builder.GraphicsConfig = config;

        return builder;
    }

    /// <summary>
    /// Required: Configures the rendering layers.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <param name="config">The config.</param>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder ConfigureRenderer(this EngineBuilder builder, ICollection<RenderLayerConfig> config)
    {
        builder.RendererConfig = config;

        return builder;
    }

    /// <summary>
    /// Optional: Configures the sound system.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <param name="config">Optional: The config. Default config is used when null.</param>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder ConfigureAudio(this EngineBuilder builder, AudioConfiguration? config = null)
    {
        builder.AudioConfig = config ?? new();

        return builder;
    }

    /// <summary>
    /// Optional: Configures the performance monitoring system.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <param name="config">Optional: The config. Default config is used when null.</param>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder ConfigurePerformanceMonitoring
        (
        this EngineBuilder builder,
        PerformanceMonitorConfiguration? config = null
        )
    {
        builder.PerformanceMonitorConfig = config ?? new();

        return builder;
    }

    /// <summary>
    /// Required: Configures the user code main.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <typeparam name="T">User code implementation of <see cref="EngineMain"/>.</typeparam>
    /// <returns>The engine builder.</returns>
    public static EngineBuilder WithMain<T>(this EngineBuilder builder) where T : EngineMain
    {
        builder.Main = Activator.CreateInstance<T>();

        return builder;
    }

    /// <summary>
    /// Builds the engine core.
    /// </summary>
    /// <param name="builder">The engine builder.</param>
    /// <returns>The configured engine object.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configuration is not valid.</exception>
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
            builder.AudioConfig,
            ev => builder.Main.InternalSoundSystem?.InternalBufferProcessedEvent = ev
        );
        builder.Main.EngineRunner = runner;

        return new
        (
            runner,
            () => builder.Main.InternalRenderer?.Init(builder.RendererConfig),
            builder.PerformanceMonitorConfig
        );
    }
}