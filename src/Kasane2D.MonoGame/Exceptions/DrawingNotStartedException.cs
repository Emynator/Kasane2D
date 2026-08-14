using Kasane2D.Exceptions;
using Kasane2D.Graphics.Interfaces;

namespace Kasane2D.MonoGame.Exceptions;

/// <summary>
/// Exception that is thrown when attempting to use a draw function before calling <see cref="IRenderer.BeginDraw"/>
/// </summary>
public sealed class DrawingNotStartedException : BackendException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    public DrawingNotStartedException() : base($"Drawing requires a call to '{nameof(IRenderer.BeginDraw)}' first.")
    {
    }
}