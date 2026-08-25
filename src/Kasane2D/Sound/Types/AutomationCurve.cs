using Kasane2D.Types;

namespace Kasane2D.Sound.Types;

internal class AutomationCurve
{
    private readonly Bezier curve;
    private readonly Action<float> setterAction;
    private readonly Action finishedCallback;
    private readonly float increment;
    private float t = 0.0f;

    public AutomationCurve(Guid id, Bezier curve, Action<float> setterAction, Action finishedCallback, float increment)
    {
        this.curve = curve;
        this.setterAction = setterAction;
        this.finishedCallback = finishedCallback;
        this.increment = increment;
        Id = id;
    }
    
    public Guid Id { get; }

    public void Apply()
    {
        t += increment;
        if (t < 1.0f)
        {
            setterAction(curve.Interpolate(t).Y);
            return;
        }

        setterAction(curve.End.Y);
        finishedCallback();
    }
}