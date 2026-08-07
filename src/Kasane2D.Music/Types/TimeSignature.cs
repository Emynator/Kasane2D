namespace Kasane2D.Music.Types;

public readonly record struct TimeSignature
{
    public static TimeSignature FourFour => new TimeSignature(4, 4);
    
    public static TimeSignature ThreeFour => new TimeSignature(3, 4);
    
    public TimeSignature(int numerator, int denominator)
    {
        if (denominator is not (2 or 4 or 8 or 16))
        {
            throw new ArgumentOutOfRangeException(nameof(denominator));
        }
        
        Numerator = numerator;
        Denominator = denominator;
    }
    
    public int Numerator { get; }
    
    public int Denominator { get; }
    
    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    internal int GetSequenceStepsPerBar()
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