using Kasane2D.Sound.Enums;

namespace Kasane2D.Sound.Types;

/// <summary>
/// Abstract base class to represent an audio file to read from disk.
/// </summary>
public abstract class AudioFileStream : IDisposable
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly Task preLoadTask;
    private readonly int targetSampleRate;
    private bool isDisposed = false;
    private AudioStream? data = null;

    /// <summary>
    /// Mode of how the file will be read in.
    /// </summary>
    protected readonly AudioFileReadMode readMode;
    /// <summary>
    /// The sample rate of the audio file. If it differs from the sound system's sample rate, the file will be resampled.
    /// </summary>
    protected int sampleRate = 0;
    /// <summary>
    /// Signals that the initialization is done and audio data can be streamed now.
    /// </summary>
    /// <remarks>Deriving classes should signal this only when they have parsed the file header and are ready to
    /// actually stream audio data.</remarks>
    protected readonly EventWaitHandle initDone = new(false, EventResetMode.AutoReset);
    /// <summary>
    /// Binary reader wrapping the actual file stream.
    /// </summary>
    protected BinaryReader? file;

    /// <summary>
    /// Base constructor.
    /// </summary>
    /// <param name="path">Path to the audio file.</param>
    /// <param name="targetSampleRate">Sample rate of the sound system.</param>
    /// <param name="readMode">Read mode of the file.</param>
    /// <exception cref="FileNotFoundException">Thrown if the provided file does not exist.</exception>
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

    /// <summary>
    /// Gets the current stream position in samples.
    /// </summary>
    /// <remarks>The position number is independent of the number of channels. A position of 10 means that the stream is
    /// at the 11th sample of a mono stream or that both channels are at the 11th sample of a stereo stream.</remarks>
    public int CurrentPosition { get; private set; } = 0;

    /// <summary>
    /// Gets the total stream length in samples.
    /// </summary>
    /// <remarks>The number is independent of the number of channels. The number of total samples in the file would be
    /// length * channel count.</remarks>
    public int Length { get; protected set; } = 0;

    /// <summary>
    /// Disposes the stream.
    /// </summary>
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        file?.Dispose();
        file = null;
        isDisposed = true;
    }
    
    /// <summary>
    /// Reads a number of samples from the provided offset.
    /// </summary>
    /// <param name="offset">The offset to read from.</param>
    /// <param name="sampleCount">The number of samples to read.</param>
    /// <returns>The audio stream that has been read.</returns>
    /// <remarks>If the requested number of samples to read surpasses the length of the file, the returned audio stream
    /// will be shorter than the requested length.</remarks>
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

    /// <summary>
    /// Reads a number of samples from the current stream position.
    /// </summary>
    /// <param name="sampleCount">The number of samples to read.</param>
    /// <returns>The audio stream that has been read.</returns>
    /// <remarks>If the requested number of samples to read surpasses the length of the file, the returned audio stream
    /// will be shorter than the requested length.</remarks>
    public AudioStream Read(int sampleCount)
    {
        tlock.Wait();
        var result = PrivateRead(sampleCount);
        tlock.Release();
        
        return result;
    }

    /// <summary>
    /// Sets the current stream position to the provided value.
    /// </summary>
    /// <param name="value">The position to set the stream to.</param>
    /// <remarks>Values smaller than 0 or larger than the file size will be clamped respectively.</remarks>
    public virtual void SetPosition(int value)
    {
        tlock.Wait();
        
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
        
        tlock.Release();
    }

    /// <summary>
    /// Resets the stream position to the start.
    /// </summary>
    public void Reset()
    {
        tlock.Wait();
        
        SetPosition(0);
        
        tlock.Release();
    }

    /// <summary>
    /// Reads the raw bytes of the determined number of samples from disk.
    /// </summary>
    /// <param name="sampleCount">Number of samples to read.</param>
    /// <returns>The read bytes from the file.</returns>
    /// <remarks>The number of samples to read means samples per channel.</remarks>
    protected abstract byte[] ReadRawSamples(int sampleCount);

    /// <summary>
    /// Converts the raw sample data into a usable audio stream for the engine.
    /// </summary>
    /// <param name="sampleCount">The number of samples to convert.</param>
    /// <param name="rawData">A span of the raw bytes to convert.</param>
    /// <returns>Audio stream containing the samples in 32bit float PCM format.</returns>
    protected abstract AudioStream Convert(int sampleCount, Span<byte> rawData);

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
            var rawData = ReadRawSamples(toReadIn);
            var streamResult = Convert(rawData.Length, rawData);
            data.Append
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

    private void PreLoad()
    {
        initDone.WaitOne();
        
        var rawData = ReadRawSamples(Length);
        var result = Convert(Length, rawData);

        data = sampleRate == targetSampleRate ? result : result.Resample(sampleRate, targetSampleRate);
        Dispose();
    }
}