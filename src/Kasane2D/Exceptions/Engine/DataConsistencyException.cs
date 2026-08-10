namespace Kasane2D.Exceptions.Engine;

/// <summary>
/// Exception that is thrown when invalid or malformed data prevents a well-defined operation state.
/// </summary>
public class DataConsistencyException : EngineException
{
    public DataConsistencyException(string message) : base(message)
    {
    }

    public DataConsistencyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}