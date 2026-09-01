namespace Kasane2D.Music.Enums;

/// <summary>
/// Musical delay length.
/// </summary>
public enum DelayLength
{
    /// <summary>
    /// 2 bars delay time.
    /// </summary>
    TwoBars,
    /// <summary>
    /// 1 bar delay time.
    /// </summary>
    Bar,
    /// <summary>
    /// Half note delay time.
    /// </summary>
    HalfNote,
    /// <summary>
    /// Quarter note delay time.
    /// </summary>
    QuarterNote,
    /// <summary>
    /// Eighth note delay time.
    /// </summary>
    EighthNote,
    /// <summary>
    /// Triplet eighth note delay time.
    /// </summary>
    DottedEighthNote,
    /// <summary>
    /// Sixteenth note delay time.
    /// </summary>
    SixteenthNote,
    /// <summary>
    /// Triplet sixteenth note delay time.
    /// </summary>
    DottedSixteenthNote,
}

internal static class DelayLengthExtensions
{
    public static float CalculateDelayTime(this DelayLength length, int bpm, int beatsPerBar)
    {
        var beatLength = 60.0f / bpm;

        return length switch
        {
            DelayLength.TwoBars => beatsPerBar * 2.0f * beatLength,
            DelayLength.Bar => beatsPerBar * beatLength,
            DelayLength.HalfNote => beatLength * 2.0f,
            DelayLength.QuarterNote => beatLength,
            DelayLength.EighthNote => beatLength / 2.0f,
            DelayLength.DottedEighthNote => beatLength / 3.0f,
            DelayLength.SixteenthNote => beatLength / 4.0f,
            DelayLength.DottedSixteenthNote => beatLength / 6.0f,
            _ => 0.0f,
        };
    }
}