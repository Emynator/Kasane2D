using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Synthesis;

internal class SynthVoice
{
    private readonly IMixBus bus;
    private readonly Generator generator;
    private double frequency = 0.0d;

    public SynthVoice(int sampleRate, IMixBus bus, Generator generator)
    {
        this.bus = bus;
        this.generator = generator;
        Envelope = new(sampleRate);
    }
    
    public Envelope Envelope { get; }

    public void Process(int sampleCount)
    {
        var generatorOut = new float[sampleCount];
        var result = new float[sampleCount];
        
        generator.Generate(generatorOut, frequency);
        Envelope.Apply(generatorOut, result);
        
        bus.InLeft.Write(result);
        bus.InRight.Write(result);
    }

    public void Play(Note note)
    {
        frequency = note.Frequency();
        Envelope.Reset();
    }

    public void Stop()
    {
        Envelope.EnterRelease();
    }
}