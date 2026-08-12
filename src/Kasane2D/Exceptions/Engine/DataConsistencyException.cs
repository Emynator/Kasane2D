namespace Kasane2D.Exceptions.Engine;

/// <summary>
/// Exception that is thrown when invalid or malformed data prevents a well-defined operation state.
/// </summary>
public class DataConsistencyException : EngineException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DataConsistencyException(string message) : base(message)
    {
    }

    /// <summary>
    /// Create a new exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DataConsistencyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}