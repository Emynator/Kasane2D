using Kasane2D;
using Kasane2D.Config;
using Kasane2D.MonoGame;

namespace EngineTest;

public static class Program
{
    public static void Main(string[] args)
    {
        var engine = new EngineBuilder()
            .UseMonoGame()
            .ConfigureGraphics(new()
            {
                DefaultTileSize = new(18, 18),
                DefaultSpriteSize = new(24, 24),
                DefaultTilemapDimensions = new(32, 32),
                ViewportSize = new(384, 216),
                ScreenSize = new(1152, 648),
            })
            .ConfigureRenderer
            (
                [
                    new()
                    {
                        Name = "BG1",
                        Type = LayerType.Tilemap,
                    },
                    new()
                    {
                        Name = "Sprite",
                        Type = LayerType.Sprite,
                        SpriteCount = 64,
                    },
                ]
            )
            .WithMain<MyGame>()
            .Build();
        
        engine.Run();
    }
}