namespace Kasane2D.Exceptions.Backend;

/// <summary>
/// Exception that is thrown when the implementation of an interface originates from a different backend implementation.
/// </summary>
public sealed class IncompatibleBackendDataException : BackendException
{
    public IncompatibleBackendDataException(string name) : base($"The type of 'name' was created with a different backend.")
    {
    }
}