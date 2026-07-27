using Kasane2D.Config;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Mixer;
using Kasane2D.Sound.Sfx;

namespace Kasane2D.Sound;

internal class SoundSystem : ISoundSystem
{
    private readonly AudioMixer mixer;
    private readonly SfxManager sfxManager;
    
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
        sfxManager.Update(sampleCount);
        mixer.Master.Mix(sampleCount);
    }
}