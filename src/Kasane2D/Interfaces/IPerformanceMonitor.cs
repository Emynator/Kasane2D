namespace Kasane2D.Interfaces;

public interface IPerformanceMonitor
{
    public bool IsActive { get; set; }

    public void StartMeasurement(string systemKey);
    
    public void FinishMeasurement(string systemKey);

    public void Tick(float dt);

    public void FinalPrint();
}