using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Synthesis;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music;

public static class MusicSystem
{
    public static ISynthEngine CreateSynthEngine(this ISoundSystem soundSystem, SynthConfig config)
    {
        var mainBus = soundSystem.AudioMixer.CreateMixBus(config.Name);
        var tracks = new Dictionary<string, Sequencer>();
        
        foreach (var trackConfig in config.TrackConfigs)
        {
            Generator generator = trackConfig.Kind switch
            {
                GeneratorKind.BasicOscillator => new BasicOscillator(soundSystem.SampleRate),
                GeneratorKind.DmgNoise => new DmgNoise(soundSystem.SampleRate),
                _ => new EmptyGenerator(soundSystem.SampleRate),
            };
            
            var voiceBus = soundSystem.AudioMixer.CreateMixBus(trackConfig.Name, mainBus);
            var voice = new SynthVoice(soundSystem.SampleRate, voiceBus, generator);
            
            tracks.Add(trackConfig.Name, new Sequencer(voice));
        }

        var result = new SynthEngine(soundSystem.SampleRate, tracks);
        soundSystem.AddSubSystem(result);
        
        return result;
    }
}