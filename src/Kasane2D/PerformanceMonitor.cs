using System.Diagnostics;
using Kasane2D.Interfaces;
using Kasane2D.Types;

namespace Kasane2D;

internal class PerformanceMonitor : IPerformanceMonitor
{
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
        
        var result = Stopwatch.GetElapsedTime(timestamps[systemKey]).TotalMilliseconds;
        if (!measurements.TryGetValue(systemKey, out var measurement))
        {
            measurement = new();
            measurements[systemKey] = measurement;
        }
        
        if (measurement.Best > result)
        {
            measurement.Best = result;
        }

        if (measurement.Worst < result)
        {
            measurement.Worst = result;
        }
        
        measurement.Measurements.Add(result);
    }

    public void Tick(float dt)
    {
        if (!IsActive)
        {
            return;
        }

        logTime += dt;
        if (logTime >= LogInterval && CliLogging)
        {
            logTime = 0.0f;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("===PERFORMANCE LOG===");
            foreach (var measurement in measurements)
            {
                Console.WriteLine($"{measurement.Key} - {measurement.Value}");
            }
        }
        
        time += dt;
        if (time < CycleLength)
        {
            return;
        }

        time = 0.0f;
        foreach (var measurement in measurements.Values)
        {
            measurement.MediumTermAverages.Add(measurement.Measurements.Sum() / measurement.Measurements.Count);
            measurement.Measurements = [];
        }

        cycles++;
        if (cycles < CycleCount)
        {
            return;
        }
        
        cycles = 0;
        foreach (var measurement in measurements.Values)
        {
            measurement.LongTermAverage = measurement.MediumTermAverages.Sum() / measurement.MediumTermAverages.Count;
            measurement.MediumTermAverages = [];
        }
    }
}