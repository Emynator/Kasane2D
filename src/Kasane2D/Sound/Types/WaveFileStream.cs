using System.Buffers.Binary;
using System.Text;
using Kasane2D.Sound.Enums;

namespace Kasane2D.Sound.Types;

/// <summary>
/// Implementation of an audio file stream for RIFF/WAVE files.
/// </summary>
/// <inheritdoc/>
public sealed class WaveFileStream : AudioFileStream
{
    private const float sBytePosFactor = 1.0f / sbyte.MaxValue;
    private const float sByteNegFactor = 1.0f / (sbyte.MinValue * -1);
    private const float shortPosFactor = 1.0f / short.MaxValue;
    private const float shortNegFactor = 1.0f / (short.MinValue * -1);
    private const float posFactor24Bit = 1.0f / 8388607;
    private const float negFactor24Bit = 1.0f / 8388608;
    private const float intPosFactor = 1.0f / int.MaxValue;
    private const float intNegFactor = 1.0f / ((long)int.MinValue * -1);

    private int fileSize = 0;
    private short numChannels = 0;
    private SampleFormat format = SampleFormat.Unknown;
    private int bytesPerSample = 0;
    private long dataStartPosition = 0;

    /// <summary>
    /// Creates a new wave file stream.
    /// </summary>
    /// <param name="path">The path to the wave file.</param>
    /// <param name="targetSampleRate">The sound system's sample rate.</param>
    /// <param name="readMode">The read mode for the file.</param>
    public WaveFileStream(string path, int targetSampleRate, AudioFileReadMode readMode = AudioFileReadMode.Preload)
        : base(path, targetSampleRate, readMode)
    {
        ParseHeader(path);
    }

    /// <inheritdoc/>
    public override void SetPosition(int value)
    {
        base.SetPosition(value);

        if (readMode == AudioFileReadMode.Stream)
        {
            file?.BaseStream.Seek(dataStartPosition + CurrentPosition, SeekOrigin.Begin);
        }
    }

    /// <inheritdoc/>
    protected override byte[] ReadRawSamples(int sampleCount)
    {
        return file?.ReadBytes(sampleCount * bytesPerSample * numChannels) ?? [];
    }

    /// <inheritdoc/>
    protected override AudioStream Convert(int sampleCount, Span<byte> rawData)
    {
        if (numChannels == 1)
        {
            var samples = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                samples[i] = ConvertSample(rawData.Slice(i * bytesPerSample, bytesPerSample));
            }

            return new MonoAudioStream(sampleCount, samples);
        }

        var left = new float[sampleCount];
        var right = new float[sampleCount];
        var dataIndex = 0;
        for (var i = 0; i < sampleCount; i++)
        {
            left[i] = ConvertSample(rawData.Slice(dataIndex, bytesPerSample));
            dataIndex += bytesPerSample;
            right[i] = ConvertSample(rawData.Slice(dataIndex, bytesPerSample));
            dataIndex += bytesPerSample;

            if (numChannels > 2)
            {
                dataIndex += bytesPerSample * (numChannels - 2);
            }
        }

        return new StereoAudioStream(sampleCount, left, right);
    }

    private void ParseHeader(string path)
    {
        if (file is null)
        {
            return;
        }

        var notWave = $"File '{path}' is not a wave file.";
        var fileFormat = ReadFourCC();
        if (fileFormat != "RIFF")
        {
            throw new InvalidDataException(notWave);
        }

        file.ReadBytes(4);
        var formatID = ReadFourCC();
        if (formatID != "WAVE")
        {
            throw new InvalidDataException(notWave);
        }

        var fourCC = ReadFourCC();
        while (fourCC != "data")
        {
            switch (fourCC)
            {
                case "fmt ":
                    ParseFmtBlock(path);
                    break;

                default:
                    SkipChunk();
                    break;
            }

            fourCC = ReadFourCC();
        }

        fileSize = BinaryPrimitives.ReadInt32LittleEndian(file.ReadBytes(4));
        Length = fileSize
            / format switch
            {
                SampleFormat.Int8 => 1,
                SampleFormat.Int16 => 2,
                SampleFormat.Int24 => 3,
                SampleFormat.Int32 => 4,
                SampleFormat.Float32 => 4,
                SampleFormat.Float64 => 8,
                _ => throw new InvalidDataException($"File '{path}' format not supported."),
            }
            / numChannels;

        dataStartPosition = file.BaseStream.Position;
        initDone.Set();
    }

    private void ParseFmtBlock(string path)
    {
        if (file is null)
        {
            return;
        }

        var formatSize = BinaryPrimitives.ReadInt32LittleEndian(file.ReadBytes(4));
        var waveFormat = BinaryPrimitives.ReadInt16LittleEndian(file.ReadBytes(2));
        if (waveFormat != 0x1 && waveFormat != 0x3)
        {
            throw new InvalidDataException($"File '{path}' is not a PCM wave file.");
        }

        numChannels = BinaryPrimitives.ReadInt16LittleEndian(file.ReadBytes(2));
        sampleRate = BinaryPrimitives.ReadInt32LittleEndian(file.ReadBytes(4));
        SkipBytes(6);
        var bitDepth = BinaryPrimitives.ReadInt16LittleEndian(file.ReadBytes(2));
        bytesPerSample = bitDepth / 8;
        if (waveFormat == 0x1)
        {
            format = bitDepth switch
            {
                8 => SampleFormat.Int8,
                16 => SampleFormat.Int16,
                24 => SampleFormat.Int24,
                32 => SampleFormat.Int32,
                _ => throw new InvalidDataException($"File '{path}' format not supported."),
            };
        }
        else
        {
            format = bitDepth switch
            {
                32 => SampleFormat.Float32,
                64 => SampleFormat.Float64,
                _ => throw new InvalidDataException($"File '{path}' format not supported."),
            };
        }

        if (formatSize != 18)
        {
            return;
        }

        var extraSize = BinaryPrimitives.ReadInt16LittleEndian(file.ReadBytes(2));
        SkipBytes(extraSize);
    }

    private void SkipBytes(int count)
    {
        file?.ReadBytes(count);
    }

    private void SkipChunk()
    {
        if (file is null)
        {
            return;
        }
        
        var size = BinaryPrimitives.ReadInt32LittleEndian(file.ReadBytes(4));
        size = (size & 1) != 0 ? size + 1 : size;
        SkipBytes(size);
    }

    private string ReadFourCC()
    {
        return file is not null
            ? Encoding.UTF8.GetString(file.ReadBytes(4))
            : "";
    }

    private float ConvertSample(Span<byte> rawData)
    {
        switch (format)
        {
            case SampleFormat.Int8:
                var b = (sbyte)rawData[0];
                return b >= 0 ? sBytePosFactor * b : sByteNegFactor * b;

            case SampleFormat.Int16:
                var s = BinaryPrimitives.ReadInt16LittleEndian(rawData);
                return s >= 0 ? shortPosFactor * s : shortNegFactor * s;

            case SampleFormat.Int24:
                var value = 0;
                value |= rawData[0];
                value |= rawData[1] << 8;
                value |= rawData[2] << 16;
                value = (value << 8) >> 8;

                var result = value >= 0 ? posFactor24Bit * value : negFactor24Bit * value;
                return result;

            case SampleFormat.Int32:
                var i = BinaryPrimitives.ReadInt32LittleEndian(rawData);
                return i >= 0 ? intPosFactor * i : intNegFactor * i;

            case SampleFormat.Float32:
                return BinaryPrimitives.ReadInt32LittleEndian(rawData);

            case SampleFormat.Float64:
                return (float)BinaryPrimitives.ReadDoubleLittleEndian(rawData);

            default:
                return 0.0f;
        }
    }

    private enum SampleFormat
    {
        Unknown,
        Int8,
        Int16,
        Int24,
        Int32,
        Float32,
        Float64,
    }
}