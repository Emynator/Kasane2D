namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents;

/// <summary>
/// Represents an update to the track's envelope.
/// </summary>
public readonly record struct EnvelopeUpdate
{
    /// <summary>
    /// Creates an empty update that changes nothing.
    /// </summary>
    public EnvelopeUpdate()
    {
        DoUpdate = false;
    }
    
    /// <summary>
    /// Creates an update that changes the ADSR value.
    /// </summary>
    /// <param name="attack">Attack time in ms.</param>
    /// <param name="decay">Decay time in ms.</param>
    /// <param name="sustain">Sustain value in relative gain.</param>
    /// <param name="release">Release time in ms.</param>
    public EnvelopeUpdate(float attack, float decay, float sustain, float release)
    {
        DoUpdate = true;
        Attack = attack;
        Decay = decay;
        Sustain = sustain;
        Release = release;
    }
    
    internal bool DoUpdate { get; }
    
    /// <summary>
    /// Attack time in ms.
    /// </summary>
    public float Attack { get; }
    
    /// <summary>
    /// Decay time in ms.
    /// </summary>
    public float Decay { get; }
    
    /// <summary>
    /// Sustain value in relative gain.
    /// </summary>
    public float Sustain { get; }
    
    /// <summary>
    /// Release time in ms.
    /// </summary>
    public float Release { get; }
}