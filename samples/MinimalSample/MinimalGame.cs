using Kasane2D;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Enums;
using Kasane2D.Types;

namespace MinimalSample;

public class MinimalGame : EngineMain
{
    private const float speed = 100.0f;
    private ISpriteLayer spriteLayer = null!;
    private Vec2F position = new(160.0f, 120.0f);
    
    public override void Init()
    {
        spriteLayer = Renderer.GetSpriteLayer("Sprites");
        var atlas = Renderer.TextureManager.CreateSpriteAtlas(spriteLayer.SpriteSize, "assets/square.png");
        spriteLayer.Sprites[0].SpriteAtlas = atlas;
        spriteLayer.Sprites[0].Position = position.ToVec2I();
        spriteLayer.Sprites[0].IsActive = true;
    }
    
    protected override void Tick(float dt)
    {
        var movement = Vec2F.Zero;
        if (InputSystem.IsKeyDown(KeyKind.Up))
        {
            movement += Vec2F.Up;
        }
        if (InputSystem.IsKeyDown(KeyKind.Down))
        {
            movement += Vec2F.Down;
        }
        if (InputSystem.IsKeyDown(KeyKind.Left))
        {
            movement += Vec2F.Left;
        }
        if (InputSystem.IsKeyDown(KeyKind.Right))
        {
            movement += Vec2F.Right;
        }
        
        movement *= speed * dt;
        position += movement;

        if (position.Y < -spriteLayer.SpriteSize.Y)
        {
            position.Y = 240.0f + spriteLayer.SpriteSize.Y;
        }
        if (position.Y > 240.0f + spriteLayer.SpriteSize.Y)
        {
            position.Y = -spriteLayer.SpriteSize.Y;
        }
        if (position.X < -spriteLayer.SpriteSize.X)
        {
            position.X = 320.0f + spriteLayer.SpriteSize.X;
        }
        if (position.X > 320.0f + spriteLayer.SpriteSize.X)
        {
            position.X = -spriteLayer.SpriteSize.X;
        }
        
        spriteLayer.Sprites[0].Position = position.ToVec2I();
    }
}