using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Input.Enums;
using Kasane2D.Input.Interfaces;
using Kasane2D.Types;

namespace EngineTest;

public class Player
{
    private const float maxWalkSpeed = 100.0f;
    private const float maxRunSpeed = 160.0f;
    private const float walkAcceleration = 180.0f;
    private const float runAcceleration = 260.0f;
    private const float releaseDeceleration = 220.0f;
    private const float skidDeceleration = 450.0f;
    private const float jumpVelocity = 280.0f;
    private const float gravity = 800.0f;
    private const float maxFallSpeed = 450.0f;

    private readonly Camera camera;
    private readonly IInputSystem input;
    private readonly SpriteSlot sprite;
    private readonly SpriteSlot debug;

    private Direction currentDirection = Direction.Right;
    private Vec2F position = new(3.0f * 16.0f, 12.0f * 16.0f);
    private Vec2F currentSpeed = Vec2F.Zero;

    public Player(Camera camera, IInputSystem input, ISlotManager slotManager, ISpriteAtlas atlas)
    {
        this.camera = camera;
        this.input = input;

        if (!slotManager.GetSlot(out var slot))
        {
            throw new InvalidOperationException();
        }

        slot.SpriteAtlas = atlas;
        slot.IsActive = true;
        slot.AtlasIndex = new(1, 0);
        slot.Position = new(4 * 16, 13 * 16);
        sprite = slot;

        if (!slotManager.GetSlot(out var debugSlot))
        {
            throw new InvalidOperationException();
        }

        debugSlot.SpriteAtlas = atlas;
        debugSlot.IsActive = true;
        debugSlot.AtlasIndex = new(0, 9);
        debugSlot.Position = new(2 * 16, 2 * 16);
        debug = debugSlot;
    }
    
    private PlayerState State
    {
        get;
        set
        {
            field = value;
            debug.AtlasIndex = value switch
            {
                PlayerState.Idle => new(0, 9),
                PlayerState.Walking => new(1, 9),
                PlayerState.Running => new(2, 9),
                PlayerState.Airborne => new(3, 9),
                PlayerState.Skidding => new(4, 9),
                _ => new(0, 0),
            };
        }
    } = PlayerState.Idle;

    public void Tick(float dt)
    {
        // check state change
        switch (State)
        {
            case PlayerState.Idle:
                if (input.IsKeyDown(KeyKind.Right))
                {
                    currentDirection = Direction.Right;
                    if (input.IsKeyDown(KeyKind.Z))
                    {
                        State = PlayerState.Running;
                        break;
                    }

                    State = PlayerState.Walking;
                    break;
                }
                if (input.IsKeyDown(KeyKind.Left))
                {
                    currentDirection = Direction.Left;
                    if (input.IsKeyDown(KeyKind.Z))
                    {
                        State = PlayerState.Running;
                        break;
                    }

                    State = PlayerState.Walking;
                    break;
                }
                break;
            
            case PlayerState.Skidding:
                if (currentDirection == Direction.Left)
                {
                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        break;
                    }

                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        currentDirection = Direction.Right;
                        if (input.IsKeyDown(KeyKind.Z))
                        {
                            State = PlayerState.Running;
                            break;
                        }
                        
                        State = PlayerState.Walking;
                        break;
                    }

                    if (currentSpeed.X >= 0.0f)
                    {
                        currentSpeed = Vec2F.Zero;
                        State = PlayerState.Idle;
                        break;
                    }
                }
                if (currentDirection == Direction.Right)
                {
                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        break;
                    }

                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        currentDirection = Direction.Left;
                        if (input.IsKeyDown(KeyKind.Z))
                        {
                            State = PlayerState.Running;
                            break;
                        }
                        
                        State = PlayerState.Walking;
                        break;
                    }

                    if (currentSpeed.X <= 0.0f)
                    {
                        currentSpeed = Vec2F.Zero;
                        State = PlayerState.Idle;
                    }
                }
                break;
            
            case PlayerState.Walking:
                if (currentDirection == Direction.Left)
                {
                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        if (!input.IsKeyDown(KeyKind.Z))
                        {
                            break;
                        }

                        State = PlayerState.Running;
                        break;
                    }
                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        currentDirection = Direction.Right;
                        State = PlayerState.Skidding;
                    }
                    State = PlayerState.Idle;
                    break;
                }
                if (currentDirection == Direction.Right)
                {
                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        if (!input.IsKeyDown(KeyKind.Z))
                        {
                            break;
                        }

                        State = PlayerState.Running;
                        break;
                    }
                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        currentDirection = Direction.Left;
                        State = PlayerState.Skidding;
                        break;
                    }
                    State = PlayerState.Idle;
                }
                break;
            
            case PlayerState.Running:
                if (currentDirection == Direction.Left)
                {
                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        if (input.IsKeyDown(KeyKind.Z))
                        {
                            break;
                        }

                        State = PlayerState.Walking;
                        break;
                    }
                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        currentDirection = Direction.Right;
                        State = PlayerState.Skidding;
                    }
                    State = PlayerState.Idle;
                    break;
                }
                if (currentDirection == Direction.Right)
                {
                    if (input.IsKeyDown(KeyKind.Right))
                    {
                        if (input.IsKeyDown(KeyKind.Z))
                        {
                            break;
                        }

                        State = PlayerState.Walking;
                        break;
                    }
                    if (input.IsKeyDown(KeyKind.Left))
                    {
                        currentDirection = Direction.Left;
                        State = PlayerState.Skidding;
                        break;
                    }
                    State = PlayerState.Idle;
                }
                break;
            
            case PlayerState.Airborne:
                break;
        }
        
        // execute state action
        var dir = currentDirection switch
        {
            Direction.Right => Vec2F.Right,
            Direction.Left => Vec2F.Left,
            _ => Vec2F.Zero,
        };
        switch (State)
        {
            case PlayerState.Idle:
                if (currentSpeed.LengthSquared() == 0.0f)
                {
                    break;
                }
                
                currentSpeed -= dir * releaseDeceleration * dt;
                
                switch (currentDirection)
                {
                    case Direction.Left when currentSpeed.X >= 0.0f:
                    case Direction.Right when currentSpeed.X <= 0.0f:
                        currentSpeed = Vec2F.Zero;
                        break;
                }
                
                break;
            
            case PlayerState.Skidding:
                currentSpeed += dir * skidDeceleration * dt;

                if (currentDirection == Direction.Right && currentSpeed.X <= 0.0f)
                {
                    currentSpeed = Vec2F.Zero;
                    State = PlayerState.Idle;

                    break;
                }
                
                if (currentDirection != Direction.Left || currentSpeed.X < 0.0f)
                {
                    break;
                }
                
                currentSpeed = Vec2F.Zero;
                State = PlayerState.Idle;
                
                break;

            case PlayerState.Walking:
                currentSpeed += dir * walkAcceleration * dt;

                if (currentSpeed.Length() > maxWalkSpeed)
                {
                    currentSpeed = dir * maxWalkSpeed;
                }
                break;

            case PlayerState.Running:
                currentSpeed += dir * runAcceleration * dt;

                if ((currentSpeed.Length() > maxRunSpeed))
                {
                    currentSpeed = dir * maxRunSpeed;
                }
                break;

            case PlayerState.Airborne:
                break;
        }

        var movement = currentSpeed * dt;
        var newPos = sprite.Position + movement.ToVec2I();

        if (newPos.X >= 208)
        {
            
            camera.Move(movement);
            newPos.X = 208;
        }
        if (newPos.X < 16)
        {
            movement.X = 0.0f;
            newPos.X = 16;
        }
        
        sprite.Position = newPos;
        position += movement;
    }

    private enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Skidding,
        Airborne,
    }

    private enum Direction
    {
        Left,
        Right,
    }
}