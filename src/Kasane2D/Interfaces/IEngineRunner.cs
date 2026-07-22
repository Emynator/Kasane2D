namespace Kasane2D.Interfaces;

public interface IEngineRunner : IDisposable
{
    public void Init();

    public void Run();

    public void Stop();
}