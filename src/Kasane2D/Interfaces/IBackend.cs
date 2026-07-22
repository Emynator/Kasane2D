using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.Interfaces;

public interface IBackend
{
    public IRasterizer CreateRasterizer(GraphicsConfiguration config);
    
    public IEngineRunner CreateRunner(EngineMain main);
}