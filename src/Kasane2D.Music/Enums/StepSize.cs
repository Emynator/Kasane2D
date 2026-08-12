namespace Kasane2D.Music.Enums;

/// <summary>
/// The kind of step size the pattern uses for note events.
/// </summary>
public enum StepSize
{
    /// <summary>
    /// Each step is a quarter note.
    /// </summary>
    Quarter,
    /// <summary>
    /// Each step is an eighth note.
    /// </summary>
    Eighth,
    /// <summary>
    /// Each step is a sixteenth note.
    /// </summary>
    Sixteenth,
    /// <summary>
    /// Each step is a thirty-twoth note.
    /// </summary>
    ThirtyTwoth,
    /// <summary>
    /// Each step is a sixty-fourth note.
    /// </summary>
    SixtyFourth,
    /// <summary>
    /// Each step is an one-hundred-twenty-eighth note.
    /// </summary>
    OneHundredTwentyEighth,
}

/// <summary>
/// Extension methods for StepSize.
/// </summary>
public static class StepSizeExtensions
{
    /// <summary>
    /// Gets the number of sequencer steps in a given step size.
    /// </summary>
    /// <param name="stepSize">The step size.</param>
    /// <returns>The number of sequencer steps.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if there is no valid amount of sequencer steps for
    /// the provided step size.</exception>
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