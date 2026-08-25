using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceFilter : VoiceEffect
{
    private readonly KasaneFilter effect;

    public VoiceFilter(KasaneFilter effect)
    {
        this.effect = effect;
    }

    public override bool Bypass => effect.Bypass;

    public override void ControlUpdate(EffectUpdate ev)
    {
        if (ev.EffectName != effect.Name)
        {
            return;
        }

        if (ev is not FilterUpdate actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }
        if (actual.Type is not null)
        {
            effect.Type = actual.Type.Value;
        }
        if (actual.Slope is not null)
        {
            effect.Slope = actual.Slope.Value;
        }
        if (actual.CutoffFrequency is not null)
        {
            effect.CutoffFrequency = actual.CutoffFrequency.Value;
        }
        if (actual.Resonance is not null)
        {
            effect.Resonance = actual.Resonance.Value;
        }
    }

    public override void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        )
    {
        effect.Apply(inLeft, inRight, outLeft, outRight);
    }
}