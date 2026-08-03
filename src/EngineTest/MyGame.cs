using Kasane2D;
using Kasane2D.Graphics.Interfaces;

namespace EngineTest;

public class MyGame : EngineMain
{
    private Map map = null!;
    private Player player = null!;
    
    public override void Init()
    {
        var parallax = Renderer.GetSurface<ITilemapSurface>("Parallax");
        var bg = Renderer.GetSurface<ITilemapSurface>("BG");
        var spriteLayer = Renderer.GetSpriteLayer("Sprites");
        var slotManager = Renderer.GetSlotManager("Sprites");

        var tileSheet = Renderer.TextureManager.CreateSpriteAtlas(bg.TileSize, "assets/TileSheet.png");
        var spriteSheet = Renderer.TextureManager.CreateSpriteAtlas(spriteLayer.SpriteSize, "assets/SpriteSheet.png");

        parallax.TileAtlas = tileSheet;
        bg.TileAtlas = tileSheet;
        map = new(parallax, bg);
        var camera = new Camera(parallax, bg);
        player = new(camera, InputSystem, slotManager, spriteSheet);
    }

    protected override void Tick(float dt)
    {
        player.Tick(dt);
    }
}