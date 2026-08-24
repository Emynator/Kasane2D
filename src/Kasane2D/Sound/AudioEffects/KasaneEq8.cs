using Kasane2D.Sound.AudioEffects.Dsp;
using Kasane2D.Sound.Enums;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// An equalizer with 8 configurable parameters.
/// </summary>
public class KasaneEq8 : IAudioEffect
{
    private readonly DspFilter[] filtersL = new DspFilter[8];
    private readonly DspFilter[] filtersR = new DspFilter[8];

    private readonly EqFilterType[] types =
    [
        EqFilterType.LowShelf,
        EqFilterType.Peak,
        EqFilterType.Peak,
        EqFilterType.Peak,
        EqFilterType.Peak,
        EqFilterType.Peak,
        EqFilterType.Peak,
        EqFilterType.HighShelf,
    ];
    
    internal KasaneEq8(int sampleRate, string? name)
    {
        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasaneEq8_{actual}";
        
        for (var i = 0; i < filtersL.Length; i++)
        {
            var frequency = 50.0f * MathF.Pow(2.0f, i);
            
            filtersL[i].Frequency = frequency;
            filtersL[i] = new(sampleRate)
            {
                Type = DspFilterType.None,
            };

            filtersR[i].Frequency = frequency;
            filtersR[i] = new(sampleRate)
            {
                Type = DspFilterType.None,
            };
        }
    }
    
    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public bool Bypass { get; set; }

    /// <summary>
    /// Gets the parameter's filter type.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <returns>The value.</returns>
    public EqFilterType GetType(int index)
    {
        return types[index];
    }

    /// <summary>
    /// Sets the parameter's filter type.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <param name="type">The value to set.</param>
    public void SetType(int index, EqFilterType type)
    {
        types[index] = type;
        SetType(index);
    }

    /// <summary>
    /// Gets if the parameter is active.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <returns>The value.</returns>
    public bool GetIsActive(int index)
    {
        return filtersL[index].Type == DspFilterType.None;
    }

    /// <summary>
    /// Sets if the parameter is active
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <param name="isActive">The value to set.</param>
    public void SetIsActive(int index, bool isActive)
    {
        if (isActive)
        {
            SetType(index, true);
            return;
        }
        
        filtersL[index].Type = DspFilterType.None;
        filtersR[index].Type = DspFilterType.None;
    }

    /// <summary>
    /// Gets the parameter's frequency.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <returns>The value.</returns>
    public float GetFrequency(int index)
    {
        return filtersL[index].Frequency;
    }

    /// <summary>
    /// Sets the parameter's frequency.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <param name="frequency">The value to set.</param>
    public void SetFrequency(int index, float frequency)
    {
        filtersL[index].Frequency = frequency;
        filtersR[index].Frequency = frequency;
    }

    /// <summary>
    /// Gets the parameter's Q.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <returns>The value.</returns>
    public float GetQ(int index)
    {
        return filtersL[index].Q;
    }

    /// <summary>
    /// Sets the parameter's Q.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <param name="q">The value to set.</param>
    public void SetQ(int index, float q)
    {
        filtersL[index].Q = q;
        filtersR[index].Q = q;
    }

    /// <summary>
    /// Gets the parameter's gain.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <returns>The value.</returns>
    public float GetGain(int index)
    {
        return 20.0f * MathF.Log10(filtersL[index].Gain);
    }

    /// <summary>
    /// Sets the parameter's gain.
    /// </summary>
    /// <param name="index">The parameter index from 0 to 7.</param>
    /// <param name="gain">The value to set.</param>
    public void SetGain(int index, float gain)
    {
        var actualGain = MathF.Pow(10.0f, gain / 20.0f);
        filtersL[index].Gain = actualGain;
        filtersR[index].Gain = actualGain;
    }
    
    /// <inheritdoc/>
    public void Apply(ReadOnlySpan<float> inLeft, ReadOnlySpan<float> inRight, Span<float> outLeft, Span<float> outRight)
    {
        for (var i = 0; i < filtersL.Length; i++)
        {
            var sampleL = inLeft[i];
            var sampleR = inRight[i];
            for (var j = 0; j < filtersL.Length; j++)
            {
                sampleL = filtersL[j].Apply(sampleL);
                sampleR = filtersR[j].Apply(sampleR);
            }
            
            outLeft[i] = sampleL;
            outRight[i] = sampleR;
        }
    }
    
    private void SetType(int index, bool setActive = false)
    {
        if (!setActive && filtersL[index].Type == DspFilterType.None)
        {
            return;
        }
        
        switch (types[index])
        {
            case EqFilterType.LowPass4X:
                filtersL[index].Type = DspFilterType.LowPass;
                filtersL[index].Slope = 4;
                
                filtersR[index].Type = DspFilterType.LowPass;
                filtersR[index].Slope = 4;
                return;
            
            case EqFilterType.LowPass:
                filtersL[index].Type = DspFilterType.LowPass;
                filtersL[index].Slope = 2;
                
                filtersR[index].Type = DspFilterType.LowPass;
                filtersR[index].Slope = 2;
                return;
            
            case EqFilterType.LowShelf:
                filtersL[index].Type = DspFilterType.LowShelf;
                filtersR[index].Type = DspFilterType.LowShelf;
                return;
            
            case EqFilterType.Peak:
                filtersL[index].Type = DspFilterType.Peak;
                filtersR[index].Type = DspFilterType.Peak;
                return;
            
            case EqFilterType.HighShelf:
                filtersL[index].Type = DspFilterType.HighShelf;
                filtersR[index].Type = DspFilterType.HighShelf;
                return;
            
            case EqFilterType.HighPass:
                filtersL[index].Type = DspFilterType.HighPass;
                filtersL[index].Slope = 2;
                
                filtersR[index].Type = DspFilterType.HighPass;
                filtersR[index].Slope = 2;
                return;
            
            case EqFilterType.HighPass4X:
                filtersL[index].Type = DspFilterType.HighPass;
                filtersL[index].Slope = 4;
                
                filtersR[index].Type = DspFilterType.HighPass;
                filtersR[index].Slope = 4;
                return;
        }
    }
}