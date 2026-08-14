using Kasane2D.Enums;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Enums;
using Kasane2D.Input.Interfaces;
using Kasane2D.Types;

namespace ParallaxDemo;

public class Camera
{
    private const float speed = 150.0f;
    
    private readonly IInputSystem input;
    private readonly ITilemapSurface layer0;
    private readonly ITilemapSurface layer1;
    private readonly ITilemapSurface layer2;

    public Camera(IInputSystem input, ITilemapSurface layer0, ITilemapSurface layer1, ITilemapSurface layer2)
    {
        this.input = input;
        this.layer0 = layer0;
        this.layer1 = layer1;
        this.layer2 = layer2;
    }

    public void Tick(float dt)
    {
        var movement = Vec2F.Zero;
        if (input.IsKeyDown(KeyKind.Left))
        {
            movement += Vec2F.Left;
        }
        if (input.IsKeyDown(KeyKind.Right))
        {
            movement += Vec2F.Right;
        }

        movement *= speed * dt;
        if (input.IsKeyDown(KeyKind.LeftShift))
        {
            movement *= 2.0f;
        }
        
        layer0.ScrollBy((movement * 0.5f).ToVec2I(RoundingMode.Nearest));
        layer1.ScrollBy(movement);
        layer2.ScrollBy(movement * 2.0f);
    }
}