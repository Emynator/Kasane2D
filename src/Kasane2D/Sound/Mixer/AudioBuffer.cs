using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.Mixer;

internal class AudioBuffer : IAudioBuffer
{
    private readonly SemaphoreSlim tlock = new(1, 1);
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
        tlock.Wait();
        
        var result = data[readIndex];
        readIndex++;
        if (readIndex >= data.Length)
        {
            readIndex = 0;
        }
        
        tlock.Release();
        
        return result;
    }

    public float[] Read(int sampleCount)
    {
        var result = new float[sampleCount];
        Read(result);
        
        return result;
    }

    public void Read(Span<float> outBuffer)
    {
        tlock.Wait();
        
        if (readIndex + outBuffer.Length >= data.Length)
        {
            var toEnd = data.Length - readIndex;
            data.AsSpan(readIndex, toEnd).CopyTo(outBuffer);
            
            var fromStart = data.Length - toEnd;
            data.AsSpan(0, fromStart).CopyTo(outBuffer.Slice(toEnd, fromStart));
            
            readIndex = fromStart;
            tlock.Release();

            return;
        }

        data.AsSpan(readIndex, data.Length).CopyTo(outBuffer);
        readIndex += data.Length;
        
        tlock.Release();
    }

    public void Write(float sample)
    {
        tlock.Wait();
        
        data[writeIndex] = sample;
        writeIndex++;
        if (writeIndex >= data.Length)
        {
            writeIndex = 0;
        }
        
        tlock.Release();
    }

    public void Write(ReadOnlySpan<float> samples)
    {
        tlock.Wait();
        
        if (writeIndex + samples.Length >= data.Length)
        {
            var toEnd = data.Length - writeIndex;
            samples.Slice(0, toEnd).CopyTo(data.AsSpan(writeIndex, toEnd));
            
            var fromStart = samples.Length - toEnd;
            samples.Slice(toEnd, fromStart).CopyTo(data);
            
            writeIndex = fromStart;
            tlock.Release();

            return;
        }
        
        samples.Slice(0, samples.Length).CopyTo(data.AsSpan(writeIndex, samples.Length));
        writeIndex += samples.Length;
        
        tlock.Release();
    }
}