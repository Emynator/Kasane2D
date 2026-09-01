using Kasane2D.Music.Configs;
using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Synthesis;
using Kasane2D.Music.Synthesis.Effects;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Sound.AudioEffects;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music;

/// <summary>
/// Static factory functions for integrating the music system with the Kasane2D audio system.
/// </summary>
public static class MusicSystem
{
    /// <summary>
    /// Creates a new synthesizer engine for the given config.
    /// </summary>
    /// <param name="soundSystem">The Kasane2D sound system.</param>
    /// <param name="config">The config for the synthesizer engine.</param>
    /// <returns>The created synthesizer engine.</returns>
    public static ISynthEngine CreateSynthEngine(this ISoundSystem soundSystem, SynthConfig config)
    {
        var mainBus = soundSystem.AudioMixer.CreateMixBus(config.Name);
        var tracks = new Dictionary<string, Sequencer>();

        foreach (var trackConfig in config.TrackConfigs)
        {
            var generator = trackConfig.Kind switch
            {
                GeneratorKind.Custom => trackConfig.CustomGeneratorFactory is not null
                    ? trackConfig.CustomGeneratorFactory(soundSystem.SampleRate)
                    : new EmptyGenerator(soundSystem.SampleRate),
                GeneratorKind.BasicOscillator => new BasicOscillator(soundSystem.SampleRate),
                GeneratorKind.DmgNoise => new DmgNoise(soundSystem.SampleRate),
                GeneratorKind.SidNoise => new SidNoise(soundSystem.SampleRate),
                GeneratorKind.RetroWaveTable => new RetroWaveTableOscillator(soundSystem.SampleRate),
                GeneratorKind.Sampler => new Sampler(soundSystem.SampleRate),
                GeneratorKind.Opl2Voice => new Opl2Voice(soundSystem.SampleRate),
                _ => new EmptyGenerator(soundSystem.SampleRate),
            };

            var voiceBus = soundSystem.AudioMixer.CreateMixBus(trackConfig.Name, mainBus);
            var effects = new List<VoiceEffect>();
            var delays = new List<VoiceDelay>();
            var ppDelays = new List<VoicePingPongDelay>();
            foreach (var effectConfig in trackConfig.Effects)
            {
                VoiceEffect? effect = null;
                switch (effectConfig.Kind)
                {
                    case VoiceEffectKind.Custom:
                        effect = effectConfig.CustomEffectFactory?.Invoke(soundSystem.SampleRate);
                        break;
                    
                    case VoiceEffectKind.Utility:
                        effect = new VoiceUtility(soundSystem.CreateUtility(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Filter:
                        effect = new VoiceFilter(soundSystem.CreateFilter(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Eq8:
                        effect = new VoiceEq8(soundSystem.CreateEq8(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Compressor:
                        effect = new VoiceCompressor(soundSystem.CreateCompressor(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Limiter:
                        effect = new VoiceLimiter(soundSystem.CreateLimiter(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Overdrive:
                        effect = new VoiceOverdrive(soundSystem.CreateOverdrive(effectConfig.Name));
                        break;
                    
                    case VoiceEffectKind.Delay:
                        var delay = new VoiceDelay(soundSystem.CreateDelay(effectConfig.Name));
                        delays.Add(delay);
                        effect = delay;
                        break;
                    
                    case VoiceEffectKind.PingPongDelay:
                        var ppDelay = new VoicePingPongDelay(soundSystem.CreatePingPongDelay(effectConfig.Name));
                        ppDelays.Add(ppDelay);
                        effect = ppDelay;
                        break;
                }

                if (effect is not null)
                {
                    effects.Add(effect);
                }
            }

            var voice = new SynthVoice
            (
                config.Name,
                trackConfig.Name,
                soundSystem.SampleRate,
                soundSystem.BufferSize,
                voiceBus,
                generator,
                effects
            );
            
            var sequencer = new Sequencer(trackConfig.Name, voice);
            foreach (var delay in delays)
            {
                delay.Sequencer = sequencer;
            }
            foreach (var delay in ppDelays)
            {
                delay.Sequencer = sequencer;
            }
            
            tracks.Add(trackConfig.Name, sequencer);
        }

        var result = new SynthEngine(config.Name, soundSystem.SampleRate, soundSystem.BufferSize, tracks);
        soundSystem.AddSubSystem(result);

        return result;
    }

    /// <summary>
    /// Creates a conductor to manage a synthesizer engine.
    /// </summary>
    /// <param name="engine">The synthesizer engine the conductor will manage.</param>
    /// <returns>The created conductor.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the synth engine is not a synth engine of the music
    /// system</exception>
    public static IConductor CreateConductor(this ISynthEngine engine)
    {
        return engine is SynthEngine actual
            ? new Conductor(actual)
            : throw new InvalidOperationException();
    }
}