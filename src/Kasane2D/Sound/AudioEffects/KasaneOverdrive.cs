using Kasane2D.Sound.Enums;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Sound.AudioEffects;

/// <summary>
/// A distortion effect.
/// </summary>
public class KasaneOverdrive : IAudioEffect
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private float drive = 1.0f;

    internal KasaneOverdrive(float drive, DistortionType type, float wet, string? name)
    {
        var actual = name ?? Guid.NewGuid().ToString();
        Name = $"KasaneCompressor_{actual}";
        Type = type;
        Wet = wet;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public bool Bypass { get; set; }

    /// <summary>
    /// Drive in db applied to the input before processing.
    /// </summary>
    public float Drive
    {
        get => 20.0f * MathF.Log10(drive);
        set
        {
            tlock.Wait();
            var actual = MathF.Max(-60.0f, MathF.Min(20.0f, value));
            drive = MathF.Pow(10.0f, actual / 20.0f);
            tlock.Release();
        }
    }

    /// <summary>
    /// The distortion type that is applied.
    /// </summary>
    public DistortionType Type
    {
        get;
        set
        {
            tlock.Wait();
            field = value;
            tlock.Release();
        }
    }

    /// <summary>
    /// Dry/Wet from 0.0f for full dry to 1.0f for full wet.
    /// </summary>
    public float Wet
    {
        get;
        set
        {
            tlock.Wait();
            field = MathF.Max(0.0f, MathF.Min(1.0f, value));
            tlock.Release();
        }
    }

    /// <inheritdoc/>
    public void Apply
        (
        ReadOnlySpan<float> inLeft,
        ReadOnlySpan<float> inRight,
        Span<float> outLeft,
        Span<float> outRight
        )
    {
        tlock.Wait();
        
        if (Bypass)
        {
            inLeft.CopyTo(outLeft);
            inRight.CopyTo(outRight);
            tlock.Release();

            return;
        }
        
        var dry = 1.0f - Wet;
        switch (Type)
        {
            case DistortionType.DigitalClip:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var left = MathF.Max(-1.0f, MathF.Min(1.0f, inLeft[i] * drive));
                    var right = MathF.Max(-1.0f, MathF.Min(1.0f, inRight[i] * drive));
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.SoftSaturation:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var left = MathF.Tanh(inLeft[i] * drive);
                    var right = MathF.Tanh(inRight[i] * drive);
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.WarmSaturation:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var left = MathF.Atanh(inLeft[i] * drive);
                    var right = MathF.Atanh(inRight[i] * drive);
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.Overdrive:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var x = inLeft[i] * drive;
                    var left = 2.0f * x / (1.0f + MathF.Abs(x));
                    x = inRight[i] * drive;
                    var right = 2.0f * x / (1.0f + MathF.Abs(x));
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.SineShaper:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var left = MathF.Sin(inLeft[i] * drive);
                    var right = MathF.Sin(inRight[i] * drive);
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.SineFold:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var left = MathF.Sin(MathF.PI * inLeft[i] * drive / 2.0f);
                    var right = MathF.Sin(MathF.PI * inRight[i] * drive / 2.0f);
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            case DistortionType.CubicFold:
                for (var i = 0; i < inLeft.Length; i++)
                {
                    var x = inLeft[i] * drive;
                    var left = MathF.Max(-1.0f, MathF.Min(1.0f, 1.5f * x - 0.5f * x * x * x));
                    x = inRight[i] * drive;
                    var right = MathF.Max(-1.0f, MathF.Min(1.0f, 1.5f * x - 0.5f * x * x * x));
                    
                    outLeft[i] = inLeft[i] * dry + left * Wet;
                    outRight[i] = inRight[i] * dry + right * Wet;
                }
                break;
            
            default:
                inLeft.CopyTo(outLeft);
                inRight.CopyTo(outRight);
                break;
        }
        
        tlock.Release();
    }
}