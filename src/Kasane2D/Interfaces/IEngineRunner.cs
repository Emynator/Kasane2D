using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.Interfaces;

/// <summary>
/// The main execution context of the engine the backend needs to implement.
/// </summary>
/// <remarks>This interface has to be implemented by a backend for the engine to use it. It is not meant for user code
/// to interact with it.</remarks>
public interface IEngineRunner : IDisposable
{
    /// <summary>
    /// Initialization function the engine core calls to initialize the execution context.
    /// </summary>
    /// <param name="initRenderer">Callback to initialize the engines <see cref="IRenderer"/>.</param>
    /// <remarks>The initRenderer callback should only be called once the backend has ensured that its graphics system
    /// is initialized and the <see cref="IRasterizer"/> implementation is able to create surfaces and textures.</remarks>
    public void Init(Action initRenderer);

    /// <summary>
    /// Starts the execution of the runner.
    /// </summary>
    public void Run();

    /// <summary>
    /// Requests shutdown of the runner.
    /// </summary>
    public void Stop();
}