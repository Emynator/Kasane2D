namespace Kasane2D.Enums;

/// <summary>
/// Scaling modes from one aspect ratio to another.
/// </summary>
public enum AspectRatioScalingMode
{
    /// <summary>
    /// Keep the original aspect ratio and insert bars if necessary.
    /// </summary>
    Keep,
    /// <summary>
    /// Stretch the original aspect ratio to the target ratio.
    /// </summary>
    Stretch,
}