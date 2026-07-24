using Kasane2D.Config;
using Kasane2D.Interfaces;

namespace Kasane2D;

public sealed class Engine
{
    private readonly IEngineRunner runner;
    private readonly Action initRenderer;
    private bool isDisposed = false;

    internal Engine(IEngineRunner runner, Action initRenderer)
    {
        this.runner = runner;
        this.initRenderer = initRenderer;
    }
    
    public void Run()
    {
        if (isDisposed)
        {
            return;
        }
        
        runner.Init(initRenderer);
        runner.Run();
        runner.Dispose();
        isDisposed = true;
    }
}