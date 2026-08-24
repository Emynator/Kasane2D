namespace Kasane2D.Config;

public class PerformanceMonitorConfiguration
{
    public float ShortTermCycleLength { get; set; } = 1.0f;

    public int MediumTermCycles { get; set; } = 60;
    
    public bool CliLogging { get; set; } = false;

    public float LogInterval { get; set; } = 1.0f;
    
    public bool FileLogging { get; set; } = false;
}