using Kasane2D.Music.Types.SequenceEvents;

namespace Kasane2D.Music.Types;

internal record class Sequence
{
    public Sequence(SequenceControlEvent initialSettings, SequenceNoteEvent[] notes, SequenceControlEvent[] controlEvents)
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
    
    public SequenceControlEvent InitialSettings { get; init; }

    public SequenceNoteEvent[] Notes { get; init; }

    public SequenceControlEvent[] ControlEvents { get; init; }
}