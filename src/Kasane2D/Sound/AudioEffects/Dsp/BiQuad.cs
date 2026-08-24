namespace Kasane2D.Sound.AudioEffects.Dsp;

internal struct BiQuad
{
    private readonly int sampleRate;
    private float a0;
    private float a1;
    private float a2;
    private float b1;
    private float b2;
    private float delay0;
    private float delay1;

    public BiQuad(int sampleRate)
    {
        this.sampleRate = sampleRate;
    }

    public void ConfigureAsLpf(float frequency, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha);
        
        a0 = (1.0f - cos) / 2.0f * normalization;
        a1 = (1.0f - cos) * normalization;
        a2 = (1.0f - cos) / 2.0f * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha) * normalization;
    }
    
    public void ConfigureAsOnePoleLpf(float frequency)
    {
        var k = MathF.Tan(MathF.PI * frequency / sampleRate);
        var normalization = 1.0f / (1.0f + k);
        
        a0 = k * normalization;
        a1 = k * normalization;
        b1 = (k - 1.0f) * normalization;
    }

    public void ConfigureAsHpf(float frequency, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha);
        
        a0 = (1.0f + cos) / 2.0f * normalization;
        a1 = -(1.0f + cos) * normalization;
        a2 = (1.0f + cos) / 2.0f * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha) * normalization;
    }
    
    public void ConfigureAsOnePoleHpf(float frequency)
    {
        var k = MathF.Tan(MathF.PI * frequency / sampleRate);
        var normalization = 1.0f / (1.0f + k);
        
        a0 = normalization;
        a1 = -normalization;
        b1 = (k - 1.0f) * normalization;
    }

    public void ConfigureAsBpf(float frequency, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha);

        a0 = q * alpha * normalization;
        a1 = 0.0f;
        a2 = -q * alpha * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha) * normalization;
    }
    
    public void ConfigureAsZeroGainBpf(float frequency, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha);

        a0 = alpha * normalization;
        a1 = 0.0f;
        a2 = -alpha * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha) * normalization;
    }

    public void ConfigureAsNotch(float frequency, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha);

        a0 = 1.0f * normalization;
        a1 = -2.0f * cos * normalization;
        a2 = 1.0f * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha) * normalization;
    }

    public void ConfigureAsPeak(float frequency, float gain, float q)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / (2.0f * q);
        var normalization = 1.0f / (1.0f + alpha / gain);
        
        a0 = (1.0f + alpha * gain) * normalization;
        a1 = -2.0f * cos * normalization;
        a2 = (1.0f - alpha * gain) * normalization;
        b1 = -2.0f * cos * normalization;
        b2 = (1.0f - alpha / gain)  * normalization;
    }

    public void ConfigureAsLowShelf(float frequency, float gain, float s)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / 2.0f * MathF.Sqrt((gain + 1.0f / gain) * (1.0f / s - 1.0f) + 2.0f);
        var beta = 2.0f * MathF.Sqrt(gain) * alpha;
        var normalization = 1.0f / (gain + 1.0f + (gain - 1.0f) * cos + beta);
        
        a0 = gain * (gain + 1.0f - (gain - 1.0f) * cos + beta) * normalization;
        a1 = 2.0f * gain * (gain - 1.0f - (gain + 1.0f) * cos) * normalization;
        a2 = gain * (gain + 1.0f - (gain - 1.0f) * cos - beta) * normalization;
        b1 = -2.0f * (gain - 1.0f + (gain + 1.0f) * cos) * normalization;
        b2 = (gain + 1.0f + (gain - 1.0f) * cos - beta) * normalization;
    }

    public void ConfigureAsHighShelf(float frequency, float gain, float s)
    {
        var omega = MathF.Tau * (frequency / sampleRate);
        var sin = MathF.Sin(omega);
        var cos = MathF.Cos(omega);
        
        var alpha = sin / 2.0f * MathF.Sqrt((gain + 1.0f / gain) * (1.0f / s - 1.0f) + 2.0f);
        var beta = 2.0f * MathF.Sqrt(gain) * alpha;
        var normalization = 1.0f / (gain + 1.0f - (gain - 1.0f) * cos + beta);
        
        a0 = gain * (gain + 1.0f + (gain - 1.0f) * cos + beta) * normalization;
        a1 = -2.0f * gain * (gain - 1.0f + (gain + 1.0f) * cos) * normalization;
        a2 = gain * (gain + 1.0f + (gain - 1.0f) * cos - beta) * normalization;
        b1 = 2.0f * (gain - 1.0f - (gain + 1.0f) * cos) * normalization;
        b2 = (gain + 1.0f - (gain - 1.0f) * cos - beta) * normalization;
    }

    public float Next(float input)
    {
        var state = input - b1 * delay0 - b2 * delay1;
        var result = state * a0 + delay0 * a1 + delay1 * a2;
        
        delay1 = delay0;
        delay0 = state;

        return result;
    }
}