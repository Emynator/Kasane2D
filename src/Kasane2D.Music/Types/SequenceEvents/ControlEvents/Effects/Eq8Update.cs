using Kasane2D.Exceptions.Engine;
using Kasane2D.Sound.Types;

namespace Kasane2D.Music.Types.SequenceEvents.ControlEvents.Effects;

/// <summary>
/// An update for eq8 parameters.
/// </summary>
public sealed record class Eq8Update : EffectUpdate
{
    /// <summary>
    /// Creates an update for compressor parameters.
    /// </summary>
    /// <param name="effectName">Name of the effect this update targets.</param>
    /// <param name="bypass">Optional: changes the bypass value of the effect.</param>
    /// <param name="bandParams">Optional: changes the band params of the eq.</param>
    /// <exception cref="DataConsistencyException">Thrown if bandParams is not null but does not have a size of 8.</exception>
    public Eq8Update
        (
        string effectName,
        bool? bypass = null,
        EqBandParams?[]? bandParams = null
        ) : base(effectName, bypass)
    {
        if (bandParams is not null && bandParams.Length != 8)
        {
            throw new DataConsistencyException("Band param count must be 8.");
        }

        BandParams = bandParams;
    }

    /// <summary>
    /// The changes to the band params of the eq.
    /// </summary>
    public EqBandParams?[]? BandParams { get; }
}