using Kasane2D.Interfaces;

namespace Kasane2D;

public sealed class Engine
{
    private readonly IEngineRunner runner;
    private bool isDisposed = false;

    internal Engine(IEngineRunner runner)
    {
        this.runner = runner;
    }
    
    public void Run()
    {
        if (isDisposed)
        {
            return;
        }
        
        runner.Run();
        runner.Dispose();
        isDisposed = true;
    }
}