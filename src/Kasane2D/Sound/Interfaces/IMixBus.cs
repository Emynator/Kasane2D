namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Represents a mix bus of the audio mixer.
/// </summary>
public interface IMixBus
{
    /// <summary>
    /// Name of the mix bus.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gain in db that will be applied to the mix bus.
    /// </summary>
    /// <remarks><seealso href="https://en.wikipedia.org/wiki/Decibel#Acoustics"/></remarks>
    public int Gain { get; set; }
    
    /// <summary>
    /// Current pan of the bus.
    /// </summary>
    /// <remarks>Ranges from -100 to 100 where -100 is fully left and 100 is fully right.</remarks>
    public int Pan { get; set; }
    
    /// <summary>
    /// Gets the parent bus this mix bus feeds into.
    /// </summary>
    IMixBus? Parent { get; }
    
    /// <summary>
    /// Gets the child busses that feed into this mix bus.
    /// </summary>
    IReadOnlyCollection<IMixBus> Children { get; }
    
    /// <summary>
    /// Audio effects to be applied to this mix bus.
    /// </summary>
    /// <remarks>Audio effects are applied sequentially in order, but before pan or gain.</remarks>
    IReadOnlyCollection<IAudioEffect> Effects { get; }

    /// <summary>
    /// Adds an audio effect to the end of the effect chain.
    /// </summary>
    /// <param name="effect">The audio effect to add.</param>
    public void AddEffect(IAudioEffect effect);
    
    /// <summary>
    /// Remove an audio effect with the provided name from the effect chain.
    /// </summary>
    /// <param name="name">The name of the audio effect to remove.</param>
    public void RemoveEffect(string name);
    
    /// <summary>
    /// Replaces the list of existing audio effects with a new list of audio effects.
    /// </summary>
    /// <param name="effects">The new list of audio effects.</param>
    public void SetEffects(IReadOnlyCollection<IAudioEffect> effects);

    /// <summary>
    /// Removes all audio effects from this mix bus.
    /// </summary>
    public void ClearEffects();

    /// <summary>
    /// Writes samples into the left channel of the mix bus.
    /// </summary>
    /// <param name="samples">Span of samples to write.</param>
    /// <remarks>The input of the mix bus is summed with the result of all children before it is fed through the effect
    /// chain and finally pan and gain before it is written to the output.</remarks>
    public void WriteLeft(ReadOnlySpan<float> samples);

    /// <summary>
    /// Writes samples into the right channel of the mix bus.
    /// </summary>
    /// <param name="samples">Span of samples to write.</param>
    /// <remarks>The input of the mix bus is summed with the result of all children before it is fed through the effect
    /// chain and finally pan and gain before it is written to the output.</remarks>
    public void WriteRight(ReadOnlySpan<float> samples);

    /// <summary>
    /// Reads a number of samples from the left channel of the mix bus.
    /// </summary>
    /// <param name="outBuffer">Span where the samples will be written to. Length of the spam is the number of samples
    /// that will be read.</param>
    /// <remarks>This method should only be called by the backend to retrieve audio data after it is mixed.</remarks>
    public void ReadLeft(Span<float> outBuffer);

    /// <summary>
    /// Reads a number of samples from the right channel of the mix bus.
    /// </summary>
    /// <param name="outBuffer">Span where the samples will be written to. Length of the spam is the number of samples
    /// that will be read.</param>
    /// <remarks>This method should only be called by the backend to retrieve audio data after it is mixed.</remarks>
    public void ReadRight(Span<float> outBuffer);
}