using Kasane2D;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Enums;
using KasanePong.Game;
using KasanePong.Screens;

namespace KasanePong;

public class Pong : EngineMain
{
    private ITilemapSurface bg = null!;
    private ISlotManager sprites = null!;
    private ISpriteAtlas gfx = null!;

    private GameState state = GameState.StartScreen;
    private float restartTimer = 0.0f;

    private StartScreen? startScreen;
    private WinScreen? winScreen = null;

    private Score? score = null;
    private Ball? ball = null;
    private Paddle? leftPaddle = null;
    private Paddle? rightPaddle = null;

    public override void Init()
    {
        gfx = Renderer.TextureManager.CreateSpriteAtlas(new(16, 16), "assets/gfx.png");
        bg = Renderer.GetSurface<ITilemapSurface>("Background");
        bg.TileAtlas = gfx;
        sprites = Renderer.GetSlotManager("Sprites");
        
        Reset();
    }

    protected override void Tick(float dt)
    {
        if (InputSystem.Check(KeyKind.Escape) == InputButtonState.JustPressed)
        {
            Cleanup();
        }
        
        switch (state)
        {
            case GameState.StartScreen:
                if (InputSystem.Check(KeyKind.Space) == InputButtonState.JustPressed)
                {
                    StartGame();
                }
                startScreen?.Tick(dt);
                break;

            case GameState.Playing:
                ball?.Tick(dt);
                leftPaddle?.Tick(dt);
                rightPaddle?.Tick(dt);

                var winner = score?.GetWinner() ?? 0;
                if (winner > 0)
                {
                    GameOver(winner);
                }
                break;
            
            case GameState.GameOver:
                winScreen?.Tick(dt);
                restartTimer += dt;

                if (restartTimer >= 5.0f)
                {
                    Reset();
                }
                break;
        }
    }

    private void Reset()
    {
        state = GameState.StartScreen;
        
        winScreen?.Dispose();
        winScreen = null;
        
        startScreen = new(bg, sprites, gfx);
    }

    private void StartGame()
    {
        state = GameState.Playing;
        
        startScreen?.Dispose();
        startScreen = null;
        
        score = new(bg);
        ball = new(sprites, gfx, score);
        leftPaddle = new(InputSystem, sprites, gfx, ball, true);
        rightPaddle = new(InputSystem, sprites, gfx, ball, false);
    }

    private void GameOver(int winner)
    {
        state = GameState.GameOver;
        restartTimer = 0.0f;

        score = null;
        
        ball?.Dispose();
        ball = null;
        
        leftPaddle?.Dispose();
        leftPaddle = null;
        
        rightPaddle?.Dispose();
        rightPaddle = null;

        winScreen = new(bg, sprites, gfx, winner);
    }

    private void Cleanup()
    {
        startScreen?.Dispose();
        winScreen?.Dispose();
        ball?.Dispose();
        leftPaddle?.Dispose();
        rightPaddle?.Dispose();
        
        Renderer.TextureManager.FreeSpriteAtlas(gfx);
        
        Quit();
    }

    private enum GameState
    {
        StartScreen,
        Playing,
        GameOver,
    }
}