using Kasane2D;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Enums;
using Kasane2D.Primitives;

namespace EngineTest;

public class MyGame : EngineMain
{
    private ITilemapSurface bg = null!;
    private ISpriteLayer sl = null!;
    private Vec2F spritePos = new(0.0f, 60.0f);

    public override void Init()
    {
        bg = Renderer.GetSurface<ITilemapSurface>("BG1");
        sl = Renderer.GetSpriteLayer("Sprite");

        var bgAtlas = Renderer.TextureManager.CreateSpriteAtlas(bg.TileSize, "assets/BgSheet.png");
        var slAtlas = Renderer.TextureManager.CreateSpriteAtlas(sl.SpriteSize, "assets/Sprites.png");

        bg.TileAtlas = bgAtlas;
        sl.Sprites[0].SpriteAtlas = slAtlas;
        sl.Sprites[0].AtlasIndex = new(1, 0);
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            for (var y = 0; y < bg.Dimensions.Y; y++)
            {
                bg.UpdateAtlasIndex(new(x, y), new(2, 6));
            }
        }
    }

    public override void Tick(double dt)
    {
        var move = Vec2F.Zero;
        if (InputSystem.Check(KeyKind.Up) == InputButtonState.Pressed)
        {
            move += Vec2F.Up;
        }
        if (InputSystem.Check(KeyKind.Down) == InputButtonState.Pressed)
        {
            move += Vec2F.Down;
        }
        if (InputSystem.Check(KeyKind.Left) == InputButtonState.Pressed)
        {
            move += Vec2F.Left;
        }
        if (InputSystem.Check(KeyKind.Right) == InputButtonState.Pressed)
        {
            move += Vec2F.Right;
        }
        move *= 180.0f * (float)dt;
        spritePos += move;
        
        if (spritePos.X > 408.0f)
        {
            spritePos.X = 0.0f;
        }
        if (spritePos.X < 0.0f)
        {
            spritePos.X = 408.0f;
        }
        if (spritePos.Y > 264.0f)
        {
            spritePos.Y = 0.0f;
        }
        if (spritePos.Y < 0.0f)
        {
            spritePos.Y = 264.0f;
        }

        sl.Sprites[0].Position = spritePos.ToVec2I();
    }
}