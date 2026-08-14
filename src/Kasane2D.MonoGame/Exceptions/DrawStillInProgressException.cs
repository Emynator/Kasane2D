using Kasane2D.Exceptions;
using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.MonoGame.Exceptions;

/// <summary>
/// Exception that is thrown when the main rasterizer encounters that drawing is still in process.
/// </summary>
public sealed class DrawStillInProgressException : BackendException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    public DrawStillInProgressException() : base
        ($"Cannot draw when drawing is still in progress. Missing call to '{nameof(IRenderer.EndDraw)}'?")
    {
    }
}