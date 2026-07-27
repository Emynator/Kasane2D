using Kasane2D.Sound.Enums;

namespace Kasane2D.Sound.Types;

public abstract class AudioFileStream : IDisposable
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly Task preLoadTask;
    private readonly int targetSampleRate;
    private bool isDisposed = false;

    protected readonly AudioFileReadMode readMode;
    protected readonly BinaryReader file;
    protected int sampleRate = 0;
    protected AudioStream? data = null;
    protected readonly EventWaitHandle initDone = new(false, EventResetMode.AutoReset);

    protected AudioFileStream(string path, int targetSampleRate, AudioFileReadMode readMode)
    {
        this.targetSampleRate = targetSampleRate;
        this.readMode = readMode;
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        file = new BinaryReader(File.OpenRead(path));
        preLoadTask = readMode == AudioFileReadMode.Preload ? Task.Run(PreLoad) : Task.CompletedTask;
    }

    public int CurrentPosition { get; private set; } = 0;

    public int Length { get; protected set; } = 0;

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        file.Dispose();
        isDisposed = true;
    }
    
    public AudioStream Read(int offset, int sampleCount)
    {
        tlock.Wait();
        if (CurrentPosition != offset)
        {
            SetPosition(offset);
        }

        var result = PrivateRead(sampleCount);
        tlock.Release();
        
        return result;
    }

    public AudioStream Read(int sampleCount)
    {
        tlock.Wait();
        var result = PrivateRead(sampleCount);
        tlock.Release();
        
        return result;
    }

    private AudioStream PrivateRead(int sampleCount)
    {
        preLoadTask.Wait();
        
        var length = CurrentPosition + sampleCount < Length ? sampleCount : Length - CurrentPosition;
        if (readMode == AudioFileReadMode.Stream)
        {
            var rawData = ReadRawSamples(sampleCount);
            var streamResult = Convert(length, rawData);

            return sampleRate == targetSampleRate ? streamResult : streamResult.Resample(sampleRate, targetSampleRate);
        }

        if (data is null)
        {
            if (readMode == AudioFileReadMode.CachedStream)
            {
                var rawData = ReadRawSamples(sampleCount);
                data = Convert(sampleCount, rawData);
            }
            else
            {
                var rawData = ReadRawSamples(Length);
                data = Convert(Length, rawData);
            }

            if (sampleRate != targetSampleRate)
            {
                data = data.Resample(sampleRate, targetSampleRate);
            }
        }
        
        if (readMode == AudioFileReadMode.CachedStream && CurrentPosition + length > data.Length)
        {
            var toReadIn = length - (data.Length - CurrentPosition);
            var rawData = ReadRawSamples(sampleCount);// 
            var streamResult = Convert(length, rawData);
            data.Add
            (
                sampleRate == targetSampleRate
                    ? streamResult
                    : streamResult.Resample(sampleRate, targetSampleRate)
            );
        }

        var result = data.Slice(CurrentPosition, length);
        CurrentPosition += length;

        return result;
    }

    public virtual void SetPosition(int value)
    {
        var newPos = value;
        if (value < 0)
        {
            newPos = 0;
        }
        if (value > Length)
        {
            newPos = Length - 1;
        }

        CurrentPosition = newPos;
    }

    public void Reset()
    {
        SetPosition(0);
    }
    
    protected abstract byte[] ReadRawSamples(int sampleCount);

    protected abstract AudioStream Convert(int sampleCount, Span<byte> rawData);

    private void PreLoad()
    {
        initDone.WaitOne();
        
        var rawData = ReadRawSamples(Length);
        var result = Convert(Length, rawData);

        data = sampleRate == targetSampleRate ? result : result.Resample(sampleRate, targetSampleRate);
        Dispose();
    }
}