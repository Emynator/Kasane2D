using Kasane2D.Music.Types.SequenceEvents.ControlEvents.Generators;

namespace Kasane2D.Music.Synthesis.Generators;

/// <summary>
/// Abstract base class for sound generators used by the synth engine.
/// </summary>
public abstract class Generator
{
    /// <summary>
    /// The sound system's sample rate in Hz.
    /// </summary>
    protected readonly int sampleRate;
    
    /// <summary>
    /// Base ctor.
    /// </summary>
    /// <param name="sampleRate">The sound system's sample rate in Hz.</param>
    protected Generator(int sampleRate)
    {
        this.sampleRate = sampleRate;
    }
    
    /// <summary>
    /// Current phase of the generator.
    /// </summary>
    protected double Phase { get; private set; } = 0.0d;

    /// <summary>
    /// Generate function that is called by the synth engine.
    /// </summary>
    /// <param name="output">Output sample buffer.</param>
    /// <param name="frequency">Frequency to generate.</param>
    public void Generate(Span<float> output, double frequency)
    {
        for (var i = 0; i < output.Length; i++)
        {
            output[i] = Generate(frequency);
        }
    }

    /// <summary>
    /// Applies a control update event to update the generator parameters.
    /// </summary>
    /// <param name="ev">The update event.</param>
    public abstract void ControlUpdate(GeneratorUpdate ev);

    /// <summary>
    /// Resets the generator's phase.
    /// </summary>
    public virtual void Reset()
    {
        Phase = 0.0d;
    }

    /// <summary>
    /// Generates a single audio sample for the given frequency.
    /// </summary>
    /// <param name="frequency">The frequency.</param>
    /// <returns>The genrated sample.</returns>
    protected abstract float Generate(double frequency);

    /// <summary>
    /// Optional callback function so implementing classes can do something whenever the phase completes a full iteration.
    /// </summary>
    protected virtual void PhaseCallback()
    {
    }

    /// <summary>
    /// Advances the phase according to the frequency.
    /// </summary>
    /// <param name="frequency">The frequency.</param>
    protected void Step(double frequency)
    {
        Phase += frequency / sampleRate;
        if (Phase >= 2.0d)
        {
            var cycles = Math.Floor(Phase);
            Phase -= cycles;
            
            for (var i = 0; i < (int)cycles; i++)
            {
                PhaseCallback();
            }
        }
        if (Phase >= 1.0d)
        {
            Phase -= 1.0d;
            PhaseCallback();
        }
    }
}