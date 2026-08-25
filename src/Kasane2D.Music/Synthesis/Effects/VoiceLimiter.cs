using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoiceLimiter : VoiceEffect
{
    private readonly KasaneLimiter effect;

    public VoiceLimiter(KasaneLimiter effect)
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

        if (ev is not LimiterUpdate actual)
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
        if (actual.Attack is not null)
        {
            effect.Attack = actual.Attack.Value;
        }
        if (actual.Release is not null)
        {
            effect.Release = actual.Release.Value;
        }
        if (actual.Ceiling is not null)
        {
            effect.Ceiling = actual.Ceiling.Value;
        }
        if (actual.Gain is not null)
        {
            effect.Gain = actual.Gain.Value;
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