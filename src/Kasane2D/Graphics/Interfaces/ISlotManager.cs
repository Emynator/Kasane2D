using System.Diagnostics.CodeAnalysis;
using Kasane2D.Graphics.Types;

namespace Kasane2D.Graphics.Interfaces;

/// <summary>
/// Slot manager that assists with managing sprite slots of a sprite layer.
/// </summary>
public interface ISlotManager
{
    /// <summary>
    /// Gets the sprite layer this manager belongs to.
    /// </summary>
    public ISpriteLayer SpriteLayer { get; }

    /// <summary>
    /// Requests an available sprite slot.
    /// </summary>
    /// <param name="result">The requested sprite slot.</param>
    /// <returns>True when a sprite slot is available and returned. False when no free sprite slot is available.</returns>
    /// <remarks>The returned sprite is marked as inactive. Values of all other fields are undefined and may contain
    /// old data if the slot has been used before.</remarks>
    public bool GetSlot([NotNullWhen(true)] out SpriteSlot? result);

    /// <summary>
    /// Returns a sprite slot to the pool and marks it as available.
    /// </summary>
    /// <param name="slot">The slot to return.</param>
    /// <remarks>Also sets the underlying sprite as inactive so it is no longer drawn.</remarks>
    public void FreeSlot(SpriteSlot slot);
}