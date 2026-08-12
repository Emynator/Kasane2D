namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all Kasane2D specific exceptions.
/// </summary>
public abstract class KasaneException : Exception
{
    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected KasaneException(string message) : base(message)
    {
    }

    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The inner exception.</param>
    protected KasaneException(string message, Exception inner) : base(message, inner)
    {
    }
}