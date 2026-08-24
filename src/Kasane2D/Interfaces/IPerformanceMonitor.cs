namespace Kasane2D.Interfaces;

/// <summary>
/// Represents a performance monitor for the engine.
/// </summary>
public interface IPerformanceMonitor
{
    /// <summary>
    /// Starts the time measurement of a system.
    /// </summary>
    /// <param name="systemKey">The identification key of the system being measured.</param>
    public void StartMeasurement(string systemKey);
    
    /// <summary>
    /// Finishes the time measurement of a system.
    /// </summary>
    /// <param name="systemKey">The identification key of the system being measured.</param>
    public void FinishMeasurement(string systemKey);

    /// <summary>
    /// Updates the performance monitor. This function should only be called by the backend.
    /// </summary>
    /// <param name="dt">Deltatime since the last update call.</param>
    public void Tick(float dt);

    /// <summary>
    /// Does a final print to the console before exiting. This should only be called by the backend.
    /// </summary>
    public void FinalPrint();
}