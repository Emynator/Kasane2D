namespace Kasane2D.Config;

/// <summary>
/// Configuration of the performance monitoring system.
/// </summary>
public class PerformanceMonitorConfiguration
{
    /// <summary>
    /// Configures the length of a medium term measurement cycles in seconds. Default is 1 second.
    /// </summary>
    public float MediumTermCycleLength { get; set; } = 1.0f;

    /// <summary>
    /// Configures the amount of medium term measurements in a long term measurement. Default is 60.
    /// </summary>
    public int MediumTermCycles { get; set; } = 60;
    
    /// <summary>
    /// Configures if the performance monitor should log to the CLI.
    /// </summary>
    public bool CliLogging { get; set; } = false;

    /// <summary>
    /// Configures if the performance monitor should log to a logfile.
    /// </summary>
    public bool FileLogging { get; set; } = false;
    
    /// <summary>
    /// Configures the logging interval of the performance monitor.
    /// </summary>
    public float LogInterval { get; set; } = 1.0f;
    
}