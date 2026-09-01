using Kasane2D.Exceptions.Engine;
using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;
using Kasane2D.Music.Types.SequenceEvents;

namespace Kasane2D.Music.Synthesis;

internal class SynthEngine : ISynthEngine
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly string systemKey;
    private readonly int samplerate;
    private readonly int bufferSize;
    private readonly Dictionary<string, Sequencer> tracks;
    private bool isPlaying = false;
    private int currentStep = 0;
    private int carryOverSamples = 0;
    private ProcessedPattern? currentPattern = null;
    private ProcessedPattern? nextPattern = null;

    public SynthEngine(string name, int samplerate, int bufferSize, Dictionary<string, Sequencer> tracks)
    {
        systemKey = $"MusicSystem::SynthEngine::{name}::Process";
        this.samplerate = samplerate;
        this.bufferSize = bufferSize;
        this.tracks = tracks;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public Conductor? InternalConductor { get; set; }

    public IConductor? Conductor => InternalConductor;

    public void Process()
    {
        tlock.Wait();
        Engine.Monitor.StartMeasurement(systemKey);

        if (currentPattern is null || !isPlaying)
        {
            ProcessTracks(bufferSize);

            Engine.Monitor.FinishMeasurement(systemKey);
            tlock.Release();

            return;
        }

        var samplesToProcess = bufferSize;
        if (carryOverSamples > 0)
        {
            if (carryOverSamples > bufferSize)
            {
                ProcessTracks(bufferSize);
                carryOverSamples -= bufferSize;

                Engine.Monitor.FinishMeasurement(systemKey);
                tlock.Release();

                return;
            }

            ProcessTracks(carryOverSamples);
            samplesToProcess -= carryOverSamples;
            carryOverSamples = 0;
        }

        var steps = samplesToProcess / currentPattern.Value.SamplesPerStep;
        var remainingSamples = samplesToProcess - steps * currentPattern.Value.SamplesPerStep;
        var samplesPerStep = currentPattern.Value.SamplesPerStep;
        for (var i = 0; i < steps; i++)
        {
            Step();
            ProcessTracks(samplesPerStep);
        }

        if (remainingSamples == 0)
        {
            Engine.Monitor.FinishMeasurement(systemKey);
            tlock.Release();

            return;
        }

        Step();
        ProcessTracks(remainingSamples);
        carryOverSamples = samplesPerStep - remainingSamples;

        Engine.Monitor.FinishMeasurement(systemKey);
        tlock.Release();
    }

    public void Play(SongPattern pattern)
    {
        tlock.Wait();

        StopEngine();
        currentPattern = ProcessPattern(pattern);
        foreach (var seq in currentPattern.Value.Sequences)
        {
            if (!tracks.TryGetValue(seq.Key, out var track))
            {
                continue;
            }

            track.NextSequence = null;
            track.CurrentSequence = seq.Value;
            track.Reset();
        }

        isPlaying = true;

        tlock.Release();
    }

    public void Queue(SongPattern pattern)
    {
        nextPattern = ProcessPattern(pattern);
        foreach (var seq in nextPattern.Value.Sequences)
        {
            if (!tracks.TryGetValue(seq.Key, out var track))
            {
                continue;
            }

            track.NextSequence = seq.Value;
        }
    }

    public void Pause()
    {
        tlock.Wait();

        isPlaying = false;
        foreach (var track in tracks.Values)
        {
            track.Stop();
        }

        tlock.Release();
    }

    public void Resume()
    {
        tlock.Wait();

        if (currentPattern is not null)
        {
            isPlaying = true;
        }

        tlock.Release();
    }

    public void Stop()
    {
        tlock.Wait();
        StopEngine();
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
            sequences.Add
            (
                trackPattern.TrackName,
                CreateSequence(trackPattern, pattern.Length, sequenceSteps, barSteps)
            );
        }

        return new(pattern.Length * barSteps, samplesPerStep, sequences);
    }

    private Sequence CreateSequence(TrackPattern pattern, int length, int sequenceSteps, int barSteps)
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
        for (var i = 0; i < length; i++)
        {
            var noteEventsSlice = sequenceNoteEvents.AsSpan().Slice(i * barSteps, barSteps);
            var noteEvents = patternNoteEvents.AsSpan().Slice(i * barNotes, barNotes);

            ProcessBar(noteEventsSlice, noteEvents, sequenceSteps, barSteps);
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

    private void ProcessBar
        (
        Span<SequenceNoteEvent> sequenceNoteEvents,
        ReadOnlySpan<NoteEvent> noteEvents,
        int sequenceSteps,
        int barSteps
        )
    {
        var patternStep = 0;
        var sequenceStep = 0;
        var noteFill = new SequenceNoteEvent();
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
                var noteEvent = noteEvents[patternStep];
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
        currentPattern = nextPattern;
        nextPattern = null;

        if (InternalConductor is not null)
        {
            Task.Run(() => InternalConductor.UpdateSynthEngine());
        }
    }

    private void StopEngine()
    {
        isPlaying = false;
        currentStep = 0;
        carryOverSamples = 0;
        currentPattern = null;
        nextPattern = null;
        foreach (var track in tracks.Values)
        {
            track.CurrentSequence = null;
            track.NextSequence = null;
            track.Reset();
        }
    }

    private readonly record struct ProcessedPattern
        (
        int PatternLength,
        int SamplesPerStep,
        Dictionary<string, Sequence> Sequences
        );
}