using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceEq8 : VoiceEffect
{
    private readonly KasaneEq8 effect;

    public VoiceEq8(KasaneEq8 effect)
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
        
        if (ev is not Eq8Update actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }

        if (actual.BandParams is null)
        {
            return;
        }
        
        for (var i = 0; i < actual.BandParams.Length; i++)
        {
            var value = actual.BandParams[i];
            if (value is not null)
            {
                effect.SetParams(i, value.Value);
            }
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