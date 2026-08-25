namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for compressor parameters.
/// </summary>
/// <param name="EffectName">Name of the effect this update targets.</param>
/// <param name="Bypass">Optional: changes the bypass value of the effect.</param>
/// <param name="Drive">Optional: changes the drive value of the compressor.</param>
/// <param name="Attack">Optional: changes the attack value of the compressor.</param>
/// <param name="Release">Optional: changes the release value of the compressor.</param>
/// <param name="Threshold">Optional: changes the threshold value of the compressor.</param>
/// <param name="Ratio">Optional: changes the ratio value of the compressor.</param>
/// <param name="MakeupGain">Optional: changes the makeup gain value of the compressor.</param>
/// <param name="Wet">Optional: changes the wet value of the compressor.</param>
public sealed record class CompressorUpdate
    (
    string EffectName,
    bool? Bypass = null,
    float? Drive = null,
    float? Attack = null,
    float? Release = null,
    float? Threshold = null,
    int? Ratio = null,
    float? MakeupGain = null,
    float? Wet = null
    ) : EffectUpdate(EffectName, Bypass);