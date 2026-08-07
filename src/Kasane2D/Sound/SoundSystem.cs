using Kasane2D.Config;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Mixer;
using Kasane2D.Sound.Sfx;

namespace Kasane2D.Sound;

internal class SoundSystem : ISoundSystem
{
    private readonly AudioMixer mixer;
    private readonly SfxManager sfxManager;
    private readonly List<ISoundSubSystem> subSystems = [];

    public SoundSystem(AudioConfiguration config)
    {
        SampleRate = config.SampleRate;
        mixer = new(config);
        sfxManager = new(config, mixer);
    }

    public int SampleRate { get; }

    public IAudioMixer AudioMixer => mixer;

    public ISfxManager SfxManager => sfxManager;

    public void Process(int sampleCount)
    {
        var tasks = subSystems.Select(system => Task.Run(() => system.Process(sampleCount))).ToList();
        tasks.Add(Task.Run(() => sfxManager.Update(sampleCount)));
        Task.WaitAll(tasks);
        
        mixer.Master.Mix(sampleCount);
    }

    public void AddSubSystem(ISoundSubSystem system)
    {
        subSystems.Add(system);
    }

    public void RemoveSubSystem(Guid id)
    {
        subSystems.RemoveAll(system => system.Id == id);
    }
}