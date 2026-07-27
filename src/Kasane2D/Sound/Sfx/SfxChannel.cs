using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Sfx;

internal class SfxChannel
{
    private readonly IMixBus bus;
    private int position = 0;

    public SfxChannel(IMixBus bus)
    {
        this.bus = bus;
    }

    public AudioFileStream? CurrentFile
    {
        get;
        set
        {
            position = 0;
            field = value;
        }
    }

    public void Update(int sampleCount)
    {
        if (CurrentFile is null)
        {
            var samples = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                samples[i] = 0.0f;
            }

            bus.InLeft.Write(samples);
            bus.InRight.Write(samples);

            return;
        }
        
        var stream = CurrentFile.Read(position, sampleCount);
        switch (stream)
        {
            case MonoAudioStream monoStream:
                bus.InLeft.Write(monoStream.Samples);
                bus.InRight.Write(monoStream.Samples);
                break;

            case StereoAudioStream stereoStream:
                bus.InLeft.Write(stereoStream.Left);
                bus.InRight.Write(stereoStream.Right);
                break;
        }

        if (stream.Length >= sampleCount)
        {
            position += sampleCount;
            
            return;
        }

        CurrentFile = null;
        var fillSamples = new float[sampleCount - stream.Length];
        for (var i = 0; i < fillSamples.Length; i++)
        {
            fillSamples[i] = 0.0f;
        }

        bus.InLeft.Write(fillSamples);
        bus.InRight.Write(fillSamples);
    }
}