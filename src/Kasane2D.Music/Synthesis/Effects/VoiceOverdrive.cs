using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceOverdrive : VoiceEffect
{
    private readonly KasaneOverdrive effect;

    public VoiceOverdrive(KasaneOverdrive effect)
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

        if (ev is not OverdriveUpdate actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }
        if (actual.Drive is not null)
        {
            effect.Drive = actual.Drive.Value;
        }
        if (actual.Type is not null)
        {
            effect.Type = actual.Type.Value;
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