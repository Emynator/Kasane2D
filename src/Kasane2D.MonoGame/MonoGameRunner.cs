using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;
using Microsoft.Xna.Framework;

namespace Kasane2D.MonoGame;

internal class MonoGameRunner : Game, IEngineRunner
{
    private readonly EngineMain main;
    private readonly GraphicsConfiguration config;
    private readonly Action<IRasterizer> createRenderer;
    private readonly GraphicsDeviceManager graphics;
    private Action? initRenderer;
    private bool doExit = false;
    private bool isRunning = true;
    
    public MonoGameRunner(EngineMain main, GraphicsConfiguration config, Action<IRasterizer> createRenderer)
    {
        this.main = main;
        this.config = config;
        this.createRenderer = createRenderer;
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = config.ScreenSize.X;
        graphics.PreferredBackBufferHeight = config.ScreenSize.Y;
    }

    protected override void LoadContent()
    {
        var rasterizer = new Rasterizer(config, GraphicsDevice);
        createRenderer(rasterizer);
        initRenderer?.Invoke();
        main.Init();
    }

    protected override void Update(GameTime gameTime)
    {
        if (!isRunning)
        {
            return;
        }
        
        if (doExit)
        {
            Exit();
        }
        
        main.Tick(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!isRunning)
        {
            return;
        }
        
        main.Draw();
        
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