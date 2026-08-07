namespace Kasane2D.Music.Enums;

public enum StepSize
{
    Quarter,
    Eighth,
    Sixteenth,
    ThirtyTwoth,
    SixtyFourth,
    OneHundredTwentyEighth,
}

public static class StepSizeExtensions
{
    public static int GetSampleCount(this StepSize stepSize, int sampleRate, int bpm)
    {
        var bps = bpm / 60.0f;
        var samplesPerBeat = (int)(sampleRate / bps);
        var result = stepSize switch
        {
            StepSize.Quarter => samplesPerBeat,
            StepSize.Eighth => samplesPerBeat / 2,
            StepSize.Sixteenth => samplesPerBeat / 4,
            StepSize.ThirtyTwoth => samplesPerBeat / 8,
            StepSize.SixtyFourth => samplesPerBeat / 16,
            StepSize.OneHundredTwentyEighth => samplesPerBeat / 32,
            _ => throw new ArgumentOutOfRangeException(nameof(stepSize), stepSize, null),
        };
        
        return result;
    }

    public static int GetSequenceSteps(this StepSize stepSize)
    {
        return stepSize switch
        {
            StepSize.Quarter => Constants.SequencerStepsPerQuarterNote,
            StepSize.Eighth => Constants.SequencerStepsPerQuarterNote / 2,
            StepSize.Sixteenth => Constants.SequencerStepsPerQuarterNote / 4,
            StepSize.ThirtyTwoth => Constants.SequencerStepsPerQuarterNote / 8,
            StepSize.SixtyFourth => Constants.SequencerStepsPerQuarterNote / 16,
            StepSize.OneHundredTwentyEighth => Constants.SequencerStepsPerQuarterNote / 32,
            _ => throw new ArgumentOutOfRangeException(nameof(stepSize), stepSize, null),
        };
    }
}