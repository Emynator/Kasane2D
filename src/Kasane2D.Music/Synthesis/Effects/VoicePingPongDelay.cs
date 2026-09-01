using Kasane2D.Music.Enums;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;
using Kasane2D.Sound.AudioEffects;

namespace Kasane2D.Music.Synthesis.Effects;

internal class VoicePingPongDelay : VoiceEffect
{
    private readonly KasanePingPongDelay effect;

    public VoicePingPongDelay(KasanePingPongDelay effect)
    {
        this.effect = effect;
    }

    public override bool Bypass => effect.Bypass;
    
    public Sequencer Sequencer { get; set; } = null!;

    public override void ControlUpdate(EffectUpdate ev)
    {
        if (ev.EffectName != effect.Name)
        {
            return;
        }

        if (ev is not PingPongDelayUpdate actual)
        {
            return;
        }

        if (actual.Bypass is not null)
        {
            effect.Bypass = actual.Bypass.Value;
        }
        if (actual.Delay is not null)
        {
            effect.Delay = actual.Delay.Value.CalculateDelayTime(Sequencer.CurrentBpm, Sequencer.BeatsPerBar);
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
    
    public override void Reset()
    {
        effect.Reset();
    }
}