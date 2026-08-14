using Kasane2D;
using Kasane2D.Config;
using Kasane2D.MonoGame;

namespace ParallaxDemo;

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
                    DefaultTileSize = new(Constants.TileSize, Constants.TileSize),
                    DefaultSpriteSize = new(Constants.TileSize, Constants.TileSize),
                    DefaultTilemapDimensions =
                        new
                        (
                            Constants.ScreenWidth * 3/ Constants.TileSize,
                            Constants.ScreenHeight/ Constants.TileSize
                        ),
                    ViewportSize = new(Constants.ScreenWidth, Constants.ScreenHeight),
                    ScreenSize = new(Constants.ScreenWidth, Constants.ScreenHeight),
                }
            )
            .ConfigureRenderer
            (
                [
                    new()
                    {
                        Name = Constants.Layer0,
                        Type = LayerType.Tilemap,
                    },
                    new()
                    {
                        Name = Constants.Layer1,
                        Type = LayerType.Tilemap,
                    },
                    new()
                    {
                        Name = Constants.Layer2,
                        Type = LayerType.Tilemap,
                    },
                ]
            )
            .WithMain<ParallaxMain>()
            .Build();

        engine.Run();
    }
}