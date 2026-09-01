using Kasane2D.Config;
using Kasane2D.Events;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;
using Kasane2D.MonoGame.Input;
using Kasane2D.Sound.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame;

internal class MonoGameRunner : Game, IEngineRunner
{
    private readonly EngineMain main;
    private readonly GraphicsConfiguration config;
    private readonly Action<IRasterizer> createRenderer;
    private readonly AudioConfiguration? audioConfig;
    private readonly Action<KasaneEvent<StereoAudioStream>> assignBufferProcessedEvent;
    private readonly GraphicsDeviceManager graphics;
    private readonly InputSystem inputSystem = new();
    private AudioHandler? audioHandler = null;
    private Action? initRenderer;
    private bool doExit = false;

    public MonoGameRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig,
        Action<KasaneEvent<StereoAudioStream>> assignBufferProcessedEvent
        )
    {
        this.main = main;
        this.config = config;
        this.createRenderer = createRenderer;
        this.audioConfig = audioConfig;
        this.assignBufferProcessedEvent = assignBufferProcessedEvent;
        assignInputSystem(inputSystem);

        graphics = new GraphicsDeviceManager(this);
        if (config.Fullscreen)
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            config.ScreenSize = new(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }
        else
        {
            graphics.PreferredBackBufferHeight = config.ScreenSize.Y;
            graphics.PreferredBackBufferWidth = config.ScreenSize.X;
        }

        IsMouseVisible = config.IsMouseVisibible;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            audioHandler?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override void LoadContent()
    {
        var rasterizer = new Rasterizer(config, GraphicsDevice);
        createRenderer(rasterizer);
        initRenderer?.Invoke();
        if (main.SoundSystem is not null && audioConfig is not null)
        {
            audioHandler = new
            (
                main.SoundSystem,
                audioConfig.BufferSizeInMs,
                audioConfig.BuffersInQueue,
                assignBufferProcessedEvent
            );
        }

        main.Init();
    }

    protected override void Update(GameTime gameTime)
    {
        if (doExit)
        {
            Exit();
        }

        inputSystem.Update();
        main.MainTick((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        main.MainDraw();

        base.Draw(gameTime);
    }

    public void Init(Action initRenderer)
    {
        this.initRenderer = initRenderer;
    }

    public void Stop()
    {
        doExit = true;
    }
}