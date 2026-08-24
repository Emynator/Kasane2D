using System.Diagnostics;
using Kasane2D.Interfaces;
using Kasane2D.Types;

namespace Kasane2D;

internal class PerformanceMonitor : IPerformanceMonitor
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly Dictionary<string, PerformanceMeasure> measurements = new();
    private readonly Dictionary<string, long> timestamps = new();
    private float time = 0.0f;
    private int cycles = 0;
    private float logTime = 0.0f;

    public bool IsActive { get; set; } = false;

    public float CycleLength { get; set; } = 1.0f;

    public int CycleCount { get; set; } = 60;

    public bool CliLogging { get; set; } = false;

    public float LogInterval { get; set; } = 1.0f;

    public bool FileLogging { get; set; } = false;

    public void StartMeasurement(string systemKey)
    {
        timestamps[systemKey] = Stopwatch.GetTimestamp();
    }

    public void FinishMeasurement(string systemKey)
    {
        if (!IsActive)
        {
            return;
        }

        tlock.Wait();

        var result = Stopwatch.GetElapsedTime(timestamps[systemKey]).TotalMilliseconds;
        if (!measurements.TryGetValue(systemKey, out var measurement))
        {
            measurement = new();
            measurements[systemKey] = measurement;
        }

        if (double.IsNaN(measurement.Best) || measurement.Best > result)
        {
            measurement.Best = result;
        }

        if (double.IsNaN(measurement.Worst) || measurement.Worst < result)
        {
            measurement.Worst = result;
        }

        measurement.Measurements.Add(result);

        tlock.Release();
    }

    public void Tick(float dt)
    {
        if (!IsActive)
        {
            return;
        }

        tlock.Wait();

        logTime += dt;
        if (logTime >= LogInterval)
        {
            logTime = 0.0f;
            Print();
        }

        time += dt;
        if (time < CycleLength)
        {
            tlock.Release();
            return;
        }

        time = 0.0f;
        foreach (var measurement in measurements.Values)
        {
            if (measurement.Measurements.Count == 0)
            {
                continue;
            }

            measurement.MediumTermAverages.Add(measurement.Measurements.Average());
            measurement.Measurements = [];
        }

        cycles++;
        if (cycles < CycleCount)
        {
            tlock.Release();
            return;
        }

        cycles = 0;
        foreach (var measurement in measurements.Values)
        {
            if (measurement.MediumTermAverages.Count == 0)
            {
                continue;
            }

            measurement.LongTermAverages.Add(measurement.MediumTermAverages.Average());
            measurement.MediumTermAverages = [];
        }

        tlock.Release();
    }

    public void FinalPrint()
    {
        if (!IsActive)
        {
            return;
        }
        
        Console.WriteLine("\n\n\n");
        Console.WriteLine("===PERFORMANCE LOG===");
        foreach (var measurement in measurements)
        {
            Console.WriteLine($"{measurement.Key} - {measurement.Value}");
        }
    }

    private void Print()
    {
        if (!CliLogging && !FileLogging)
        {
            return;
        }

        using var file = FileLogging
            ? new StreamWriter(File.OpenWrite("PerformanceLog.log"))
            : null;

        file?.WriteLine("\n");
        file?.WriteLine(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        if (CliLogging)
        {
            Console.WriteLine("\n\n\n");
            Console.WriteLine("===PERFORMANCE LOG===");
        }

        foreach (var measurement in measurements)
        {
            file?.WriteLine($"{measurement.Key} - {measurement.Value}");
            if (CliLogging)
            {
                Console.WriteLine($"{measurement.Key} - {measurement.Value}");
            }
        }
    }
}