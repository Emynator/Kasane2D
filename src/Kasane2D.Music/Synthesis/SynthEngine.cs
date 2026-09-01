using Kasane2D.Music.Extensions;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class SynthEngine : ISynthEngine
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly string systemKey;
    private readonly int bufferSize;
    private readonly Dictionary<string, Sequencer> tracks;
    private bool isPlaying = false;
    private int currentStep = 0;
    private int carryOverSamples = 0;
    private ProcessedSongPattern? currentPattern = null;
    private ProcessedSongPattern? nextPattern = null;

    public SynthEngine(string name, int samplerate, int bufferSize, Dictionary<string, Sequencer> tracks)
    {
        systemKey = $"MusicSystem::SynthEngine::{name}::Process";
        Samplerate = samplerate;
        this.bufferSize = bufferSize;
        this.tracks = tracks;
        Tracks = tracks.Select(t => t.Value).ToList();
    }

    public Guid Id { get; } = Guid.NewGuid();

    public IReadOnlyCollection<ITrack> Tracks { get; }
    
    public Conductor? InternalConductor { get; set; }

    public IConductor? Conductor => InternalConductor;

    public int Samplerate { get; }
    
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

        var steps = samplesToProcess / currentPattern.SamplesPerStep;
        var remainingSamples = samplesToProcess - steps * currentPattern.SamplesPerStep;
        var samplesPerStep = currentPattern.SamplesPerStep;
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
        Play(pattern.ProcessPattern(Samplerate));
    }

    public void Play(ProcessedSongPattern songPattern)
    {
        tlock.Wait();

        StopEngine();
        currentPattern = songPattern;
        foreach (var seq in songPattern.Sequences)
        {
            if (!tracks.TryGetValue(seq.Key, out var track))
            {
                continue;
            }

            track.CurrentSequence = seq.Value;
            track.NextSequence = null;
            track.Reset();
        }

        isPlaying = true;

        tlock.Release();
    }

    public void Queue(SongPattern pattern)
    {
        Queue(pattern.ProcessPattern(Samplerate));
    }
    
    public void Queue(ProcessedSongPattern songPattern)
    {
        tlock.Wait();
        
        foreach (var seq in songPattern.Sequences)
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
}