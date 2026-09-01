using Kasane2D.Exceptions.Engine;
using Kasane2D.Music.Enums;
using Kasane2D.Music.Types;
using Kasane2D.Music.Types.SequenceEvents;

namespace Kasane2D.Music.Extensions;

internal static class SongPatternExtensions
{
    public static ProcessedSong ProcessSong(this Song song, int samplerate)
    {
        var processedPatterns = new Dictionary<string, ProcessedSongPattern>();
        foreach (var pattern in song.Patterns)
        {
            processedPatterns[pattern.Key] = pattern.Value.ProcessPattern(samplerate);
        }

        return new(song.Name, processedPatterns, song.Sections);
    }
    
    public static ProcessedSongPattern ProcessPattern(this SongPattern pattern, int samplerate)
    {
        var sequenceSteps = pattern.StepSize.GetSequenceSteps();
        var barSteps = pattern.TimeSignature.GetSequenceStepsPerBar();
        var samplesPerStep = pattern.TimeSignature.GetSamplesPerStep(samplerate, pattern.Bpm);
        var sequences = new Dictionary<string, Sequence>();
        foreach (var trackPattern in pattern.TrackPatterns)
        {
            sequences.Add
            (
                trackPattern.TrackName,
                CreateSequence(trackPattern, pattern.Length, sequenceSteps, barSteps)
            );
        }

        return new(pattern.Name, pattern.Length * barSteps, samplesPerStep, sequences);
    }

    private static Sequence CreateSequence(TrackPattern pattern, int length, int sequenceSteps, int barSteps)
    {
        var sequenceLength = length * barSteps;
        if (pattern.ControlEvents.Length != sequenceLength)
        {
            throw new DataConsistencyException
                ($"Length of {nameof(pattern.ControlEvents)} must match the sequence length.");
        }

        var barNotes = barSteps / sequenceSteps;
        var noteCount = barNotes * length;
        if (pattern.NoteEvents.Count != noteCount)
        {
            throw new DataConsistencyException
                ($"Length of {nameof(pattern.NoteEvents)} must match the sequence length.");
        }

        var sequenceNoteEvents = new SequenceNoteEvent[sequenceLength];
        var patternNoteEvents = pattern.NoteEvents.ToArray();
        var patternStep = 0;
        var sequenceStep = 0;
        var noteFill = new SequenceNoteEvent();
        var processNext = true;
        for (var i = 0; i < sequenceLength; i++)
        {
            if (i == sequenceLength - 1 && noteFill.Kind is SequenceNoteEventKind.Hold)
            {
                sequenceNoteEvents[i] = new(Kind: SequenceNoteEventKind.Release);
                continue;
            }

            if (processNext)
            {
                var noteEvent = patternNoteEvents[patternStep];
                noteFill = noteEvent.Kind is NoteEventKind.Begin or NoteEventKind.Hold
                    ? new SequenceNoteEvent(Kind: SequenceNoteEventKind.Hold)
                    : new SequenceNoteEvent(Kind: SequenceNoteEventKind.Off);

                var kind = noteEvent.Kind switch
                {
                    NoteEventKind.Begin => SequenceNoteEventKind.Begin,
                    NoteEventKind.Hold => SequenceNoteEventKind.Hold,
                    _ => SequenceNoteEventKind.Off,
                };
                sequenceNoteEvents[i] = new(noteEvent.Note, kind);

                if (
                    i > 0
                    && noteEvent.Kind is NoteEventKind.Begin or NoteEventKind.None
                    && sequenceNoteEvents[i - 1].Kind != SequenceNoteEventKind.Off
                    )
                {
                    sequenceNoteEvents[i - 1].Kind = SequenceNoteEventKind.Release;
                }

                processNext = false;
            }
            else
            {
                sequenceNoteEvents[i] = noteFill;
            }

            sequenceStep++;
            if (sequenceStep < sequenceSteps)
            {
                continue;
            }

            patternStep++;
            sequenceStep = 0;
            processNext = true;
        }

        return new
        (
            new
            (
                VolumeUpdate: pattern.InitialSettings.VolumeUpdate,
                PanUpdate: pattern.InitialSettings.PanUpdate,
                EnvelopeUpdate: pattern.InitialSettings.EnvelopeUpdate,
                EffectUpdates: pattern.InitialSettings.EffectUpdates,
                GeneratorUpdate: pattern.InitialSettings.GeneratorUpdate
            ),
            sequenceNoteEvents,
            pattern.ControlEvents
        );
    }
}