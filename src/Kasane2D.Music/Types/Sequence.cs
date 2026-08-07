namespace Kasane2D.Music.Types;

internal record class Sequence
{
    public static Sequence Empty(int length)
    {
        return new Sequence
        (
            new SequenceNoteEvent[length * Constants.SequencerStepsPerQuarterNote * 4],
            new SequenceControlEvent[length * Constants.SequencerStepsPerQuarterNote * 4]
        );
    }

    public Sequence(SequenceNoteEvent[] Notes, SequenceControlEvent[] ControlEvents)
    {
        if (Notes.Length != ControlEvents.Length)
        {
            throw new ArgumentException("Sequence contains unequal number of notes and control events.");
        }

        Length = Notes.Length;
        this.Notes = Notes;
        this.ControlEvents = ControlEvents;
    }

    public int Length { get; init; }

    public SequenceNoteEvent[] Notes { get; init; }

    public SequenceControlEvent[] ControlEvents { get; init; }
}