namespace Kasane2D.Sound.Interfaces;

/// <summary>
/// Interface for a custom sound subsystem that wants to integrate with the main sound system.
/// </summary>
public interface ISoundSubSystem
{
    /// <summary>
    /// GUID of the subsystem.
    /// </summary>
    /// <remarks>Should be unique per instance since it is used to identify the system for removal on runtime.</remarks>
    public Guid Id { get; }
    
    /// <summary>
    /// Process a single buffer worth of samples.
    /// </summary>
    public void Process();
}