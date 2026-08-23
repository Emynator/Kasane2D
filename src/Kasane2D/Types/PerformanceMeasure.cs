namespace Kasane2D.Types;

internal class PerformanceMeasure
{
    public List<double> Measurements { get; set; } = [];

    public List<double> MediumTermAverages { get; set; } = [];

    public double Best { get; set; } = -1.0f;

    public double Worst { get; set; } = -1.0f;

    public double LongTermAverage { get; set; } = -1.0f;

    public override string ToString()
    {
        return $"BEST: {
            Best
            :N2} - WORST: {
            Worst
            :N2} - CURRENT AVERAGE: {
            Measurements.Average()
            :N2} - MEDIUM TERM AVERAGE: {
            MediumTermAverages.Average()
            :N2} - LongTermAverage: {
            LongTermAverage
            :N2}";
    }
}