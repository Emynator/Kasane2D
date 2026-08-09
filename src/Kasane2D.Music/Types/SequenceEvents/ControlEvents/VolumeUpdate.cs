namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

public readonly record struct VolumeUpdate
{
    public VolumeUpdate()
    {
        DoUpdate = false;
        Value = 0;
    }

    public VolumeUpdate(int value)
    {
        DoUpdate = true;
        Value = value;
    }

    public bool DoUpdate { get; init; }

    public int Value { get; init; }
}