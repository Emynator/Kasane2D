namespace Kasane2D.Exceptions.Backend;

/// <summary>
/// Exception that is thrown when the implementation of an interface originates from a different backend implementation.
/// </summary>
public sealed class IncompatibleBackendDataException : BackendException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    /// <param name="name">The name of the incompatible type.</param>
    public IncompatibleBackendDataException(string name) : base($"The type of '{name}' was created with a different backend.")
    {
    }
}