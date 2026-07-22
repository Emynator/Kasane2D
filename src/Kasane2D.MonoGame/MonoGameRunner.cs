using Kasane2D.Interfaces;
using Microsoft.Xna.Framework;

namespace Kasane2D.MonoGame;

internal class MonoGameRunner : Game, IEngineRunner
{
    private readonly GraphicsDeviceManager graphics;
    private bool doExit = false;
    
    public MonoGameRunner()
    {
        graphics = new GraphicsDeviceManager(this);
    }
    
    public EngineMain? Main { get; set; }

    protected override void Update(GameTime gameTime)
    {
        if (doExit)
        {
            Exit();
        }
        
        Main?.Tick(gameTime.ElapsedGameTime.TotalMilliseconds);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Main?.Draw();
        
        base.Draw(gameTime);
    }

    public void Init()
    {
        Main?.Init();
    }

    public void Stop()
    {
        doExit = true;
    }
}