using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Sfx;

internal class SfxChannel
{
    private readonly IMixBus bus;
    private readonly float[] scratchBuffer0;
    private readonly float[] scratchBuffer1;
    private int position = 0;

    public SfxChannel(IMixBus bus, int bufferSize)
    {
        this.bus = bus;
        scratchBuffer0 = new float[bufferSize];
        scratchBuffer1 = new float[bufferSize];
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

    public void Update()
    {
        var left = scratchBuffer0.AsSpan();
        left.Clear();

        if (CurrentFile is null)
        {
            bus.WriteLeft(left);
            bus.WriteRight(left);

            return;
        }

        if (CurrentFile.Slice(position, left.Length) is not StereoAudioStream stream)
        {
            throw new InvalidOperationException();
        }
        
        position += left.Length;
        var right = scratchBuffer1.AsSpan();
        right.Clear();

        stream.GetLeft().CopyTo(left);
        stream.GetRight().CopyTo(right);
        
        bus.WriteLeft(left);
        bus.WriteRight(right);

        if (stream.Length < left.Length)
        {
            CurrentFile = null;
        }
    }
}