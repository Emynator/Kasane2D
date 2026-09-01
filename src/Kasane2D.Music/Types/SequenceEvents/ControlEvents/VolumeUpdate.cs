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
        Value = 0.0f;
    }

    /// <summary>
    /// Creates an update that changes the track's volume.
    /// </summary>
    /// <param name="value">The volume in dBFS.</param>
    public VolumeUpdate(float value)
    {
        DoUpdate = true;
        Value = value;
    }

    internal bool DoUpdate { get; }

    /// <summary>
    /// The volume in dBFS.
    /// </summary>
    public float Value { get; }
}