using Kasane2D.Music.Enums;
using Kasane2D.Music.Synthesis.Effects;
using Kasane2D.Music.Synthesis.Generators;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Synthesis;

internal class SynthVoice
{
    private readonly string systemKey;
    private readonly IMixBus bus;
    private readonly Generator generator;
    private readonly List<VoiceEffect> effects;
    private readonly float[] scratchBuffer0;
    private readonly float[] scratchBuffer1;
    private readonly float[] scratchBuffer2;
    private readonly float[] scratchBuffer3;
    private Envelope envelope;
    private double frequency = 0.0d;

    public SynthVoice
        (
        string engineName,
        string name,
        int sampleRate,
        int bufferSize,
        IMixBus bus,
        Generator generator,
        List<VoiceEffect> effects
        )
    {
        systemKey = $"MusicSystem::SynthEngine::{engineName}::Track::{name}::Process";
        this.bus = bus;
        this.generator = generator;
        this.effects = effects;
        envelope = new(sampleRate);
        scratchBuffer0 = new float[bufferSize];
        scratchBuffer1 = new float[bufferSize];
        scratchBuffer2 = new float[bufferSize];
        scratchBuffer3 = new float[bufferSize];
    }
    
    public void Process(int sampleCount)
    {
        Engine.Monitor.StartMeasurement(systemKey);

        var generatorOut = scratchBuffer0.AsSpan().Slice(0, sampleCount);
        var result = scratchBuffer1.AsSpan().Slice(0, sampleCount);

        generator.Generate(generatorOut, frequency);
        envelope.Apply(generatorOut, result);
        
        var effectInLeft = scratchBuffer2.AsSpan().Slice(0, sampleCount);
        var effectInRight = scratchBuffer3.AsSpan().Slice(0, sampleCount);
        var effectOutLeft = scratchBuffer0.AsSpan().Slice(0, sampleCount);
        var effectOutRight = scratchBuffer1.AsSpan().Slice(0, sampleCount);
        
        result.CopyTo(effectInLeft);
        result.CopyTo(effectInRight);

        foreach (var effect in effects)
        {
            if (effect.Bypass)
            {
                continue;
            }
            
            effect.Apply(effectInLeft, effectInRight, effectOutLeft, effectOutRight);
            
            var t = effectInLeft;
            effectInLeft = effectOutLeft;
            effectOutLeft = t;
            
            t = effectInRight;
            effectInRight = effectOutRight;
            effectOutRight = t;
        }

        bus.WriteLeft(effectInLeft);
        bus.WriteRight(effectInRight);

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

    public void Reset()
    {
        generator.Reset();
        envelope.Reset();
        foreach (var effect in effects)
        {
            effect.Reset();
        }
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

        foreach (var effect in effects)
        {
            foreach (var update in ev.EffectUpdates)
            {
                effect.ControlUpdate(update);
            }
        }
    }
}