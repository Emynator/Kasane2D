using Kasane2D.Config;
using Kasane2D.Events;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Mixer;
using Kasane2D.Sound.MusicPlayback;
using Kasane2D.Sound.Sfx;
using Kasane2D.Sound.Types;
using Kasane2D.Types;

namespace Kasane2D.Sound;

internal class SoundSystem : ISoundSystem
{
    private const string systemKey = "Engine::SoundSystem::Process";

    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly float bufferSizeInSeconds;
    private readonly AudioMixer mixer;
    private readonly SfxManager sfxManager;
    private readonly MusicPlayer musicPlayer;
    private readonly List<ISoundSubSystem> subSystems = [];
    private readonly List<AutomationCurve> automationCurves = [];

    public SoundSystem(AudioConfiguration config)
    {
        bufferSizeInSeconds = config.BufferSizeInMs / 1000.0f;
        SampleRate = config.SampleRate;
        BufferSize = (int)(SampleRate / 1000.0f * config.BufferSizeInMs);
        mixer = new(config, BufferSize);
        sfxManager = new(config, BufferSize, mixer);
        musicPlayer = new(BufferSize, mixer);
    }

    public int SampleRate { get; }

    public int BufferSize { get; }

    public IAudioMixer AudioMixer => mixer;

    public KasaneEvent<StereoAudioStream>? InternalBufferProcessedEvent { get; set; }

    public KasaneEvent<StereoAudioStream> BufferProcessedEvent =>
        InternalBufferProcessedEvent ?? throw new InvalidOperationException("Audio device not initialized.");

    public ISfxManager SfxManager => sfxManager;

    public IMusicPlayer MusicPlayer => musicPlayer;

    public void Process()
    {
        tlock.Wait();
        Engine.Monitor.StartMeasurement(systemKey);

        var tasks = subSystems.Select(system => Task.Run(system.Process)).ToList();
        tasks.AddRange(automationCurves.Select(c => Task.Run(c.Apply)));
        tasks.Add(Task.Run(() => sfxManager.Update()));
        tasks.Add(Task.Run(() => musicPlayer.Update()));
        Task.WaitAll(tasks);

        mixer.InternalMaster.Mix();

        Engine.Monitor.FinishMeasurement(systemKey);
        tlock.Release();
    }

    public void AddSubSystem(ISoundSubSystem system)
    {
        tlock.Wait();
        subSystems.Add(system);
        tlock.Release();
    }

    public void RemoveSubSystem(Guid id)
    {
        tlock.Wait();
        subSystems.RemoveAll(system => system.Id == id);
        tlock.Release();
    }

    public IAudioBuffer CreateBuffer(int bufferSize)
    {
        return new AudioBuffer(bufferSize);
    }

    public void AddAutomationCurve
        (
        float startValue,
        float targetValue,
        float duration,
        Action<float> setterAction,
        float? timeBias = null,
        float? valueBias = null,
        Action? finishedCallback = null
        )
    {
        var quantizedDuration = MathF.Ceiling(duration / bufferSizeInSeconds) * bufferSizeInSeconds;
        var increment = bufferSizeInSeconds / quantizedDuration;

        var start = new Vec2F(0.0f, startValue);
        var end = new Vec2F(quantizedDuration, targetValue);
        var controlX = timeBias ?? duration / 2.0f;
        var controlY = valueBias ?? (startValue + targetValue) / 2.0f;

        var curve = new Bezier(start, end, new(controlX, controlY));
        var id = Guid.NewGuid();

        tlock.Wait();
        automationCurves.Add
        (
            new
            (
                id,
                curve,
                setterAction,
                () =>
                {
                    RemoveAutomationCurve(id);
                    finishedCallback?.Invoke();
                },
                increment
            )
        );
        tlock.Release();
    }

    private void RemoveAutomationCurve(Guid id)
    {
        tlock.Wait();
        automationCurves.RemoveAll(curve => curve.Id == id);
        tlock.Release();
    }
}