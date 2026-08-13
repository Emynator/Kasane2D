using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Types;
using KasanePong.Utils;

namespace KasanePong.Game;

public class Ball : IDisposable
{
    private const float maxSpeed = 200.0f;
    private const float increment = 10.0f;
    private const float resetIncreaseFactor = 2.0f;

    private static readonly Vec2F startPosition = new(160.0f, 120.0f);
    private static readonly Vec2I colliderOffset = new(3, 3);
    private static readonly Vec2I colliderSize = new(8, 8);

    private readonly ISlotManager slotManager;
    private readonly Score score;
    private readonly SpriteSlot sprite;

    private Vec2F position = startPosition;
    private Vec2F direction;
    private float currentStartSpeed = 50.0f;
    private float currentSpeed = 50.0f;
    private bool allowBounce = true;
    private float bounceTimer = 0.0f;

    public Ball(ISlotManager slotManager, ISpriteAtlas gfx, Score score)
    {
        this.slotManager = slotManager;
        this.score = score;
        if (!slotManager.GetSlot(out var slot))
        {
            throw new InvalidOperationException();
        }

        sprite = slot;
        sprite.SpriteAtlas = gfx;
        sprite.AtlasIndex = new(0, 1);
        sprite.Position = position.ToVec2I();
        sprite.IsActive = true;

        direction = Random.Shared.Next(0, 2) == 0 ? Vec2F.Left : Vec2F.Right;
    }

    public Rect Collider => new(position.ToVec2I() + colliderOffset, colliderSize);

    public void Dispose()
    {
        slotManager.FreeSlot(sprite);
    }

    public void Tick(float dt)
    {
        if (!allowBounce)
        {
            bounceTimer += dt;
            if (bounceTimer > 0.2f)
            {
                bounceTimer = 0.0f;
                allowBounce = true;
            }
        }
        
        position += direction * currentSpeed * dt;

        if (position.Y < Constants.PlayAreaTop)
        {
            position.Y = Constants.PlayAreaTop + MathF.Abs(position.Y - Constants.PlayAreaTop);
            direction.Y = -direction.Y;
        }
        if (position.Y > Constants.PlayAreaBottom)
        {
            position.Y = Constants.PlayAreaBottom - MathF.Abs(position.Y - Constants.PlayAreaBottom);
            direction.Y = -direction.Y;
        }

        if (position.X < -Constants.SpriteSize)
        {
            score.IncrementScore(false);
            Reset(Vec2F.Right);
        }
        if (position.X > Constants.ScreenWidth)
        {
            score.IncrementScore(true);
            Reset(Vec2F.Left);
        }

        sprite.Position = position.ToVec2I();
    }

    public void Bounce(Vec2F bounceDirection, Direction paddleDirection, float speed)
    {
        if (!allowBounce)
        {
            return;
        }
        
        allowBounce = false;
        currentSpeed += increment;
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        var angle = float.Lerp(0.0f, 80.0f, speed / Constants.MaxPaddleSpeed);
        if (paddleDirection == Direction.Up)
        {
            angle = -angle;
        }

        direction = bounceDirection.Rotate(angle);
    }

    private void Reset(Vec2F dir)
    {
        currentStartSpeed += increment * resetIncreaseFactor;
        if (currentStartSpeed > maxSpeed)
        {
            currentStartSpeed = maxSpeed;
        }

        position = startPosition;
        direction = dir;
        currentSpeed = currentStartSpeed;
    }
}