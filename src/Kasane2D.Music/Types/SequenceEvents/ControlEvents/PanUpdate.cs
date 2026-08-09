namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

public readonly record struct PanUpdate
{
    public PanUpdate()
    {
        DoUpdate = false;
        Value = 0;
    }

    public PanUpdate(int value)
    {
        DoUpdate = true;
        Value = value;
    }

    public bool DoUpdate { get; init; }

    public int Value { get; init; }
}