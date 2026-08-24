namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Represents and audio effect.
/// </summary>
public interface IAudioEffect
{
    /// <summary>
    /// Name of the audio effect.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Toggles if the audio effect should be applied or bypassed.
    /// </summary>
    public bool Bypass { get; set; }

    /// <summary>
    /// Called to apply the audio effect.
    /// </summary>
    /// <param name="inLeft">Input buffer containing the samples of the left channel.</param>
    /// <param name="inRight">Input buffer containing the samples of the right channel.</param>
    /// <param name="outLeft">Output buffer of the left channel where the resulting samples will be written to.</param>
    /// <param name="outRight">Output buffer of the right channel where the resulting samples will be written to.</param>
    public void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        );
}