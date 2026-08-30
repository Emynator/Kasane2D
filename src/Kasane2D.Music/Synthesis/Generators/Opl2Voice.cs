using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

internal class Opl2Voice : Generator
{
    private FmOperator op0;
    private FmOperator op1;
    private float modulationDepth = 0.0f;
    private bool isAdditive = false;
    
    public Opl2Voice(int sampleRate) : base(sampleRate)
    {
        op0 = new(sampleRate);
        op1 = new(sampleRate);
    }
    
    public override void ControlUpdate(GeneratorUpdate ev)
    {
        if (ev is not Opl2VoiceUpdate actual)
        {
            return;
        }

        if (actual.Operator0 is not null)
        {
            op0.Update(actual.Operator0.Value);
        }

        if (actual.Operator1 is not null)
        {
            op1.Update(actual.Operator1.Value);
        }

        if (actual.ModulationDepth is not null)
        {
            var x = Math.Clamp(actual.ModulationDepth.Value, 0.0f, 1.0f);
            modulationDepth = 8.0f * MathF.PI * x * x;
        }

        if (actual.IsAdditive is not null)
        {
            isAdditive = actual.IsAdditive.Value;
        }
    }

    public override void Reset()
    {
        base.Reset();
        op0.Reset();
        op1.Reset();
    }

    protected override float Generate(double frequency)
    {
        if (isAdditive)
        {
            return op0.Next(frequency) + op1.Next(frequency);
        }

        return op1.Next(frequency, modulationDepth * op0.Next(frequency));
    }
}