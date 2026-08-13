using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;
using Kasane2D.Types;
using KasanePong.Utils;

namespace KasanePong.Screens;

public class WobbleLetter : IDisposable
{
    private readonly ISlotManager slotManager;
    private readonly SpriteSlot sprite;
    private readonly int initialY;
    private float phase;

    public WobbleLetter(ISlotManager slotManager, ISpriteAtlas gfx, Vec2I position, string letter, float initialPhase)
    {
        this.slotManager = slotManager;
        initialY = position.Y;
        if (!slotManager.GetSlot(out var slot))
        {
            throw new InvalidOperationException();
        }

        sprite = slot;
        sprite.SpriteAtlas = gfx;
        sprite.Position = position;
        sprite.AtlasIndex = letter.ToAtlasIndex();
        sprite.IsActive = true;
        
        phase = MathF.Max(0.0f, MathF.Min(1.0f, initialPhase));
    }

    public void Dispose()
    {
        slotManager.FreeSlot(sprite);
    }

    public void Tick(float dt)
    {
        phase += 1.0f * dt;
        if (phase > 1.0f)
        {
            phase -= 1.0f;
        }

        var offset = MathF.Sin(MathF.Tau * phase);
        var y = initialY + offset * Constants.SpriteSize * 0.5f;
        sprite.Position = new(sprite.Position.X, (int)y);
    }
}