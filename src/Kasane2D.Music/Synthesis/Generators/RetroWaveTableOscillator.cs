using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

public class RetroWaveTableOscillator : Generator
{
    private float[] wavetable = new float[32];
    
    public RetroWaveTableOscillator(int sampleRate) : base(sampleRate)
    {
    }

    public override void ControlUpdate(GeneratorUpdate ev)
    {
        if (ev is not RetroWaveTableOscillatorUpdate actual)
        {
            return;
        }

        if (actual.Table.Length < wavetable.Length)
        {
            var givenTable = actual
                .Table
                .Select(v => actual.IsByte ? (v & 0xFF) / 127.5f - 1.0f : (v & 0xF) / 7.5f - 1.0f)
                .ToArray();
            
            var indexGiven = 0.0f;
            var incrementGiven = (float)givenTable.Length / actual.Table.Length;
            for (var i = 0; i < wavetable.Length; i++)
            {
                var indexFirst = (int)MathF.Floor(indexGiven);
                indexFirst = indexFirst < givenTable.Length ? indexFirst : givenTable.Length - 1;
                var first = givenTable[indexFirst];

                var indexSecond = (int)MathF.Ceiling(indexGiven + incrementGiven);
                indexSecond = indexSecond < givenTable.Length ? indexSecond : givenTable.Length - 1;
                var second = givenTable[indexSecond];

                var t = MathF.Min(1.0f, MathF.Max(0.0f, indexGiven - i));
                wavetable[i] = float.Lerp(first, second, t);
                
                indexGiven += incrementGiven;
            }
            
            return;
        }

        for (var i = 0; i < wavetable.Length; i++)
        {
            wavetable[i] = actual.IsByte
                ? (actual.Table[i] & 0xFF) / 127.5f - 1.0f
                : (actual.Table[i] & 0xF) / 7.5f - 1.0f;
        }
    }

    protected override float Generate(double frequency)
    {
        var result = wavetable[(int)Math.Floor((Phase + 1.0d) / 2.0d * 32.0d)];
        Step(frequency);
        
        return result;
    }
}