using Kasane2D.Graphics.Interfaces;
using Kasane2D.MonoGame.Extensions;
using Kasane2D.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kasane2D.MonoGame.Graphics.RenderObjects;

internal class Overlay : MonoGameSurface, IOverlay
{
    private readonly string systemKey;
    private readonly MonoGameSurface child;
    private readonly Texture2D empty;

    public Overlay
        (
        string name,
        GraphicsDevice device,
        MonoGameSurface child
        ) : base(device, child.SurfaceSize, child.Viewport.Size)
    {
        systemKey = $"Backend::GraphicsSystem::Surface::{name}::Rasterize";
        this.child = child;
        Size = child.Viewport.Size;
        empty = new(device, child.Viewport.Size.X, child.Viewport.Size.Y);

        var data = new Color[empty.Width * empty.Height];
        for (var i = 0; i < data.Length; i++)
        {
            data[i] = Color.Transparent;
        }
        empty.SetData(data);
    }

    public ISurface Surface => child;

    public bool IsActive { get; set; } = false;

    public Vec2I Position { get; set; } = Vec2I.Zero;

    public Vec2I Size
    {
        get;
        set
        {
            var x = Math.Max(1, Math.Min(child.Viewport.Size.X, value.X));
            var y = Math.Max(1, Math.Min(child.Viewport.Size.Y, value.Y));
            field = new Vec2I(x, y);
        }
    }

    public Rectangle ClipRect => new(Position.ToPoint(), Size.ToPoint());

    public override Texture2D GetSurface()
    {
        return IsActive ? child.GetSurface() : empty;
    }

    public override void Rasterize()
    {
        Engine.Monitor.StartMeasurement(systemKey);

        if (IsActive)
        {
            child.Rasterize();
        }

        Engine.Monitor.FinishMeasurement(systemKey);
    }
}