namespace Kasane2D.Sound.AudioEffects.Dsp;

internal struct EnvelopeFollower
{
    private readonly int sampleRate;
    private float attackCoefficient;
    private float releaseCoefficient;
    private float envelope;

    public EnvelopeFollower(int sampleRate)
    {
        this.sampleRate = sampleRate;
    }
    
    public float Attack
    {
        get;
        set
        {
            field = value;
            attackCoefficient = MathF.Exp(-1.0f / (value * sampleRate));
        }
    }

    public float Release
    {
        get;
        set
        {
            field = value;
            releaseCoefficient = MathF.Exp(-1.0f / (value * sampleRate));
        }
    }

    public float Next(float sample)
    {
        var level = MathF.Abs(sample);
        
        if (level > envelope)
        {
            envelope = attackCoefficient * envelope +  (1.0f - attackCoefficient) * level;
        }
        else
        {
            envelope = releaseCoefficient * envelope + (1.0f - releaseCoefficient) * level;
        }
        
        return envelope;
    }
}