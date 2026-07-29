using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ITexture
{
    public Vec2I Size { get; }

    public Color[,] GetData();

    public void SetData(Color[,] data);
}