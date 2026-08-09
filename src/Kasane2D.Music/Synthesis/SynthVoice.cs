using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Music.Types.SequenceEvents;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Synthesis;

internal class SynthVoice
{
    private readonly IMixBus bus;
    private readonly Generator generator;
    private readonly Envelope envelope;
    private double frequency = 0.0d;

    public SynthVoice(int sampleRate, IMixBus bus, Generator generator)
    {
        this.bus = bus;
        this.generator = generator;
        envelope = new(sampleRate);
    }

    public void Process(int sampleCount)
    {
        var generatorOut = new float[sampleCount];
        var result = new float[sampleCount];
        
        generator.Generate(generatorOut, frequency);
        envelope.Apply(generatorOut, result);
        
        bus.InLeft.Write(result);
        bus.InRight.Write(result);
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

    public void ControlUpdate(SequenceControlEvent ev)
    {
        if (ev.VolumeUpdate.DoUpdate)
        {
            bus.Level = ev.VolumeUpdate.Value;
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