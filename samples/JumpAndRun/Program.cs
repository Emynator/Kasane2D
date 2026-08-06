using Kasane2D;
using Kasane2D.Config;
using Kasane2D.MonoGame;

namespace JumpAndRun;

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
                DefaultTilemapDimensions = new(32, 32),
                ViewportSize = new(256, 240),
                ScreenSize = new(768, 720),
            })
            .ConfigureRenderer
            (
                [
                    new()
                    {
                        Name = "Parallax",
                        Type = LayerType.Tilemap,
                        Dimensions = new(48, 15),
                    },
                    new()
                    {
                        Name = "BG",
                        Type = LayerType.Tilemap,
                        Dimensions = new(224, 15),
                    },
                    new()
                    {
                        Name = "Sprites",
                        Type = LayerType.Sprite,
                        SpriteCount = 64,
                    },
                ]
            )
            .ConfigureAudio()
            .WithMain<JumpAndRunGame>()
            .Build();
        
        engine.Run();
    }
}