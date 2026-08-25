using Kasane2D.Sound.Enums;

namespace Kasane2D.Sound.Types;

/// <summary>
/// Represents all parameters for a single equalizer band.
/// </summary>
/// <param name="Type">The filter type of the band.</param>
/// <param name="IsActive">If the band is active or not.</param>
/// <param name="Frequency">The frequency this band works on.</param>
/// <param name="Q">The Q-value of the band.</param>
/// <param name="Gain">The gain of the band.</param>
public readonly record struct EqBandParams(EqFilterType Type, bool IsActive, float Frequency, float Q, float Gain)
{
    public static readonly EqBandParams[] DefaultParams =
    [
        new(EqFilterType.LowShelf, true, 30.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, true, 100.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, true, 200.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, true, 1000.0f, 0.71f, 0.0f),
        new(EqFilterType.HighShelf, true, 5000.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, false, 7500.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, false, 10000.0f, 0.71f, 0.0f),
        new(EqFilterType.Peak, false, 15000.0f, 0.71f, 0.0f),
    ];
}