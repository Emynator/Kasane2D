using System.Diagnostics.CodeAnalysis;
using Kasane2D.Config;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class AudioMixer : IAudioMixer
{
    private readonly int bufferSize;
    private readonly Dictionary<string, IMixBus> busses = new();

    public AudioMixer(AudioConfiguration config)
    {
        bufferSize = (int)(config.SampleRate / 1000.0f * config.DefaultBufferSizeInMs);
        var outLeft = new AudioBuffer(bufferSize);
        var outRight = new AudioBuffer(bufferSize);
        var inLeft = new AudioBuffer(bufferSize);
        var inRight = new AudioBuffer(bufferSize);
        InternalMaster = new MixBus("Master", outLeft, outRight, inLeft, inRight, null);
        busses.Add("Master", InternalMaster);
    }

    public IMixBus Master => InternalMaster;
    
    public MixBus InternalMaster { get; }

    public IMixBus CreateMixBus(string name, IMixBus? parent = null)
    {
        if (parent is not null && parent is not MixBus)
        {
            throw new ArgumentException($"Incompatible implementations of {nameof(IMixBus)}.", nameof(parent));
        }

        var parentBus = parent as MixBus;

        var outLeft = new AudioBuffer(bufferSize);
        var outRight = new AudioBuffer(bufferSize);
        var inLeft = new AudioBuffer(bufferSize);
        var inRight = new AudioBuffer(bufferSize);
        var result = new MixBus(name, outLeft, outRight, inLeft, inRight, parentBus);
        if (parent is null)
        {
            result.InternalParent = InternalMaster;
            InternalMaster.InternalChildren.Add(result);
        }
        
        busses.Add(name, result);

        return result;
    }

    public void ReleaseMixBus(IMixBus bus)
    {
        if (bus is not MixBus mixBus)
        {
            throw new ArgumentException($"Incompatible implementations of {nameof(IMixBus)}.", nameof(bus));
        }

        if (mixBus.InternalParent is not null)
        {
            mixBus.InternalParent.InternalChildren.Remove(mixBus);

            return;
        }
        
        busses.Remove(mixBus.Name);
        InternalMaster.InternalChildren.Remove(mixBus);
    }

    public bool TryGetMixBus(string name, [NotNullWhen(true)] out IMixBus? bus)
    {
        return busses.TryGetValue(name, out bus);
    }
}