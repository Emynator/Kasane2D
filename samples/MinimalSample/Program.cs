using Kasane2D;
using Kasane2D.Config;
using Kasane2D.MonoGame;

namespace MinimalSample;

public static class Program
{
    public static void Main(string[] args)
    {
        var engine = new EngineBuilder()
            .UseMonoGame()
            .ConfigureGraphics(new()
            {
                DefaultTileSize = new(16, 16),
                DefaultSpriteSize = new(16, 16),
                ViewportSize = new(320, 240),
                ScreenSize = new(960, 720),
            })
            .ConfigureRenderer
            (
                [
                    new()
                    {
                        Name = "Sprites",
                        Type = LayerType.Sprite,
                        SpriteCount = 64,
                    },
                ]
            )
            .ConfigureAudio()
            .WithMain<MinimalGame>()
            .Build();
        
        engine.Run();
    }
}