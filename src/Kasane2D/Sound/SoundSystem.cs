using Kasane2D.Config;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Mixer;
using Kasane2D.Sound.MusicPlayback;
using Kasane2D.Sound.Sfx;

namespace Kasane2D.Sound;

internal class SoundSystem : ISoundSystem
{
    private readonly AudioMixer mixer;
    private readonly SfxManager sfxManager;
    private readonly MusicPlayer musicPlayer;
    private readonly List<ISoundSubSystem> subSystems = [];

    public SoundSystem(AudioConfiguration config)
    {
        SampleRate = config.SampleRate;
        BufferSize = (int)(SampleRate / 1000.0f * config.BufferSizeInMs);
        mixer = new(config, BufferSize);
        sfxManager = new(config, BufferSize, mixer);
        musicPlayer = new(BufferSize, mixer);
    }

    public int SampleRate { get; }
    
    public int BufferSize { get; }

    public IAudioMixer AudioMixer => mixer;

    public ISfxManager SfxManager => sfxManager;
    
    public IMusicPlayer MusicPlayer => musicPlayer;

    public void Process()
    {
        var tasks = subSystems.Select(system => Task.Run(system.Process)).ToList();
        tasks.Add(Task.Run(() => sfxManager.Update()));
        tasks.Add(Task.Run(() => musicPlayer.Update()));
        Task.WaitAll(tasks);
        
        mixer.InternalMaster.Mix();
    }

    public void AddSubSystem(ISoundSubSystem system)
    {
        subSystems.Add(system);
    }

    public void RemoveSubSystem(Guid id)
    {
        subSystems.RemoveAll(system => system.Id == id);
    }

    public IAudioBuffer CreateBuffer(int bufferSize)
    {
        return new AudioBuffer(BufferSize);
    }
}