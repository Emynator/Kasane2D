namespace Kasane2D.Graphics.Interfaces;

public interface IGraphicsBackend
{
    public IRasterizer InitializeRasterizer(GraphicsConfiguration config);
}