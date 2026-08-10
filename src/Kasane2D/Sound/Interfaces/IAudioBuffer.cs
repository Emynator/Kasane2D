namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Represents a ring buffer for audio applications.
/// </summary>
/// <remarks>The buffer is a continuously running ring buffer. There are no checks if either the read or write index
/// surpasses the other. If the buffer is read faster than it is consumed, previously written samples will be read and
/// vice versa. All buffer operations are thread safe.</remarks>
public interface IAudioBuffer
{
    /// <summary>
    /// The total buffer length in samples.
    /// </summary>
    public int Length { get; }
    
    /// <summary>
    /// Read a single sample value and advances the read index.
    /// </summary>
    /// <returns>The read sample value.</returns>
    public float Read();

    /// <summary>
    /// Reads a determined number of samples and advances the read index.
    /// </summary>
    /// <param name="sampleCount">Number of samples to read.</param>
    /// <returns>The read samples.</returns>
    public float[] Read(int sampleCount);

    /// <summary>
    /// Writes a determined number of samples to a provided span.
    /// </summary>
    /// <param name="outBuffer">Span where the samples will be written to. Length of the spam is the number of samples
    /// that will be read.</param>
    public void Read(Span<float> outBuffer);
    
    /// <summary>
    /// Writes a single sample into the buffer and advances the write index.
    /// </summary>
    /// <param name="sample">The sample value to write into the buffer.</param>
    public void Write(float sample);
    
    /// <summary>
    /// Writes a determined number of samples from a provided span into the buffer and advances the write index.
    /// </summary>
    /// <param name="samples">Span to take the samples from. Length of the span is the number of samples that
    /// will be written.</param>
    public void Write(ReadOnlySpan<float> samples);
}