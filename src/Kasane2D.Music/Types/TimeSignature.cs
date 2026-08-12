namespace Kasane2D.Music.Types;

/// <summary>
/// Represents a musical time signature.
/// </summary>
public readonly record struct TimeSignature
{
    /// <summary>
    /// Standard 4/4 time.
    /// </summary>
    public static TimeSignature FourFour => new TimeSignature(4, 4);
    
    /// <summary>
    /// Commonly used 3/4 time.
    /// </summary>
    public static TimeSignature ThreeFour => new TimeSignature(3, 4);
    
    /// <summary>
    /// Creates a new time signature.
    /// </summary>
    /// <param name="numerator">The number of note types in a bar.</param>
    /// <param name="denominator">The type of note used to define the signature.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the denominator is neither 2, 3, 8, nor 16.</exception>
    /// <remarks>2 is half-note, 4 is quarter-note, 8 is 8th note, 16 is 16th note.</remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Time_signature"/>
    public TimeSignature(int numerator, int denominator)
    {
        if (denominator is not (2 or 4 or 8 or 16))
        {
            throw new ArgumentOutOfRangeException(nameof(denominator));
        }
        
        Numerator = numerator;
        Denominator = denominator;
    }
    
    /// <summary>
    /// Gets the number of note types in a bar.
    /// </summary>
    public int Numerator { get; }
    
    /// <summary>
    /// Gets the type of note used to define the signature.
    /// </summary>
    public int Denominator { get; }
    
    /// <summary>
    /// ToString override.
    /// </summary>
    /// <returns>String representation of the time signature.</returns>
    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    /// <summary>
    /// Gets the number of sequencer control steps in a single bar.
    /// </summary>
    /// <returns>The number of sequencer control steps in a bar.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public int GetSequenceStepsPerBar()
    {
        return Denominator switch
        {
            2 => Numerator * Constants.SequencerStepsPerQuarterNote * 2,
            4 => Numerator * Constants.SequencerStepsPerQuarterNote,
            8 => Numerator * Constants.SequencerStepsPerQuarterNote / 2,
            16 => Numerator * Constants.SequencerStepsPerQuarterNote / 4,
            _ => throw new InvalidOperationException(),
        };
    }

    internal int GetSamplesPerStep(int sampleRate, int bpm)
    {
        var bps = bpm / 60.0f;
        var samplesPerBeat = sampleRate / bps;
        var stepsPerBeat = Denominator switch
        {
            2 => Constants.SequencerStepsPerQuarterNote * 2,
            4 => Constants.SequencerStepsPerQuarterNote,
            8 => Constants.SequencerStepsPerQuarterNote / 2,
            16 => Constants.SequencerStepsPerQuarterNote / 4,
            _ => throw new InvalidOperationException(),
        };
        
        var result = (int)(samplesPerBeat / stepsPerBeat);
        
        return result;
    }
}