namespace Kasane2D.Music.Types.SequenceEvents;

public readonly record struct EnvelopeUpdate
{
    public EnvelopeUpdate()
    {
        DoUpdate = false;
    }
    
    public EnvelopeUpdate(float attack, float decay, float sustain, float release)
    {
        DoUpdate = true;
        Attack = attack;
        Decay = decay;
        Sustain = sustain;
        Release = release;
    }
    
    public bool DoUpdate { get; }
    
    public float Attack { get; }
    
    public float Decay { get; }
    
    public float Sustain { get; }
    
    public float Release { get; }
}