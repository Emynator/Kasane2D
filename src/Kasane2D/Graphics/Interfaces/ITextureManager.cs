using Kasane2D.Exceptions.Engine;
using Kasane2D.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// A manager to manage texture resources.
/// </summary>
public interface ITextureManager
{
    /// <summary>
    /// Creates an empty texture of the provided size. 
    /// </summary>
    /// <param name="size">Width and height of the texture in pixels.</param>
    /// <returns>The created texture.</returns>
    /// <remarks>Textures need to be deallocated with <see cref="FreeTexture"/> when no longer needed. Otherwise,
    /// this leads to memory leaks.</remarks>
    public ITexture CreateTexture(Vec2I size);
    
    /// <summary>
    /// Creates a texture from an image file.
    /// </summary>
    /// <param name="filePath">The path to the texture file.</param>
    /// <returns>The created texture.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the provided file does not exist.</exception>
    /// <remarks>Textures need to be deallocated with <see cref="FreeTexture"/> when no longer needed. Otherwise,
    /// this leads to memory leaks.</remarks>
    public ITexture CreateTexture(string filePath);
    
    /// <summary>
    /// Deallocates a texture.
    /// </summary>
    /// <param name="texture">The texture to deallocate.</param>
    public void FreeTexture(ITexture texture);
    
    /// <summary>
    /// Creates a sprite atlas with an empty texture of the provided dimensions.
    /// </summary>
    /// <param name="dimensions">Number of rows and columns of the atlas.</param>
    /// <param name="spriteSize">Width and height of a single sprite in pixels.</param>
    /// <returns>The created sprite atlas.</returns>
    /// <remarks>Just like textures, sprite atlases need to be deallocated with <see cref="FreeSpriteAtlas"/> when no
    /// longer needed. Otherwise, this leads to memory leaks.</remarks>
    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize);
    
    /// <summary>
    /// Creates a sprite atlas from an image file.
    /// </summary>
    /// <param name="spriteSize">Width and height of a single sprite in pixels.</param>
    /// <param name="filePath">The path to the texture file.</param>
    /// <returns>The created sprite atlas.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the provided file does not exist.</exception>
    /// <remarks>Just like textures, sprite atlases need to be deallocated with <see cref="FreeSpriteAtlas"/> when no
    /// longer needed. Otherwise, this leads to memory leaks.
    /// The number of rows and columns in the atlas is automatically determined from the loaded texture image
    /// by using the provided size of a single sprite.</remarks>
    public ISpriteAtlas CreateSpriteAtlas(Vec2I spriteSize, string filePath);
    
    /// <summary>
    /// Creates a sprite atlas from an image file with specified atlas dimensions.
    /// </summary>
    /// <param name="dimensions">Number of rows and columns of the atlas.</param>
    /// <param name="spriteSize">Width and height of a single sprite in pixels.</param>
    /// <param name="filePath">The path to the texture file.</param>
    /// <returns>The created sprite atlas.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the provided file does not exist.</exception>
    /// <exception cref="DataConsistencyException">Thrown when the provided image file is not large enough for the
    /// requested atlas dimensions.</exception>
    /// <remarks>Just like textures, sprite atlases need to be deallocated with <see cref="FreeSpriteAtlas"/> when no
    /// longer needed. Otherwise, this leads to memory leaks.
    /// This function sets the dimensions of the atlas regardless of the size of the loaded texture file.</remarks>
    public ISpriteAtlas CreateSpriteAtlas(Vec2I dimensions, Vec2I spriteSize, string filePath);
    
    /// <summary>
    /// Deallocates a sprite atlas.
    /// </summary>
    /// <param name="atlas">The sprite atlas to deallocate.</param>
    /// <remarks>Just like textures, sprite atlases need to be deallocated with <see cref="FreeSpriteAtlas"/> when no
    /// longer needed. Otherwise, this leads to memory leaks.</remarks>
    public void FreeSpriteAtlas(ISpriteAtlas atlas);
}