namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// The main audio mixer of the sound system.
/// </summary>
public interface IAudioMixer
{
    /// <summary>
    /// The master bus that will be output to the sound device.
    /// </summary>
    public IMixBus Master { get; }
    
    /// <summary>
    /// Creates a new mix bus.
    /// </summary>
    /// <param name="name">Name of the new mix bus.</param>
    /// <param name="parent">Optional: parent mix bus the new mix bus will feed into. Will be master if null.</param>
    /// <returns>The created mix bus.</returns>
    public IMixBus CreateMixBus(string name, IMixBus? parent = null);
    
    /// <summary>
    /// Free a mix bus and remove it from the mixer.
    /// </summary>
    /// <param name="bus">The mix bus to be freed.</param>
    public void ReleaseMixBus(IMixBus bus);
}