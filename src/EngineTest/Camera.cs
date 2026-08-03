using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace EngineTest;

public class Camera
{
    private readonly ITilemapSurface parallax;
    private readonly ITilemapSurface bg;

    public Camera(ITilemapSurface parallax, ITilemapSurface bg)
    {
        this.parallax = parallax;
        this.bg = bg;
    }

    public Vec2F Position { get; private set; }

    public void Move(Vec2F movement)
    {
        var move = new Vec2F(movement.X, 0.0f);
        Position += move;
        
        bg.ScrollTo(Position.ToVec2I());
        parallax.ScrollTo((Position * 0.1f).ToVec2I());
    }
}