using Kasane2D.Graphics.Interfaces;

namespace ParallaxDemo;

public abstract class BackgroundLayer
{
    private bool layerVisible = false;
    protected readonly ITilemapSurface surface;

    protected BackgroundLayer(ITilemapSurface surface)
    {
        this.surface = surface;
        Clear();
    }

    public void Init()
    {
        ShowLayer();
        layerVisible = true;
    }

    public void Toggle()
    {
        layerVisible = !layerVisible;
        if (layerVisible)
        {
            ShowLayer();
            return;
        }

        Clear();
    }

    protected abstract void ShowLayer();

    private void Clear()
    {
        for (var x = 0; x < surface.Dimensions.X; x++)
        {
            for (var y = 0; y < surface.Dimensions.Y; y++)
            {
                surface.UpdateAtlasIndex(x, y, 0, 0);
            }
        }
    }
}

public class Layer0 : BackgroundLayer
{
    public Layer0(ITilemapSurface surface) : base(surface)
    {
    }

    protected override void ShowLayer()
    {
        for (var x = 0; x < surface.Dimensions.X; x++)
        {
            for (var y = 3; y < surface.Dimensions.Y; y++)
            {
                surface.UpdateAtlasIndex(x, y, 3, 3);
            }
        }

        // stars
        surface.UpdateAtlasIndex(5, 3, 3, 1);
        surface.UpdateAtlasIndex(29, 4, 3, 1);
        surface.UpdateAtlasIndex(43, 3, 3, 1);

        // seashells
        surface.UpdateAtlasIndex(12, 4, 3, 2);
        surface.UpdateAtlasIndex(36, 3, 3, 2);

        // fish
        surface.UpdateAtlasIndex(2, 0, 7, 6);
        surface.UpdateAtlasIndex(38, 1, 7, 11);
        
        // ridge
        surface.UpdateAtlasIndex(0, 2, 2, 1);
        surface.UpdateAtlasIndex(1, 2, 2, 3);
        surface.UpdateAtlasIndex(2, 2, 1, 9);
        surface.UpdateAtlasIndex(3, 2, 1, 11);
        surface.UpdateAtlasIndex(4, 2, 2, 3);
        surface.UpdateAtlasIndex(5, 2, 2, 9);
        surface.UpdateAtlasIndex(6, 2, 2, 11);
        surface.UpdateAtlasIndex(7, 2, 2, 3);
        surface.UpdateAtlasIndex(8, 2, 2, 1);
        surface.UpdateAtlasIndex(9, 2, 2, 9);
        surface.UpdateAtlasIndex(10, 2, 2, 11);
        surface.UpdateAtlasIndex(11, 2, 1, 11);
        surface.UpdateAtlasIndex(12, 2, 2, 3);
        surface.UpdateAtlasIndex(13, 2, 2, 1);
        surface.UpdateAtlasIndex(14, 2, 2, 9);
        surface.UpdateAtlasIndex(15, 2, 2, 11);
        surface.UpdateAtlasIndex(16, 2, 1, 9);
        surface.UpdateAtlasIndex(17, 2, 2, 11);
        surface.UpdateAtlasIndex(18, 2, 2, 9);
        surface.UpdateAtlasIndex(19, 2, 1, 9);
        surface.UpdateAtlasIndex(20, 2, 2, 3);
        surface.UpdateAtlasIndex(21, 2, 2, 1);
        surface.UpdateAtlasIndex(22, 2, 2, 9);
        surface.UpdateAtlasIndex(23, 2, 2, 11);
        surface.UpdateAtlasIndex(24, 2, 1, 11);
        surface.UpdateAtlasIndex(25, 2, 1, 9);
        surface.UpdateAtlasIndex(26, 2, 2, 3);
        surface.UpdateAtlasIndex(27, 2, 2, 1);
        surface.UpdateAtlasIndex(28, 2, 2, 11);
        surface.UpdateAtlasIndex(29, 2, 2, 3);
        surface.UpdateAtlasIndex(30, 2, 2, 1);
        surface.UpdateAtlasIndex(31, 2, 2, 3);
        surface.UpdateAtlasIndex(32, 2, 2, 1);
        surface.UpdateAtlasIndex(33, 2, 2, 9);
        surface.UpdateAtlasIndex(34, 2, 2, 11);
        surface.UpdateAtlasIndex(35, 2, 1, 9);
        surface.UpdateAtlasIndex(36, 2, 2, 11);
        surface.UpdateAtlasIndex(37, 2, 1, 11);
        surface.UpdateAtlasIndex(38, 2, 2, 3);
        surface.UpdateAtlasIndex(39, 2, 2, 1);
        surface.UpdateAtlasIndex(40, 2, 2, 3);
        surface.UpdateAtlasIndex(41, 2, 2, 1);
        surface.UpdateAtlasIndex(42, 2, 2, 9);
        surface.UpdateAtlasIndex(43, 2, 2, 11);
        surface.UpdateAtlasIndex(44, 2, 1, 11);
        surface.UpdateAtlasIndex(45, 2, 2, 3);
        surface.UpdateAtlasIndex(46, 2, 2, 9);
        surface.UpdateAtlasIndex(47, 2, 2, 3);
    }
}

public class Layer1 : BackgroundLayer
{
    public Layer1(ITilemapSurface surface) : base(surface)
    {
    }

    protected override void ShowLayer()
    {
        for (var x = 0; x < surface.Dimensions.X; x++)
        {
            for (var y = 6; y < surface.Dimensions.Y; y++)
            {
                surface.UpdateAtlasIndex(x, y, 1, 7);
            }
        }
        
        // stars
        surface.UpdateAtlasIndex(3, 7, 1, 5);
        surface.UpdateAtlasIndex(20, 8, 1, 5);
        surface.UpdateAtlasIndex(38, 7, 1, 5);
        
        // seashells
        surface.UpdateAtlasIndex(9, 6, 1, 6);
        surface.UpdateAtlasIndex(30, 6, 1, 6);
        
        // ridge
        surface.UpdateAtlasIndex(0, 5, 1, 3);
        surface.UpdateAtlasIndex(1, 5, 0, 7);
        surface.UpdateAtlasIndex(2, 5, 0, 5);
        surface.UpdateAtlasIndex(3, 5, 0, 7);
        surface.UpdateAtlasIndex(4, 5, 0, 1);
        surface.UpdateAtlasIndex(5, 5, 1, 3);
        surface.UpdateAtlasIndex(6, 5, 0, 7);
        surface.UpdateAtlasIndex(7, 5, 0, 3);
        surface.UpdateAtlasIndex(8, 5, 0, 1);
        surface.UpdateAtlasIndex(9, 5, 0, 7);
        surface.UpdateAtlasIndex(10, 5, 0, 3);
        surface.UpdateAtlasIndex(11, 5, 1, 3);
        surface.UpdateAtlasIndex(12, 5, 0, 3);
        surface.UpdateAtlasIndex(13, 5, 1, 3);
        surface.UpdateAtlasIndex(14, 5, 0, 7);
        surface.UpdateAtlasIndex(15, 5, 0, 1);
        surface.UpdateAtlasIndex(16, 5, 0, 7);
        surface.UpdateAtlasIndex(17, 5, 0, 3);
        surface.UpdateAtlasIndex(18, 5, 0, 1);
        surface.UpdateAtlasIndex(19, 5, 1, 3);
        surface.UpdateAtlasIndex(20, 5, 1, 3);
        surface.UpdateAtlasIndex(21, 5, 1, 3);
        surface.UpdateAtlasIndex(22, 5, 0, 7);
        surface.UpdateAtlasIndex(23, 5, 0, 1);
        surface.UpdateAtlasIndex(24, 5, 1, 3);
        surface.UpdateAtlasIndex(25, 5, 0, 3);
        surface.UpdateAtlasIndex(26, 5, 0, 1);
        surface.UpdateAtlasIndex(27, 5, 0, 7);
        surface.UpdateAtlasIndex(28, 5, 0, 3);
        surface.UpdateAtlasIndex(29, 5, 0, 3);
        surface.UpdateAtlasIndex(30, 5, 1, 3);
        surface.UpdateAtlasIndex(31, 5, 0, 7);
        surface.UpdateAtlasIndex(32, 5, 1, 3);
        surface.UpdateAtlasIndex(33, 5, 0, 1);
        surface.UpdateAtlasIndex(34, 5, 1, 3);
        surface.UpdateAtlasIndex(35, 5, 0, 1);
        surface.UpdateAtlasIndex(36, 5, 1, 3);
        surface.UpdateAtlasIndex(37, 5, 0, 3);
        surface.UpdateAtlasIndex(38, 5, 1, 3);
        surface.UpdateAtlasIndex(39, 5, 0, 7);
        surface.UpdateAtlasIndex(40, 5, 0, 1);
        surface.UpdateAtlasIndex(41, 5, 0, 7);
        surface.UpdateAtlasIndex(42, 5, 1, 3);
        surface.UpdateAtlasIndex(43, 5, 0, 7);
        surface.UpdateAtlasIndex(44, 5, 0, 3);
        surface.UpdateAtlasIndex(45, 5, 1, 3);
        surface.UpdateAtlasIndex(46, 5, 0, 7);
        surface.UpdateAtlasIndex(47, 5, 0, 1);
    }
}

public class Layer2 : BackgroundLayer
{
    public Layer2(ITilemapSurface surface) : base(surface)
    {
    }

    protected override void ShowLayer()
    {
        // fossils
        surface.UpdateAtlasIndex(2, 6, 7, 8);
        surface.UpdateAtlasIndex(26, 7, 6, 12);
        
        // ground
        surface.UpdateAtlasIndex(1, 8, 4, 11);
        surface.UpdateAtlasIndex(5, 8, 3, 11);
        surface.UpdateAtlasIndex(9, 8, 3, 10);
        surface.UpdateAtlasIndex(12, 8, 4, 8);
        surface.UpdateAtlasIndex(14, 8, 3, 5);
        surface.UpdateAtlasIndex(16, 8, 4, 11);
        surface.UpdateAtlasIndex(19, 8, 5, 8);
        surface.UpdateAtlasIndex(23, 8, 3, 11);
        surface.UpdateAtlasIndex(28, 8, 3, 5);
        surface.UpdateAtlasIndex(32, 8, 4, 10);
        surface.UpdateAtlasIndex(36, 8, 4, 11);
        surface.UpdateAtlasIndex(38, 8, 3, 5);
        surface.UpdateAtlasIndex(42, 8, 4, 11);
        surface.UpdateAtlasIndex(45, 8, 5, 5);
    }
}