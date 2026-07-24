using Kasane2D.Config;

namespace Kasane2D.Interfaces;

public interface IEngineRunner : IDisposable
{
    public void Init(Action initRenderer);

    public void Run();

    public void Stop();
}