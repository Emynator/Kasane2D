namespace Kasane2D.Exceptions;

/// <summary>
/// Abstract base class of all exceptions originating in the core engine.
/// </summary>
public abstract class EngineException : KasaneException
{
    protected EngineException(string message) : base(message)
    {
    }

    protected EngineException(string message, Exception innerException) : base(message, innerException)
    {
    }
}