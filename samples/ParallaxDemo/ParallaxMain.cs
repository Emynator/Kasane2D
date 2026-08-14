using Kasane2D;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Enums;

namespace ParallaxDemo;

public class ParallaxMain : EngineMain
{
    private ITilemapSurface layer0 = null!;
    private ITilemapSurface layer1 = null!;
    private ITilemapSurface layer2 = null!;
    private ISpriteAtlas gfx = null!;
    
    private Camera? cam;
    private Layer0? bg0;
    private Layer1? bg1;
    private Layer2? bg2;
    
    public override void Init()
    {
        Renderer.ClearColor = new()
        {
            R = 0xa1,
            G = 0xd6,
            B = 0xe7,
            A = 0xFF,
        };
        
        layer0 = Renderer.GetSurface<ITilemapSurface>(Constants.Layer0);
        layer1 = Renderer.GetSurface<ITilemapSurface>(Constants.Layer1);
        layer2 = Renderer.GetSurface<ITilemapSurface>(Constants.Layer2);

        gfx = Renderer.TextureManager.CreateSpriteAtlas(layer0.TileSize, "assets/gfx.png");
        layer0.TileAtlas = gfx;
        layer1.TileAtlas = gfx;
        layer2.TileAtlas = gfx;
        
        cam = new(InputSystem, layer0, layer1, layer2);
        
        bg0 = new(layer0);
        bg0.Init();
        
        bg1 = new(layer1);
        bg1.Init();
        
        bg2 = new(layer2);
        bg2.Init();
    }

    protected override void Tick(float dt)
    {
        if (InputSystem.Check(KeyKind.Escape) == InputButtonState.JustPressed)
        {
            Cleanup();
        }

        if (InputSystem.Check(KeyKind.N1) == InputButtonState.JustPressed)
        {
            bg0?.Toggle();
        }
        if (InputSystem.Check(KeyKind.N2) == InputButtonState.JustPressed)
        {
            bg1?.Toggle();
        }
        if (InputSystem.Check(KeyKind.N3) == InputButtonState.JustPressed)
        {
            bg2?.Toggle();
        }
        
        cam?.Tick(dt);
    }

    private void Cleanup()
    {
        Renderer.TextureManager.FreeSpriteAtlas(gfx);
        Quit();
    }
}