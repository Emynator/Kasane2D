namespace Kasane2D.Events;

/// <summary>
/// Represents the subscriber facing API for <see cref="KasaneEventSource"/>
/// </summary>
public class KasaneEvent
{
    private readonly KasaneEventSource source;

    internal KasaneEvent(KasaneEventSource source)
    {
        this.source = source;
    }

    /// <summary>
    /// Adds a callback action that will be executed whenever the event is triggered.
    /// </summary>
    /// <param name="callback">The callback action.</param>
    /// <returns>An action to unsubscribe from this event.</returns>
    public Action Subscribe(Action callback)
    {
        var id = Guid.NewGuid();
        source.AddSubscriber(id, callback);
        
        return () => source.RemoveSubscriber(id);
    }
}

/// <summary>
/// Represents the subscriber facing API for <see cref="KasaneEventSource{T}"/>
/// </summary>
/// <typeparam name="T">The type of value that this event will send.</typeparam>
public class KasaneEvent<T>
{
    private readonly KasaneEventSource<T> source;

    internal KasaneEvent(KasaneEventSource<T> source)
    {
        this.source = source;
    }

    /// <summary>
    /// Adds a callback action that will be executed whenever the event is triggered.
    /// </summary>
    /// <param name="callback">The callback action.</param>
    /// <returns>An action to unsubscribe from this event.</returns>
    public Action Subscribe(Action<T> callback)
    {
        var id = Guid.NewGuid();
        source.AddSubscriber(id, callback);
        
        return () => source.RemoveSubscriber(id);
    }
}