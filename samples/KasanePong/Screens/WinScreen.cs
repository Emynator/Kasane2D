using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;
using KasanePong.Utils;

namespace KasanePong.Screens;

public class WinScreen : IDisposable
{
    private readonly List<WobbleLetter> wobbleLetters = [];

    public WinScreen(ITilemapSurface bg, ISlotManager slotManager, ISpriteAtlas gfx, int winner)
    {
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            for (var y = 0; y < bg.Dimensions.Y; y++)
            {
                bg.UpdateAtlasIndex(x, y, 0, 0);
            }
        }

        bg.UpdateAtlasIndex(6, 5, "p".ToAtlasIndex());
        bg.UpdateAtlasIndex(7, 5, "l".ToAtlasIndex());
        bg.UpdateAtlasIndex(8, 5, "a".ToAtlasIndex());
        bg.UpdateAtlasIndex(9, 5, "y".ToAtlasIndex());
        bg.UpdateAtlasIndex(10, 5, "e".ToAtlasIndex());
        bg.UpdateAtlasIndex(11, 5, "r".ToAtlasIndex());
        bg.UpdateAtlasIndex(12, 5, winner == 1 ? new Vec2I(3, 0) : new(4, 0));

        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(7, 8) * Constants.SpriteSize, "w", 0.0f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(8, 8) * Constants.SpriteSize, "i", 0.25f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(9, 8) * Constants.SpriteSize, "n", 0.5f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(10, 8) * Constants.SpriteSize, "s", 0.75f));
        bg.UpdateAtlasIndex(11, 8, "!".ToAtlasIndex());
    }

    public void Dispose()
    {
        foreach (var letter in wobbleLetters)
        {
            letter.Dispose();
        }
    }

    public void Tick(float dt)
    {
        foreach (var letter in wobbleLetters)
        {
            letter.Tick(dt);
        }
    }
}