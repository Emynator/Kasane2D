using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class SynthEngine : ISynthEngine
{
    private readonly int samplerate;
    private readonly Dictionary<string, Sequencer> tracks;
    private readonly SemaphoreSlim tlock = new(1, 1);
    private bool isPlaying = false;
    private int currentStep = 0;
    private int carryOverSamples = 0;
    private ProcessedPattern? currentPattern = null;
    private ProcessedPattern? nextPattern = null;

    public SynthEngine(int samplerate, Dictionary<string, Sequencer> tracks)
    {
        this.samplerate = samplerate;
        this.tracks = tracks;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public void Process(int sampleCount)
    {
        tlock.Wait();

        if (currentPattern is null || !isPlaying)
        {
            ProcessTracks(sampleCount);
            tlock.Release();
            
            return;
        }

        var samplesToProcess = sampleCount;
        if (carryOverSamples > 0)
        {
            if (carryOverSamples > sampleCount)
            {
                ProcessTracks(sampleCount);
                carryOverSamples -= sampleCount;
                tlock.Release();

                return;
            }

            ProcessTracks(carryOverSamples);
            samplesToProcess -= carryOverSamples;
            carryOverSamples = 0;
        }

        var steps = samplesToProcess / currentPattern.Value.SamplesPerStep;
        var remainingSamples = samplesToProcess - steps * currentPattern.Value.SamplesPerStep;
        for (var i = 0; i < steps; i++)
        {
            Step();
            ProcessTracks(currentPattern.Value.SamplesPerStep);
        }

        if (remainingSamples == 0)
        {
            tlock.Release();
            
            return;
        }

        Step();
        ProcessTracks(remainingSamples);
        carryOverSamples = currentPattern.Value.SamplesPerStep - remainingSamples;

        tlock.Release();
    }

    public void Play(SongPattern pattern)
    {
        tlock.Wait();

        foreach (var track in tracks.Values)
        {
            track.Reset();
        }
        
        nextPattern = null;
        currentPattern = ProcessPattern(pattern);
        foreach (var seq in currentPattern.Value.Sequences)
        {
            if (!tracks.TryGetValue(seq.Key, out var track))
            {
                continue;
            }

            track.NextSequence = null;
            track.CurrentSequence = seq.Value;
        }

        isPlaying = true;
        
        tlock.Release();
    }

    public void Queue(SongPattern pattern)
    {
        tlock.Wait();
        
        nextPattern = ProcessPattern(pattern);
        foreach (var seq in nextPattern.Value.Sequences)
        {
            if (!tracks.TryGetValue(seq.Key, out var track))
            {
                continue;
            }
            
            track.NextSequence = seq.Value;
        }
        
        tlock.Release();
    }

    public void Pause()
    {
        tlock.Wait();

        isPlaying = false;
        foreach (var track in tracks.Values)
        {
            track.Reset();
        }
        
        tlock.Release();
    }

    public void Resume()
    {
        tlock.Wait();
        
        isPlaying = true;

        tlock.Release();
    }

    public void Stop()
    {
        tlock.Wait();
        
        isPlaying = false;
        currentPattern = null;
        nextPattern = null;
        foreach (var track in tracks.Values)
        {
            track.CurrentSequence = null;
            track.NextSequence = null;
            track.Reset();
        }
        
        tlock.Release();
    }

    private ProcessedPattern ProcessPattern(SongPattern pattern)
    {
        var sequenceSteps = pattern.StepSize.GetSequenceSteps();
        var barSteps = pattern.TimeSignature.GetSequenceStepsPerBar();
        var samplesPerStep = pattern.TimeSignature.GetSamplesPerStep(samplerate, pattern.Bpm);
        var sequences = new Dictionary<string, Sequence>();
        foreach (var trackPattern in pattern.TrackPatterns)
        {
            sequences.Add(trackPattern.TrackName, CreateSequence(trackPattern, pattern.Length, sequenceSteps, barSteps));
        }

        return new(pattern.Length * barSteps, samplesPerStep, sequences);
    }

    private Sequence CreateSequence(TrackPattern pattern, int length, int sequenceSteps, int barSteps)
    {
        var sequenceNoteEvents = new SequenceNoteEvent[length * barSteps];
        var sequenceControlEvents = new SequenceControlEvent[length * barSteps];
        for (var i = 0; i < length; i++)
        {
            var noteEventsSlice = sequenceNoteEvents.AsSpan().Slice(i * barSteps, barSteps);
            var controlEventsSlice = sequenceControlEvents.AsSpan().Slice(i * barSteps, barSteps);
            var noteEvents = pattern.NoteEvents.Where(ev => ev.Bar == i).ToList();
            var controlEvents = pattern.ControlEvents.Where(ev => ev.Bar == i).ToList();

            ProcessBar(noteEventsSlice, controlEventsSlice, noteEvents, controlEvents, sequenceSteps, barSteps);
        }

        return new(sequenceNoteEvents, sequenceControlEvents);
    }

    private void ProcessBar
        (
        Span<SequenceNoteEvent> sequenceNoteEvents,
        Span<SequenceControlEvent> sequenceControlEvents,
        List<NoteEvent> noteEvents,
        List<ControlEvent> controlEvents,
        int sequenceSteps,
        int barSteps
        )
    {
        var patternStep = 0;
        var sequenceStep = 0;
        var noteFill = new SequenceNoteEvent();
        var controlFill = new SequenceControlEvent();
        var processNext = true;
        for (var i = 0; i < barSteps; i++)
        {
            if (i == barSteps - 1 && noteFill.Kind is SequenceNoteEventKind.Hold)
            {
                sequenceNoteEvents[i] = new(Kind: SequenceNoteEventKind.Release);
                continue;
            }

            if (processNext)
            {
                var noteEvent = noteEvents.FirstOrDefault(ev => ev.Step == patternStep);
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
                sequenceControlEvents[i] = controlFill;
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
    }

    private void ProcessTracks(int sampleCount)
    {
        Parallel.ForEach(tracks.Values, track => track.Process(sampleCount));
    }

    private void Step()
    {
        Parallel.ForEach(tracks.Values, track => track.Step(currentStep));

        currentStep++;
        if (currentStep < currentPattern?.PatternLength)
        {
            return;
        }

        currentStep = 0;
        Parallel.ForEach(tracks.Values, track => track.Next());
        if (nextPattern is not null)
        {
            currentPattern = nextPattern;
        }
    }

    private readonly record struct ProcessedPattern
        (
        int PatternLength,
        int SamplesPerStep,
        Dictionary<string, Sequence> Sequences
        );
}