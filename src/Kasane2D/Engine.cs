using Kasane2D.Config;
using Kasane2D.Interfaces;

namespace Kasane2D;

/// <summary>
/// The core engine class managing the execution.
/// </summary>
public sealed class Engine
{
    private static readonly PerformanceMonitor monitor = new();

    /// <summary>
    /// A performance monitor to log performance metrics across the engine.
    /// </summary>
    public static IPerformanceMonitor Monitor => monitor;

    private readonly IEngineRunner runner;
    private readonly Action initRenderer;
    private bool isDisposed = false;

    internal Engine(IEngineRunner runner, Action initRenderer, PerformanceMonitorConfiguration? perfConfig)
    {
        this.runner = runner;
        this.initRenderer = initRenderer;
        
        if (perfConfig is null)
        {
            return;
        }
        
        monitor.IsActive = true;
        monitor.CycleLength = perfConfig.MediumTermCycleLength;
        monitor.CycleCount = perfConfig.MediumTermCycles;
        monitor.CliLogging = perfConfig.CliLogging;
        monitor.LogInterval = perfConfig.LogInterval;
        monitor.FileLogging = perfConfig.FileLogging;
    }

    /// <summary>
    /// Starts execution.
    /// </summary>
    public void Run()
    {
        if (isDisposed)
        {
            return;
        }

        runner.Init(initRenderer);
        runner.Run();
        runner.Dispose();
        isDisposed = true;
    }
}