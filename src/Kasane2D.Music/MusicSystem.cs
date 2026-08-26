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
            Generator generator = trackConfig.Kind switch
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
            foreach (var effectConfig in trackConfig.Effects)
            {
                var effect = effectConfig.Kind switch
                {
                    VoiceEffectKind.Custom => effectConfig.CustomEffectFactory?.Invoke(soundSystem.SampleRate),
                    VoiceEffectKind.Utility => new VoiceUtility(soundSystem.CreateUtility(effectConfig.Name)),
                    VoiceEffectKind.Filter => new VoiceFilter(soundSystem.CreateFilter(effectConfig.Name)),
                    VoiceEffectKind.Eq8 => new VoiceEq8(soundSystem.CreateEq8(effectConfig.Name)),
                    VoiceEffectKind.Compressor => new VoiceCompressor(soundSystem.CreateCompressor(effectConfig.Name)),
                    VoiceEffectKind.Limiter => new VoiceLimiter(soundSystem.CreateLimiter(effectConfig.Name)),
                    VoiceEffectKind.Overdrive => new VoiceOverdrive(soundSystem.CreateOverdrive(effectConfig.Name)),
                    VoiceEffectKind.Delay => new VoiceDelay(soundSystem.CreateDelay(effectConfig.Name)),
                    VoiceEffectKind.PingPongDelay => new VoicePingPongDelay
                        (soundSystem.CreatePingPongDelay(effectConfig.Name)),
                    _ => null,
                };

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
            tracks.Add(trackConfig.Name, new Sequencer(voice));
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