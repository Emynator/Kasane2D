using Kasane2D.Graphics.Interfaces;

namespace EngineTest;

public class Map
{
    private readonly ITilemapSurface parallax;
    private readonly ITilemapSurface bg;

    public Map(ITilemapSurface parallax, ITilemapSurface bg)
    {
        this.parallax = parallax;
        this.bg = bg;
        
        LoadParallax();
        LoadMap();
    }
    
    private void LoadParallax()
    {
        // sky
        for (var x = 0; x < parallax.Dimensions.X; x++)
        {
            for (var y = 0; y < parallax.Dimensions.Y; y++)
            {
                parallax.UpdateAtlasIndex(x, y, 1, 0);
            }
        }
        
        // big hill
        parallax.UpdateAtlasIndex(2, 10, 9, 1);
        
        parallax.UpdateAtlasIndex(1, 11, 9, 0);
        parallax.UpdateHFlip(1, 11, true);
        parallax.UpdateAtlasIndex(2, 11, 8, 0);
        parallax.UpdateAtlasIndex(3, 11, 9, 0);
        
        parallax.UpdateAtlasIndex(0, 12, 9, 0);
        parallax.UpdateHFlip(0, 12, true);
        parallax.UpdateAtlasIndex(1, 12, 8, 0);
        parallax.UpdateAtlasIndex(2, 12, 8, 1);
        parallax.UpdateAtlasIndex(3, 12, 8, 0);
        parallax.UpdateHFlip(3, 12, true);
        parallax.UpdateAtlasIndex(4, 12, 9, 0);
        
        // small hill
        parallax.UpdateAtlasIndex(17, 11, 9, 1);
        
        parallax.UpdateAtlasIndex(16, 12, 9, 0);
        parallax.UpdateHFlip(16, 12, true);
        parallax.UpdateAtlasIndex(17, 12, 8, 0);
        parallax.UpdateAtlasIndex(18, 12, 9, 0);
        
        // small bush
        parallax.UpdateAtlasIndex(23, 12, 0, 1);
        parallax.UpdateAtlasIndex(24, 12, 1, 1);
        parallax.UpdateAtlasIndex(25, 12, 2, 1);
        
        // medium bush
        parallax.UpdateAtlasIndex(41, 12, 0, 1);
        parallax.UpdateAtlasIndex(42, 12, 1, 1);
        parallax.UpdateAtlasIndex(43, 12, 1, 1);
        parallax.UpdateAtlasIndex(44, 12, 2, 1);
        
        // large bush
        parallax.UpdateAtlasIndex(11, 12, 0, 1);
        parallax.UpdateAtlasIndex(12, 12, 1, 1);
        parallax.UpdateAtlasIndex(13, 12, 1, 1);
        parallax.UpdateAtlasIndex(14, 12, 1, 1);
        parallax.UpdateAtlasIndex(15, 12, 2, 1);
        
        // small cloud 1
        parallax.UpdateAtlasIndex(8, 3, 4, 0);
        parallax.UpdateAtlasIndex(9, 3, 2, 0);
        parallax.UpdateAtlasIndex(10, 3, 7, 0);
        
        parallax.UpdateAtlasIndex(8, 4, 5, 0);
        parallax.UpdateAtlasIndex(9, 4, 3, 0);
        parallax.UpdateAtlasIndex(10, 4, 6, 0);
        
        // small cloud 2
        parallax.UpdateAtlasIndex(19, 2, 4, 0);
        parallax.UpdateAtlasIndex(20, 2, 2, 0);
        parallax.UpdateAtlasIndex(21, 2, 7, 0);
        
        parallax.UpdateAtlasIndex(19, 3, 5, 0);
        parallax.UpdateAtlasIndex(20, 3, 3, 0);
        parallax.UpdateAtlasIndex(21, 3, 6, 0);
        
        // medium cloud
        parallax.UpdateAtlasIndex(36, 2, 4, 0);
        parallax.UpdateAtlasIndex(37, 2, 2, 0);
        parallax.UpdateAtlasIndex(38, 2, 2, 0);
        parallax.UpdateAtlasIndex(39, 2, 7, 0);
        
        parallax.UpdateAtlasIndex(36, 3, 5, 0);
        parallax.UpdateAtlasIndex(37, 3, 3, 0);
        parallax.UpdateAtlasIndex(38, 3, 3, 0);
        parallax.UpdateAtlasIndex(39, 3, 6, 0);
        
        // large cloud
        parallax.UpdateAtlasIndex(27, 3, 4, 0);
        parallax.UpdateAtlasIndex(28, 3, 2, 0);
        parallax.UpdateAtlasIndex(29, 3, 2, 0);
        parallax.UpdateAtlasIndex(30, 3, 2, 0);
        parallax.UpdateAtlasIndex(31, 3, 7, 0);
        
        parallax.UpdateAtlasIndex(27, 4, 5, 0);
        parallax.UpdateAtlasIndex(28, 4, 3, 0);
        parallax.UpdateAtlasIndex(29, 4, 3, 0);
        parallax.UpdateAtlasIndex(30, 4, 3, 0);
        parallax.UpdateAtlasIndex(31, 4, 6, 0);
    }

    private void LoadMap()
    {
        // clear
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            for (var y = 0; y < bg.Dimensions.Y; y++)
            {
                bg.UpdateAtlasIndex(x, y, 0, 0);
            }
        }

        // floor
        for (var x = 0; x < bg.Dimensions.X; x++)
        {
            if (x is 69 or 70 or 86 or 87 or 89 or 153 or 154)
            {
                continue;
            }
            
            bg.UpdateAtlasIndex(x, 13, 3, 2);
            bg.UpdateAtlasIndex(x, 14, 3, 2);
        }
        
        // pipes
        
        // pipe 1
        bg.UpdateAtlasIndex(28, 11, 0, 2);
        bg.UpdateAtlasIndex(29, 11, 1, 2);
        bg.UpdateAtlasIndex(28, 12, 0, 3);
        bg.UpdateAtlasIndex(29, 12, 1, 3);
        
        // pipe 2
        bg.UpdateAtlasIndex(38, 10, 0, 2);
        bg.UpdateAtlasIndex(39, 10, 1, 2);
        bg.UpdateAtlasIndex(38, 11, 0, 3);
        bg.UpdateAtlasIndex(39, 11, 1, 3);
        bg.UpdateAtlasIndex(38, 12, 0, 3);
        bg.UpdateAtlasIndex(39, 12, 1, 3);
        
        // pipe 3
        bg.UpdateAtlasIndex(46, 9, 0, 2);
        bg.UpdateAtlasIndex(47, 9, 1, 2);
        bg.UpdateAtlasIndex(46, 10, 0, 3);
        bg.UpdateAtlasIndex(47, 10, 1, 3);
        bg.UpdateAtlasIndex(46, 11, 0, 3);
        bg.UpdateAtlasIndex(47, 11, 1, 3);
        bg.UpdateAtlasIndex(46, 12, 0, 3);
        bg.UpdateAtlasIndex(47, 12, 1, 3);
        
        // pipe 4
        bg.UpdateAtlasIndex(57, 9, 0, 2);
        bg.UpdateAtlasIndex(58, 9, 1, 2);
        bg.UpdateAtlasIndex(57, 10, 0, 3);
        bg.UpdateAtlasIndex(58, 10, 1, 3);
        bg.UpdateAtlasIndex(57, 11, 0, 3);
        bg.UpdateAtlasIndex(58, 11, 1, 3);
        bg.UpdateAtlasIndex(57, 12, 0, 3);
        bg.UpdateAtlasIndex(58, 12, 1, 3);
        
        // pipe 5
        bg.UpdateAtlasIndex(163, 11, 0, 2);
        bg.UpdateAtlasIndex(164, 11, 1, 2);
        bg.UpdateAtlasIndex(163, 12, 0, 3);
        bg.UpdateAtlasIndex(164, 12, 1, 3);
        
        // pipe 6
        bg.UpdateAtlasIndex(179, 11, 0, 2);
        bg.UpdateAtlasIndex(180, 11, 1, 2);
        bg.UpdateAtlasIndex(179, 12, 0, 3);
        bg.UpdateAtlasIndex(180, 12, 1, 3);
        
        // bricks
        bg.UpdateAtlasIndex(20, 9, 5, 3);
        bg.UpdateAtlasIndex(22, 9, 5, 3);
        bg.UpdateAtlasIndex(24, 9, 5, 3);
        
        bg.UpdateAtlasIndex(77, 9, 5, 3);
        bg.UpdateAtlasIndex(79, 9, 5, 3);
        
        bg.UpdateAtlasIndex(80, 5, 5, 3);
        bg.UpdateAtlasIndex(81, 5, 5, 3);
        bg.UpdateAtlasIndex(82, 5, 5, 3);
        bg.UpdateAtlasIndex(83, 5, 5, 3);
        bg.UpdateAtlasIndex(84, 5, 5, 3);
        bg.UpdateAtlasIndex(85, 5, 5, 3);
        bg.UpdateAtlasIndex(86, 5, 5, 3);
        bg.UpdateAtlasIndex(87, 5, 5, 3);
        
        bg.UpdateAtlasIndex(91, 5, 5, 3);
        bg.UpdateAtlasIndex(92, 5, 5, 3);
        bg.UpdateAtlasIndex(93, 5, 5, 3);
        
        bg.UpdateAtlasIndex(94, 9, 5, 3);
        
        bg.UpdateAtlasIndex(100, 9, 5, 3);
        bg.UpdateAtlasIndex(101, 9, 5, 3);
        
        bg.UpdateAtlasIndex(118, 9, 5, 3);
        
        bg.UpdateAtlasIndex(121, 5, 5, 3);
        bg.UpdateAtlasIndex(122, 5, 5, 3);
        bg.UpdateAtlasIndex(123, 5, 5, 3);
        
        bg.UpdateAtlasIndex(128, 5, 5, 3);
        bg.UpdateAtlasIndex(131, 5, 5, 3);
        
        bg.UpdateAtlasIndex(129, 9, 5, 3);
        bg.UpdateAtlasIndex(130, 9, 5, 3);
        
        bg.UpdateAtlasIndex(168, 9, 5, 3);
        bg.UpdateAtlasIndex(169, 9, 5, 3);
        bg.UpdateAtlasIndex(171, 9, 5, 3);
        
        // ? blocks
        bg.UpdateAtlasIndex(16, 9, 4, 3);
        
        bg.UpdateAtlasIndex(21, 9, 4, 3);
        bg.UpdateAtlasIndex(23, 9, 4, 3);
        
        bg.UpdateAtlasIndex(22, 5, 4, 3);
        
        bg.UpdateAtlasIndex(78, 9, 4, 3);
        
        bg.UpdateAtlasIndex(94, 5, 4, 3);
        
        bg.UpdateAtlasIndex(109, 5, 4, 3);
        bg.UpdateAtlasIndex(106, 9, 4, 3);
        bg.UpdateAtlasIndex(109, 9, 4, 3);
        bg.UpdateAtlasIndex(112, 9, 4, 3);
        
        bg.UpdateAtlasIndex(129, 5, 4, 3);
        bg.UpdateAtlasIndex(130, 5, 4, 3);
        
        bg.UpdateAtlasIndex(170, 9, 4, 3);
        
        // stairs
        
        // stair 1
        bg.UpdateAtlasIndex(137, 9, 3, 3);
        bg.UpdateAtlasIndex(136, 10, 3, 3);
        bg.UpdateAtlasIndex(137, 10, 3, 3);
        bg.UpdateAtlasIndex(135, 11, 3, 3);
        bg.UpdateAtlasIndex(136, 11, 3, 3);
        bg.UpdateAtlasIndex(137, 11, 3, 3);
        bg.UpdateAtlasIndex(134, 12, 3, 3);
        bg.UpdateAtlasIndex(135, 12, 3, 3);
        bg.UpdateAtlasIndex(136, 12, 3, 3);
        bg.UpdateAtlasIndex(137, 12, 3, 3);
        
        // stair 2
        bg.UpdateAtlasIndex(140, 9, 3, 3);
        bg.UpdateAtlasIndex(140, 10, 3, 3);
        bg.UpdateAtlasIndex(141, 10, 3, 3);
        bg.UpdateAtlasIndex(140, 11, 3, 3);
        bg.UpdateAtlasIndex(141, 11, 3, 3);
        bg.UpdateAtlasIndex(142, 11, 3, 3);
        bg.UpdateAtlasIndex(140, 12, 3, 3);
        bg.UpdateAtlasIndex(141, 12, 3, 3);
        bg.UpdateAtlasIndex(142, 12, 3, 3);
        bg.UpdateAtlasIndex(143, 12, 3, 3);
        
        // stair 3
        bg.UpdateAtlasIndex(152, 9, 3, 3);
        bg.UpdateAtlasIndex(151, 9, 3, 3);
        bg.UpdateAtlasIndex(152, 10, 3, 3);
        bg.UpdateAtlasIndex(151, 10, 3, 3);
        bg.UpdateAtlasIndex(150, 10, 3, 3);
        bg.UpdateAtlasIndex(152, 11, 3, 3);
        bg.UpdateAtlasIndex(151, 11, 3, 3);
        bg.UpdateAtlasIndex(150, 11, 3, 3);
        bg.UpdateAtlasIndex(149, 11, 3, 3);
        bg.UpdateAtlasIndex(152, 12, 3, 3);
        bg.UpdateAtlasIndex(151, 12, 3, 3);
        bg.UpdateAtlasIndex(150, 12, 3, 3);
        bg.UpdateAtlasIndex(149, 12, 3, 3);
        bg.UpdateAtlasIndex(148, 12, 3, 3);
        
        // stair 4
        bg.UpdateAtlasIndex(155, 9, 3, 3);
        bg.UpdateAtlasIndex(155, 10, 3, 3);
        bg.UpdateAtlasIndex(156, 10, 3, 3);
        bg.UpdateAtlasIndex(155, 11, 3, 3);
        bg.UpdateAtlasIndex(156, 11, 3, 3);
        bg.UpdateAtlasIndex(157, 11, 3, 3);
        bg.UpdateAtlasIndex(155, 12, 3, 3);
        bg.UpdateAtlasIndex(156, 12, 3, 3);
        bg.UpdateAtlasIndex(157, 12, 3, 3);
        bg.UpdateAtlasIndex(158, 12, 3, 3);
        
        // stair 5
        bg.UpdateAtlasIndex(188, 5, 3, 3);
        bg.UpdateAtlasIndex(189, 5, 3, 3);
        bg.UpdateAtlasIndex(187, 6, 3, 3);
        bg.UpdateAtlasIndex(188, 6, 3, 3);
        bg.UpdateAtlasIndex(189, 6, 3, 3);
        bg.UpdateAtlasIndex(186, 7, 3, 3);
        bg.UpdateAtlasIndex(187, 7, 3, 3);
        bg.UpdateAtlasIndex(188, 7, 3, 3);
        bg.UpdateAtlasIndex(189, 7, 3, 3);
        bg.UpdateAtlasIndex(185, 8, 3, 3);
        bg.UpdateAtlasIndex(186, 8, 3, 3);
        bg.UpdateAtlasIndex(187, 8, 3, 3);
        bg.UpdateAtlasIndex(188, 8, 3, 3);
        bg.UpdateAtlasIndex(189, 8, 3, 3);
        bg.UpdateAtlasIndex(184, 9, 3, 3);
        bg.UpdateAtlasIndex(185, 9, 3, 3);
        bg.UpdateAtlasIndex(186, 9, 3, 3);
        bg.UpdateAtlasIndex(187, 9, 3, 3);
        bg.UpdateAtlasIndex(188, 9, 3, 3);
        bg.UpdateAtlasIndex(189, 9, 3, 3);
        bg.UpdateAtlasIndex(183, 10, 3, 3);
        bg.UpdateAtlasIndex(184, 10, 3, 3);
        bg.UpdateAtlasIndex(185, 10, 3, 3);
        bg.UpdateAtlasIndex(186, 10, 3, 3);
        bg.UpdateAtlasIndex(187, 10, 3, 3);
        bg.UpdateAtlasIndex(188, 10, 3, 3);
        bg.UpdateAtlasIndex(189, 10, 3, 3);
        bg.UpdateAtlasIndex(182, 11, 3, 3);
        bg.UpdateAtlasIndex(183, 11, 3, 3);
        bg.UpdateAtlasIndex(184, 11, 3, 3);
        bg.UpdateAtlasIndex(185, 11, 3, 3);
        bg.UpdateAtlasIndex(186, 11, 3, 3);
        bg.UpdateAtlasIndex(187, 11, 3, 3);
        bg.UpdateAtlasIndex(188, 11, 3, 3);
        bg.UpdateAtlasIndex(189, 11, 3, 3);
        bg.UpdateAtlasIndex(181, 12, 3, 3);
        bg.UpdateAtlasIndex(182, 12, 3, 3);
        bg.UpdateAtlasIndex(183, 12, 3, 3);
        bg.UpdateAtlasIndex(184, 12, 3, 3);
        bg.UpdateAtlasIndex(185, 12, 3, 3);
        bg.UpdateAtlasIndex(186, 12, 3, 3);
        bg.UpdateAtlasIndex(187, 12, 3, 3);
        bg.UpdateAtlasIndex(188, 12, 3, 3);
        bg.UpdateAtlasIndex(189, 12, 3, 3);
        
        // flagpole
        bg.UpdateAtlasIndex(198, 2, 2, 2);
        bg.UpdateAtlasIndex(198, 3, 2, 3);
        bg.UpdateAtlasIndex(198, 4, 2, 3);
        bg.UpdateAtlasIndex(198, 5, 2, 3);
        bg.UpdateAtlasIndex(198, 6, 2, 3);
        bg.UpdateAtlasIndex(198, 7, 2, 3);
        bg.UpdateAtlasIndex(198, 8, 2, 3);
        bg.UpdateAtlasIndex(198, 9, 2, 3);
        bg.UpdateAtlasIndex(198, 10, 2, 3);
        bg.UpdateAtlasIndex(198, 11, 2, 3);
        bg.UpdateAtlasIndex(198, 12, 3, 3);
        
        // castle
        bg.UpdateAtlasIndex(203, 8, 5, 2);
        bg.UpdateAtlasIndex(204, 8, 5, 2);
        bg.UpdateAtlasIndex(205, 8, 5, 2);
        bg.UpdateAtlasIndex(203, 9, 6, 2);
        bg.UpdateAtlasIndex(204, 9, 5, 3);
        bg.UpdateAtlasIndex(205, 9, 6, 3);
        bg.UpdateAtlasIndex(202, 10, 5, 2);
        bg.UpdateAtlasIndex(203, 10, 4, 2);
        bg.UpdateAtlasIndex(204, 10, 4, 2);
        bg.UpdateAtlasIndex(205, 10, 4, 2);
        bg.UpdateAtlasIndex(206, 10, 5, 2);
        bg.UpdateAtlasIndex(202, 11, 5, 3);
        bg.UpdateAtlasIndex(203, 11, 5, 3);
        bg.UpdateAtlasIndex(204, 11, 7, 2);
        bg.UpdateAtlasIndex(205, 11, 5, 3);
        bg.UpdateAtlasIndex(206, 11, 5, 3);
        bg.UpdateAtlasIndex(202, 12, 5, 3);
        bg.UpdateAtlasIndex(203, 12, 5, 3);
        bg.UpdateAtlasIndex(204, 12, 7, 3);
        bg.UpdateAtlasIndex(205, 12, 5, 3);
        bg.UpdateAtlasIndex(206, 12, 5, 3);
    }
}