using Kasane2D.Graphics.Types;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// Represents a single texture.
/// </summary>
public interface ITexture
{
    /// <summary>
    /// Width and Height of the texture in pixels.
    /// </summary>
    public Vec2I Size { get; }

    /// <summary>
    /// Gets the pixel data of the texture.
    /// </summary>
    /// <returns>2D array of the pixel data with [x, y] indices.</returns>
    public Color[,] GetData();

    /// <summary>
    /// Manually sets the pixel data of the texture.
    /// </summary>
    /// <param name="data">The 2D array of the pixel data with [x, y] indices.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if data does not contain the exact amount of pixels for
    /// the texture.</exception>
    public void SetData(Color[,] data);
}