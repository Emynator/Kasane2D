using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Synthesis;

internal class SynthVoice
{
    private readonly string systemKey;
    private readonly IMixBus bus;
    private readonly Generator generator;
    private readonly Envelope envelope;
    private double frequency = 0.0d;

    public SynthVoice(string engineName, string name, int sampleRate, IMixBus bus, Generator generator)
    {
        systemKey = $"MusicSystem::{engineName}::Track::{name}::Process";
        this.bus = bus;
        this.generator = generator;
        envelope = new(sampleRate);
    }

    public void Process(int sampleCount)
    {
        Engine.Monitor.StartMeasurement(systemKey);
        
        var generatorOut = new float[sampleCount];
        var result = new float[sampleCount];
        
        generator.Generate(generatorOut, frequency);
        envelope.Apply(generatorOut, result);
        
        bus.WriteLeft(result);
        bus.WriteRight(result);
        
        Engine.Monitor.FinishMeasurement(systemKey);
    }

    public void Play(Note note)
    {
        frequency = note.Frequency();
        generator.Reset();
        envelope.Reset();
    }

    public void Stop()
    {
        envelope.EnterRelease();
    }

    public void ControlUpdate(ControlEvent ev)
    {
        if (ev.VolumeUpdate.DoUpdate)
        {
            bus.Gain = ev.VolumeUpdate.Value;
        }

        if (ev.PanUpdate.DoUpdate)
        {
            bus.Pan = ev.PanUpdate.Value;
        }

        if (ev.EnvelopeUpdate.DoUpdate)
        {
            envelope.Attack = ev.EnvelopeUpdate.Attack;
            envelope.Decay = ev.EnvelopeUpdate.Decay;
            envelope.Sustain = ev.EnvelopeUpdate.Sustain;
            envelope.Release = ev.EnvelopeUpdate.Release;
        }

        if (ev.GeneratorUpdate is not null)
        {
            generator.ControlUpdate(ev.GeneratorUpdate);
        }
    }
}