using Kasane2D.Sound.Extensions;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Sfx;

internal class SfxChannel
{
    private readonly IMixBus bus;
    private readonly int bufferSize;
    private readonly float[] scratchBuffer;
    private int position = 0;

    public SfxChannel(IMixBus bus, int bufferSize)
    {
        this.bus = bus;
        this.bufferSize = bufferSize;
        scratchBuffer = new float[bufferSize];
        bus.Level = -3;
    }

    public AudioStream? CurrentFile
    {
        get;
        set
        {
            position = 0;
            field = value;
        }
    }

    public void Update()
    {
        var zeroBuffer = scratchBuffer.AsSpan();
        zeroBuffer.Clear();

        if (CurrentFile is null)
        {
            bus.WriteLeft(zeroBuffer);
            bus.WriteRight(zeroBuffer);

            return;
        }

        var stream = CurrentFile.Slice(position, bufferSize).AsStereoStream();
        bus.WriteLeft(stream.GetLeft());
        bus.WriteRight(stream.GetRight());
        position += stream.Length;

        if (position < CurrentFile.Length)
        {
            return;
        }

        position = 0;
        CurrentFile = null;
        
        var remaining = zeroBuffer.Slice(0, bufferSize - stream.Length);
        bus.WriteLeft(remaining);
        bus.WriteRight(remaining);
    }
}