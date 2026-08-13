using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;
using KasanePong.Utils;

namespace KasanePong.Screens;

public class StartScreen : IDisposable
{
    private readonly List<WobbleLetter> wobbleLetters = [];

    public StartScreen(ITilemapSurface bg, ISlotManager slotManager, ISpriteAtlas gfx)
    {
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            for (var y = 0; y < bg.Dimensions.Y; y++)
            {
                bg.UpdateAtlasIndex(x, y, 0, 0);
            }
        }

        bg.UpdateAtlasIndex(4, 5, "p".ToAtlasIndex());
        bg.UpdateAtlasIndex(5, 5, "r".ToAtlasIndex());
        bg.UpdateAtlasIndex(6, 5, "e".ToAtlasIndex());
        bg.UpdateAtlasIndex(7, 5, "s".ToAtlasIndex());
        bg.UpdateAtlasIndex(8, 5, "s".ToAtlasIndex());

        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(10, 5) * Constants.SpriteSize, "s", 0.0f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(11, 5) * Constants.SpriteSize, "p", 0.2f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(12, 5) * Constants.SpriteSize, "a", 0.4f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(13, 5) * Constants.SpriteSize, "c", 0.6f));
        wobbleLetters.Add(new(slotManager, gfx, new Vec2I(14, 5) * Constants.SpriteSize, "e", 0.8f));

        bg.UpdateAtlasIndex(6, 7, "t".ToAtlasIndex());
        bg.UpdateAtlasIndex(7, 7, "o".ToAtlasIndex());

        bg.UpdateAtlasIndex(9, 7, "s".ToAtlasIndex());
        bg.UpdateAtlasIndex(10, 7, "t".ToAtlasIndex());
        bg.UpdateAtlasIndex(11, 7, "a".ToAtlasIndex());
        bg.UpdateAtlasIndex(12, 7, "r".ToAtlasIndex());
        bg.UpdateAtlasIndex(13, 7, "t".ToAtlasIndex());

        bg.UpdateAtlasIndex(1, 13, "o".ToAtlasIndex());
        bg.UpdateAtlasIndex(2, 13, "r".ToAtlasIndex());

        bg.UpdateAtlasIndex(4, 13, "e".ToAtlasIndex());
        bg.UpdateAtlasIndex(5, 13, "s".ToAtlasIndex());
        bg.UpdateAtlasIndex(6, 13, "c".ToAtlasIndex());
        bg.UpdateAtlasIndex(7, 13, "a".ToAtlasIndex());
        bg.UpdateAtlasIndex(8, 13, "p".ToAtlasIndex());
        bg.UpdateAtlasIndex(9, 13, "e".ToAtlasIndex());

        bg.UpdateAtlasIndex(11, 13, "t".ToAtlasIndex());
        bg.UpdateAtlasIndex(12, 13, "o".ToAtlasIndex());

        bg.UpdateAtlasIndex(14, 13, "e".ToAtlasIndex());
        bg.UpdateAtlasIndex(15, 13, "x".ToAtlasIndex());
        bg.UpdateAtlasIndex(16, 13, "i".ToAtlasIndex());
        bg.UpdateAtlasIndex(17, 13, "t".ToAtlasIndex());
        bg.UpdateAtlasIndex(18, 13, 4, 5);
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