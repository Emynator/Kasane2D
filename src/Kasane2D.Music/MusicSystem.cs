using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Synthesis;
using Kasane2D.Music.Synthesis.Generators;
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
                _ => new EmptyGenerator(soundSystem.SampleRate),
            };

            var voiceBus = soundSystem.AudioMixer.CreateMixBus(trackConfig.Name, mainBus);
            var voice = new SynthVoice(soundSystem.SampleRate, voiceBus, generator);

            tracks.Add(trackConfig.Name, new Sequencer(voice));
        }

        var result = new SynthEngine(soundSystem.SampleRate, soundSystem.BufferSize, tracks);
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