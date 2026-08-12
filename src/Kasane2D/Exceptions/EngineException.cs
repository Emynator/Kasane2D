namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all exceptions originating in the core engine.
/// </summary>
public abstract class EngineException : KasaneException
{
    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected EngineException(string message) : base(message)
    {
    }

    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    protected EngineException(string message, Exception innerException) : base(message, innerException)
    {
    }
}