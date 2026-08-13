using Kasane2D;
using Kasane2D.Config;
using Kasane2D.MonoGame;
using KasanePong.Utils;

namespace KasanePong;

public static class Program
{
    public static void Main(string[] args)
    {
        var engine = new EngineBuilder()
            .UseMonoGame()
            .ConfigureGraphics
            (
                new()
                {
                    DefaultTileSize = new(Constants.SpriteSize, Constants.SpriteSize),
                    DefaultSpriteSize = new(Constants.SpriteSize, Constants.SpriteSize),
                    DefaultTilemapDimensions =
                        new
                        (
                            Constants.ScreenWidth / Constants.SpriteSize,
                            Constants.ScreenHeight / Constants.SpriteSize
                        ),
                    ViewportSize = new(Constants.ScreenWidth, Constants.ScreenHeight),
                    ScreenSize = new(Constants.ScreenWidth * 3, Constants.ScreenHeight * 3),
                }
            )
            .ConfigureRenderer
            (
                [
                    new()
                    {
                        Name = "Background",
                        Type = LayerType.Tilemap,
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
            .WithMain<Pong>()
            .Build();

        engine.Run();
    }
}