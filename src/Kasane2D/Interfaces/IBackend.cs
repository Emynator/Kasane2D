using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;

namespace Kasane2D.Interfaces;

public interface IBackend
{
    public IEngineRunner CreateRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig
        );
    
    public bool IsSampleRateSupported(int sampleRate);
    
    public int[] GetSupportedSampleRates();
}