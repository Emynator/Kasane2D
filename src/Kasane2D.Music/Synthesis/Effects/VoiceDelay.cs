using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceDelay : VoiceEffect
{
    private readonly KasaneDelay effect;

    public VoiceDelay(KasaneDelay effect)
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

        if (ev is not DelayUpdate actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }
        if (actual.Delay is not null)
        {
            effect.Delay = actual.Delay.Value;
        }
        if (actual.DecayGain is not null)
        {
            effect.DecayGain = actual.DecayGain.Value;
        }
        if (actual.Feedback is not null)
        {
            effect.Feedback = actual.Feedback.Value;
        }
        if (actual.Wet is not null)
        {
            effect.Wet = actual.Wet.Value;
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