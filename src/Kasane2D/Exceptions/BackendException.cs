namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all exceptions specific to the engine backend.
/// </summary>
public abstract class BackendException : KasaneException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected BackendException(string message) : base(message)
    {
    }

    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The inner exception.</param>
    protected BackendException(string message, Exception inner) : base(message, inner)
    {
    }
}