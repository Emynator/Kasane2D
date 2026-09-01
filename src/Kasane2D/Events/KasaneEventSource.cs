using System.Collections.Concurrent;

namespace Kasane2D.Events;

/// <summary>
/// Represents the source for an asynchronous event.
/// </summary>
public class KasaneEventSource : IDisposable
{
    private readonly ConcurrentDictionary<Guid, Action> subscribers = new();

    /// <summary>
    /// Creates a new EventSource.
    /// </summary>
    public KasaneEventSource()
    {
        Event = new(this);
    }
    
    /// <summary>
    /// The event belonging to this source.
    /// </summary>
    public KasaneEvent Event { get; }

    /// <summary>
    /// Clears all subscribers from this event source.
    /// </summary>
    public void Dispose()
    {
        subscribers.Clear();
    }

    /// <summary>
    /// Triggers this event and call all subscriber actions in parallels.
    /// </summary>
    /// <returns>A task that completes when all subcriber callbacks have run.</returns>
    public Task Trigger()
    {
        return Task.WhenAll(subscribers.Values.Select(Task.Run));
    }

    internal void AddSubscriber(Guid id, Action action)
    {
        subscribers.TryAdd(id, action);
    }

    internal void RemoveSubscriber(Guid id)
    {
        subscribers.TryRemove(id, out _);
    }
}

/// <summary>
/// Represents the source for an asynchronous event with a value attached.
/// </summary>
/// <typeparam name="T">The type of value that this event will send.</typeparam>
public class KasaneEventSource<T> : IDisposable
{
    private readonly ConcurrentDictionary<Guid, Action<T>> subscribers = new();
    
    /// <summary>
    /// Creates a new EventSource.
    /// </summary>
    public KasaneEventSource()
    {
        Event = new(this);
    }
    
    /// <summary>
    /// The event belonging to this source.
    /// </summary>
    public KasaneEvent<T> Event { get; }

    /// <summary>
    /// Clears all subscribers from this event source.
    /// </summary>
    public void Dispose()
    {
        subscribers.Clear();
    }

    /// <summary>
    /// Triggers this event and call all subscriber actions in parallels.
    /// </summary>
    /// <param name="value">The value to send to all subscribers.</param>
    /// <returns>A task that completes when all subcriber callbacks have run.</returns>
    public Task Trigger(T value)
    {
        return Task.WhenAll(subscribers.Values.Select(action => Task.Run(() => action(value))));
    }

    internal void AddSubscriber(Guid id, Action<T> action)
    {
        subscribers.TryAdd(id, action);
    }

    internal void RemoveSubscriber(Guid id)
    {
        subscribers.TryRemove(id, out _);
    }
}