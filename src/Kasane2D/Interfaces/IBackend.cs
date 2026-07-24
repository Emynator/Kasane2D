using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.Interfaces;

public interface IBackend
{
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer
        );
}