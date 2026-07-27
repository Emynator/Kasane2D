using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class MixBus : IMixBus
{
    public MixBus
        (
        string name,
        AudioBuffer outLeft,
        AudioBuffer outRight,
        AudioBuffer inLeft,
        AudioBuffer inRight,
        MixBus? parent
        )
    {
        Name = name;
        OutLeft = outLeft;
        OutRight = outRight;
        InLeft = inLeft;
        InRight = inRight;
        InternalParent = parent;
        parent?.InternalChildren.Add(this);
    }

    public string Name { get; }

    public int Level
    {
        get => (int)(20.0f * MathF.Log10(Gain));
        set => Gain = MathF.Pow(10.0f, value / 20.0f);
    }

    public int Pan { get; set; }

    public float Gain { get; set; }

    public IAudioBuffer OutLeft { get; }

    public IAudioBuffer OutRight { get; }

    public IAudioBuffer InLeft { get; }

    public IAudioBuffer InRight { get; }

    public IMixBus? Parent => InternalParent;

    public MixBus? InternalParent { get; set; }

    public IReadOnlyCollection<IMixBus> Children => InternalChildren;

    public List<MixBus> InternalChildren { get; set; } = [];

    public void Mix(int sampleCount)
    {
        if (InternalChildren.Count >= 5)
        {
            Parallel.ForEach(InternalChildren, child => child.Mix(sampleCount));
        }
        else
        {
            foreach (var child in InternalChildren)
            {
                child.Mix(sampleCount);
            }
        }

        var sumLeft = InternalChildren
            .Select(c => c.OutLeft.Read(sampleCount))
            .Aggregate
            (
                new float[sampleCount],
                (sum, next) =>
                {
                    for (var i = 0; i < sum.Length; i++)
                    {
                        sum[i] += next[i];
                    }

                    return sum;
                }
            );

        var sumRight = InternalChildren
            .Select(c => c.OutRight.Read(sampleCount))
            .Aggregate
            (
                new float[sampleCount],
                (sum, next) =>
                {
                    for (var i = 0; i < sum.Length; i++)
                    {
                        sum[i] += next[i];
                    }

                    return sum;
                }
            );

        var inLeft = InLeft.Read(sampleCount);
        var inRight = InRight.Read(sampleCount);
        for (var i = 0; i < sampleCount; i++)
        {
            sumLeft[i] += inLeft[i];
            sumRight[i] += inRight[i];
        }

        OutLeft.Write(sumLeft);
        OutRight.Write(sumRight);
    }
}