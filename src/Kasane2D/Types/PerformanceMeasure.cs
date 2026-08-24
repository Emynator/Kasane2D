namespace Kasane2D.Types;

internal class PerformanceMeasure
{
    public List<double> Measurements { get; set; } = [];

    public List<double> MediumTermAverages { get; set; } = [];
    
    public List<double> LongTermAverages { get; set; } = [];

    public double Best { get; set; } = float.NaN;

    public double Worst { get; set; } = float.NaN;

    public override string ToString()
    {
        var currentAverage = Measurements.Count > 0 ? Measurements.Average() : float.NaN;
        var mediumTermAverage = MediumTermAverages.Count > 0 ? MediumTermAverages.Average() : float.NaN;
        var longTermAverage = LongTermAverages.Count > 0 ? LongTermAverages.Average() : float.NaN;
        
        return $"BEST: {
            Best
            :N2} - WORST: {
            Worst
            :N2} - CURRENT AVERAGE: {
            currentAverage
            :N2} - MEDIUM TERM AVERAGE: {
            mediumTermAverage
            :N2} - LongTermAverage: {
            longTermAverage
            :N2}";
    }
}