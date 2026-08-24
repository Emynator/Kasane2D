using Kasane2D.Graphics;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.Sound;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D;

/// <summary>
/// Entry point of all user code containing the main loop and additional render code..
/// </summary>
public abstract class EngineMain
{
    private const string systemKey = "UserCode::EngineMain::";
    
    /// <summary>
    /// Gets the engine's sound system if it is initialized.
    /// </summary>
    public ISoundSystem? SoundSystem => InternalSoundSystem;

    internal IRasterizer? Rasterizer { get; set; }

    internal Renderer? InternalRenderer { get; set; }

    internal IEngineRunner? EngineRunner { get; set; }

    internal IInputSystem? InternalInputSystem { get; set; }

    internal SoundSystem? InternalSoundSystem { get; set; }

    /// <summary>
    /// Gets the engine's renderer.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the renderer is not initialized.</exception>
    /// <remarks>Should never throw in a proper configured environment. An exception here indicates either a configuration
    /// error or a backend error.</remarks>
    protected IRenderer Renderer =>
        InternalRenderer ?? throw new InvalidOperationException("Renderer not initialized.");

    /// <summary>
    /// Gets the engine's input system.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the input system is not initialized.</exception>
    /// <remarks>Should never throw in a proper configured environment. An exception here indicates either a configuration
    /// error or a backend error.</remarks>
    protected IInputSystem InputSystem =>
        InternalInputSystem ?? throw new InvalidOperationException("Input system not initialized.");

    /// <summary>
    /// Optional: user code init function.
    /// </summary>
    /// <remarks>The user code init function runs before the first iteration of Tick but after all engine systems
    /// are initialized.</remarks>
    public virtual void Init()
    {
    }

    /// <summary>
    /// Engine core ticking function.
    /// </summary>
    /// <param name="dt">Time in seconds since the last call to MainTick.</param>
    /// <remarks>To be called by the backend's engine runner. User code should not override this function.</remarks>
    public void MainTick(float dt)
    {
        Engine.Monitor.StartMeasurement($"{systemKey}Tick");
        Tick(dt);
        Engine.Monitor.FinishMeasurement($"{systemKey}Tick");
        Engine.Monitor.Tick(dt);
    }

    /// <summary>
    /// Engine core drawing function.
    /// </summary>
    /// <remarks>To be called by the backend's engine runner. User code should not override this function.</remarks>
    public void MainDraw()
    {
        Engine.Monitor.StartMeasurement($"{systemKey}Draw");
        Draw();
        Engine.Monitor.FinishMeasurement($"{systemKey}Draw");
        Rasterizer?.Rasterize();
    }

    /// <summary>
    /// Required: User code main function. The user code part of the main game loop.
    /// </summary>
    /// <param name="dt">Time in seconds since the last iteration.</param>
    protected abstract void Tick(float dt);

    /// <summary>
    /// Optional: User code drawing function.
    /// </summary>
    /// <remarks>All custom drawing and rendering should be done in this function. This function is guaranteed to be
    /// called before the engine's main rendering function composes the layers together.</remarks>
    protected virtual void Draw()
    {
    }

    /// <summary>
    /// Requests program exit from user code.
    /// </summary>
    protected void Quit()
    {
        Engine.Monitor.FinalPrint();
        EngineRunner?.Stop();
    }
}