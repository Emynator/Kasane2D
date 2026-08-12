namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

/// <summary>
/// Represents an update to the track's pan.
/// </summary>
public readonly record struct PanUpdate
{
    /// <summary>
    /// Creates an empty update that changes nothing.
    /// </summary>
    public PanUpdate()
    {
        DoUpdate = false;
    }

    /// <summary>
    /// Creates an update that changes the track's pan.
    /// </summary>
    /// <param name="value">The pan value.</param>
    public PanUpdate(int value)
    {
        DoUpdate = true;
        Value = value;
    }

    internal bool DoUpdate { get; init; }

    /// <summary>
    /// The pan value.
    /// </summary>
    public int Value { get; init; }
}