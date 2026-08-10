namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all exceptions specific to the engine backend.
/// </summary>
public abstract class BackendException : KasaneException
{
    protected BackendException(string message) : base(message)
    {
    }

    protected BackendException(string message, Exception inner) : base(message, inner)
    {
    }
}