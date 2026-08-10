namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all Kasane2D specific exceptions.
/// </summary>
public abstract class KasaneException : Exception
{
    protected KasaneException(string message) : base(message)
    {
    }

    protected KasaneException(string message, Exception inner) : base(message, inner)
    {
    }
}