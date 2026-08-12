using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;

namespace Kasane2D.Music.Types;

internal record class Sequence
{
    public Sequence(ControlEvent initialSettings, SequenceNoteEvent[] notes, ControlEvent[] controlEvents)
    {
        if (notes.Length != controlEvents.Length)
        {
            throw new ArgumentException("Sequence contains unequal number of notes and control events.");
        }

        Length = notes.Length;
        InitialSettings = initialSettings;
        Notes = notes;
        ControlEvents = controlEvents;
    }

    public int Length { get; init; }
    
    public ControlEvent InitialSettings { get; init; }

    public SequenceNoteEvent[] Notes { get; init; }

    public ControlEvent[] ControlEvents { get; init; }
}