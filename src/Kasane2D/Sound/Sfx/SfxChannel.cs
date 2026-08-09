using System.Buffers;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Sfx;

internal class SfxChannel
{
    private static readonly ArrayPool<float> bufferPool = ArrayPool<float>.Shared;
    
    private readonly IMixBus bus;
    private int position = 0;

    public SfxChannel(IMixBus bus)
    {
        this.bus = bus;
        bus.Level = -3;
    }

    public StereoAudioStream? CurrentFile
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
            var buffer = bufferPool.Rent(sampleCount);
            var samples = buffer.AsSpan().Slice(0, sampleCount);
            samples.Clear();
            
            bus.WriteLeft(samples);
            bus.WriteRight(samples);
            
            bufferPool.Return(buffer);

            return;
        }

        if (CurrentFile.Slice(position, sampleCount) is not StereoAudioStream stream)
        {
            throw new InvalidOperationException();
        }

        bus.WriteLeft(stream.GetLeft());
        bus.WriteRight(stream.GetRight());

        if (stream.Length >= sampleCount)
        {
            position += sampleCount;
            
            return;
        }

        CurrentFile = null;
        
        var fillLength = sampleCount - stream.Length;
        var fillBuffer = bufferPool.Rent(fillLength);
        var fillSamples = fillBuffer.AsSpan().Slice(0, fillLength);
        fillSamples.Clear();
        
        bus.WriteLeft(fillSamples);
        bus.WriteRight(fillSamples);
        
        bufferPool.Return(fillBuffer);
    }
}