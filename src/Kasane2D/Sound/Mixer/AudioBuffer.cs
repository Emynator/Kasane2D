using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class AudioBuffer : IAudioBuffer
{
    private readonly float[] data;
    private int writeIndex = 0;
    private int readIndex = 0;

    public AudioBuffer(int size)
    {
        data = new float[size];
    }
    
    public int Length => data.Length;
    
    public float Read()
    {
        var result = data[readIndex];
        readIndex++;
        if (readIndex >= data.Length)
        {
            readIndex = 0;
        }
        
        return result;
    }

    public float[] Read(int sampleCount)
    {
        var result = new float[sampleCount];
        if (readIndex + sampleCount >= data.Length)
        {
            var toEnd = data.Length - readIndex;
            data.AsSpan(readIndex, toEnd).CopyTo(result);
            
            var fromStart = sampleCount - toEnd;
            data.AsSpan(0, fromStart).CopyTo(result.AsSpan(toEnd, fromStart));
            
            readIndex = fromStart;

            return result;
        }

        data.AsSpan(readIndex, sampleCount).CopyTo(result);
        readIndex += sampleCount;
        
        return result;
    }

    public void Write(float sample)
    {
        data[writeIndex] = sample;
        writeIndex++;
        if (writeIndex >= data.Length)
        {
            writeIndex = 0;
        }
    }

    public void Write(float[] samples)
    {
        if (writeIndex + samples.Length >= data.Length)
        {
            var toEnd = data.Length - writeIndex;
            samples.AsSpan(0, toEnd).CopyTo(data.AsSpan(writeIndex, toEnd));
            
            var fromStart = samples.Length - toEnd;
            samples.AsSpan(toEnd, fromStart).CopyTo(data);
            
            writeIndex = fromStart;

            return;
        }
        
        samples.AsSpan(0, samples.Length).CopyTo(data.AsSpan(writeIndex, samples.Length));
        writeIndex += samples.Length;
    }
}