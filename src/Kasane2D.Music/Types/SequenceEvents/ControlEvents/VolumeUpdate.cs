namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

/// <summary>
/// Represents an update to the track's volume.
/// </summary>
public readonly record struct VolumeUpdate
{
    /// <summary>
    /// Creates an empty update that changes nothing.
    /// </summary>
    public VolumeUpdate()
    {
        DoUpdate = false;
        Value = 0;
    }

    /// <summary>
    /// Creates an update that changes the track's volume.
    /// </summary>
    /// <param name="value">The volume in dbFS.</param>
    public VolumeUpdate(int value)
    {
        DoUpdate = true;
        Value = value;
    }

    internal bool DoUpdate { get; init; }

    /// <summary>
    /// The volume in dbFS.
    /// </summary>
    public int Value { get; init; }
}