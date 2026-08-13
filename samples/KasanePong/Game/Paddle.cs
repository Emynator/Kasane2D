using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Input.Enums;
using Kasane2D.Input.Interfaces;
using Kasane2D.Types;
using KasanePong.Utils;

namespace KasanePong.Game;

public class Paddle : IDisposable
{
    private const float acceleration = 150.0f;
    private const float deacceleration = 300.0f;
    private const float spacing = 8.0f;
    
    private static readonly Vec2I colliderOffset = new(3, 3);
    private static readonly Vec2I colliderSize = new(8, 24);

    private readonly IInputSystem input;
    private readonly ISlotManager slotManager;
    private readonly bool isLeft;
    private readonly SpriteSlot upperSprite;
    private readonly SpriteSlot lowerSprite;
    private readonly KeyKind upKey;
    private readonly KeyKind downKey;

    private Vec2F position;
    private Rect collider;
    private PaddleState state = PaddleState.Stopped;
    private Direction direction = Direction.Up;
    private float currentSpeed = 0.0f;

    public Paddle(IInputSystem input, ISlotManager slotManager, ISpriteAtlas gfx, Ball ball, bool isLeft)
    {
        this.input = input;
        this.slotManager = slotManager;
        this.isLeft = isLeft;
        Ball = ball;

        if (!slotManager.GetSlot(out var slot0) || !slotManager.GetSlot(out var slot1))
        {
            throw new InvalidOperationException();
        }

        upperSprite = slot0;
        upperSprite.SpriteAtlas = gfx;
        upperSprite.AtlasIndex = new(1, 0);
        upperSprite.IsActive = true;

        lowerSprite = slot1;
        lowerSprite.SpriteAtlas = gfx;
        lowerSprite.AtlasIndex = new(1, 1);
        lowerSprite.IsActive = true;

        if (isLeft)
        {
            upKey = KeyKind.W;
            downKey = KeyKind.S;
            position = new(spacing, 104.0f);
        }
        else
        {
            upKey = KeyKind.Up;
            downKey = KeyKind.Down;
            position = new(Constants.ScreenWidth - Constants.SpriteSize - spacing, 104.0f);
        }

        UpdateSpritePosition();
    }
    
    public Ball Ball { get; set; }

    public void Dispose()
    {
        slotManager.FreeSlot(upperSprite);
        slotManager.FreeSlot(lowerSprite);
    }

    public void Tick(float dt)
    {
        HandleInput();

        switch (state)
        {
            case PaddleState.Stopped:
                currentSpeed = 0.0f;
                break;
            
            case PaddleState.Moving:
                currentSpeed += acceleration * dt;
                if (currentSpeed > Constants.MaxPaddleSpeed)
                {
                    currentSpeed = Constants.MaxPaddleSpeed;
                }
                break;
            
            case PaddleState.Stopping:
                currentSpeed -= deacceleration * dt;
                if (currentSpeed < 0.0f)
                {
                    currentSpeed = 0.0f;
                    state = PaddleState.Stopped;
                }
                break;
        }
        
        position += direction.ToVec2F() * currentSpeed * dt;

        if (position.Y < Constants.PlayAreaTop - colliderOffset.Y)
        {
            position.Y = Constants.PlayAreaTop - colliderOffset.Y;
        }
        if (position.Y > Constants.PlayAreaBottom - colliderSize.Y)
        {
            position.Y = Constants.PlayAreaBottom - colliderSize.Y;
        }
        
        UpdateSpritePosition();

        if (Ball.Collider.Intersects(collider))
        {
            Ball.Bounce(isLeft ? Vec2F.Right : Vec2F.Left, direction, currentSpeed);
        }
    }

    private void UpdateSpritePosition()
    {
        var pos = position.ToVec2I();
        collider = new(pos + colliderOffset, colliderSize);
        
        upperSprite.Position = pos;

        var lowerPos = position.ToVec2I();
        lowerPos.Y += upperSprite.Size.Y;
        lowerSprite.Position = lowerPos;
    }

    private void HandleInput()
    {
        switch (state)
        {
            case PaddleState.Stopped:
                if (input.IsKeyDown(upKey))
                {
                    direction = Direction.Up;
                    state = PaddleState.Moving;
                    break;
                }

                if (input.IsKeyDown(downKey))
                {
                    direction = Direction.Down;
                    state = PaddleState.Moving;
                }
                break;

            case PaddleState.Moving:
                if (!input.IsKeyDown(upKey) && !input.IsKeyDown(downKey))
                {
                    state = PaddleState.Stopping;
                    break;
                }
                if (direction == Direction.Up && input.IsKeyDown(downKey))
                {
                    currentSpeed = -currentSpeed;
                    direction = Direction.Down;
                    break;
                }
                if (direction == Direction.Down && input.IsKeyDown(upKey))
                {
                    currentSpeed = -currentSpeed;
                    direction = Direction.Up;
                }
                break;

            case PaddleState.Stopping:
                if (direction == Direction.Up && input.IsKeyDown(downKey))
                {
                    state = PaddleState.Moving;
                    direction = Direction.Down;
                    currentSpeed = -currentSpeed;
                    break;
                }
                if (direction == Direction.Down && input.IsKeyDown(upKey))
                {
                    state = PaddleState.Moving;
                    direction = Direction.Up;
                    currentSpeed = -currentSpeed;
                }
                break;
        }
    }

    private enum PaddleState
    {
        Stopped,
        Moving,
        Stopping,
    }
}