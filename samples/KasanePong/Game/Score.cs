using Kasane2D.Graphics.Interfaces;
using Kasane2D.Types;

namespace KasanePong.Game;

public class Score
{
    private readonly ITilemapSurface bg;
    private readonly Vec2I scoreLeftPos = new(8, 0);
    private readonly Vec2I scoreRightPos = new(10, 0);
    private int scoreLeft = 0;
    private int scoreRight = 0;

    public Score(ITilemapSurface bg)
    {
        this.bg = bg;
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            for (var y = 0; y < bg.Dimensions.Y; y++)
            {
                bg.UpdateAtlasIndex(x, y, 0, 0);
            }
        }
        
        bg.UpdateAtlasIndex(9, 0, 6, 1);
        for (var i = 0; i < 20; i++)
        {
            bg.UpdateAtlasIndex(i, 1, 3, 5);
        }
        bg.UpdateAtlasIndex(scoreLeftPos, GetNumberPos(0));
        bg.UpdateAtlasIndex(scoreRightPos, GetNumberPos(0));
    }

    public void IncrementScore(bool left)
    {
        if (left)
        {
            scoreLeft++;
            bg.UpdateAtlasIndex(scoreLeftPos, GetNumberPos(scoreLeft));

            return;
        }

        scoreRight++;
        bg.UpdateAtlasIndex(scoreRightPos, GetNumberPos(scoreRight));
    }

    public int GetWinner()
    {
        if (scoreLeft == 10)
        {
            return 1;
        }
        
        if (scoreRight == 10)
        {
            return 2;
        }
        
        return 0;
    }

    private Vec2I GetNumberPos(int number)
    {
        return number switch
        {
            0 => new(2, 0),
            1 => new(3, 0),
            2 => new(4, 0),
            3 => new(5, 0),
            4 => new(6, 0),
            5 => new(7, 0),
            6 => new(2, 1),
            7 => new(3, 1),
            8 => new(4, 1),
            9 => new(5, 1),
            _ => new(0, 0),
        };
    }
}