using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceUtility : VoiceEffect
{
    private readonly KasaneUtility effect;

    public VoiceUtility(KasaneUtility effect)
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
        
        if (ev is not UtilityUpdate actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }
        if (actual.Gain is not null)
        {
            effect.Gain = actual.Gain.Value;
        }
        if (actual.Pan is not null)
        {
            effect.Pan = actual.Pan.Value;
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