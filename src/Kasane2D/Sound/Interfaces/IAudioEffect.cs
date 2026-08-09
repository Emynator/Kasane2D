namespace Kasane2D.Sound.Interfaces;

public interface IAudioEffect
{
    public string Name { get; }

    public void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        );
}