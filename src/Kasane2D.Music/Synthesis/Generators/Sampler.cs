using Kasane2D.Music.Enums;
using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Synthesis.Generators;

public class Sampler : Generator
{
    private Dictionary<Note, MonoAudioStream> samples = new();

    public Sampler(int sampleRate) : base(sampleRate)
    {
    }

    public override void ControlUpdate(GeneratorUpdate ev)
    {
        if (ev is not SamplerUpdate actual)
        {
            return;
        }

        samples = actual.SampleAssignments;
    }

    protected override float Generate(double frequency)
    {
        if (samples.Count == 0)
        {
            Step(frequency);

            return 0.0f;
        }

        var sample = MonoAudioStream.Empty;
        if (samples.Count == 1)
        {
            sample = samples.First().Value;
        }
        else
        {
            var availableSamples = samples.Keys.Select(k => (Note: k, Frequency: k.Frequency())).ToList();
            var closestNote = Note.None;
            var closestFrequency = 0.0d;
            foreach (var s in availableSamples)
            {
                if (s.Frequency > closestFrequency)
                {
                    var lowerDistance = Math.Abs(closestFrequency - frequency);
                    var upperDistance = Math.Abs(s.Frequency - frequency);
                    var selection = lowerDistance - upperDistance;
                    if (selection < 0.0d)
                    {
                        sample = samples[closestNote];
                    }
                    sample = samples[s.Note];

                    break;
                }
                closestNote = s.Note;
                closestFrequency = s.Frequency;
            }
        }

        if (sample.Length == 0)
        {
            Step(frequency);

            return 0.0f;
        }

        var index = Phase + 1.0f * sample.Length / 2.0f;
        var indexLower = (int)Math.Floor(index);
        indexLower = indexLower < sample.Length ? indexLower : sample.Length - 1;
        var indexUpper = (int)Math.Ceiling(index);
        indexUpper = indexUpper < sample.Length ? indexUpper : sample.Length - 1;

        var sampleLower = sample.GetSamples()[indexLower];
        var sampleUpper = sample.GetSamples()[indexUpper];
        var t = Math.Min(1.0d, Math.Max(0.0d, index - indexLower));
        var result = float.Lerp(sampleLower, sampleUpper, (float)t);

        Step(frequency);

        return result;
    }
}