using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;

namespace Kasane2D.Interfaces;

/// <summary>
/// Represents a backend implementation usable by the core engine.
/// </summary>
/// <remarks>This interface has to be implemented by a backend for the engine to use it. It is not meant for user code
/// to interact with it.</remarks>
public interface IBackend
{
    /// <summary>
    /// Creates an engine runner that manages the execution of the game and handles the game's lifetime.
    /// </summary>
    /// <param name="main">The user code entry point.</param>
    /// <param name="config">The graphics configuration.</param>
    /// <param name="createRenderer">Callback to create the IRenderer after the backend has created its rasterizer.</param>
    /// <param name="assignInputSystem">Callback to assign the input system implementation created by the backend.</param>
    /// <param name="audioConfig">Optional: the audio configuration. May be null.</param>
    /// <returns></returns>
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig
        );
    
    /// <summary>
    /// Checks if the backend supports the requested sample rate for the sound system.
    /// </summary>
    /// <param name="sampleRate">The sample rate in Hz.</param>
    /// <returns>True if the sample rate is supported, false if not.</returns>
    public bool IsSampleRateSupported(int sampleRate);
    
    /// <summary>
    /// Gets a list of all sample rates supported by the backend implementation.
    /// </summary>
    /// <returns>The array of supported sample rates in Hz.</returns>
    public int[] GetSupportedSampleRates();
}