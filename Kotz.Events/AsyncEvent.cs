namespace Kotz.Events;

/// <summary>
/// Represents an asynchronous event that properly awaits the execution of all its handlers.
/// </summary>
/// <typeparam name="T1">The sender object.</typeparam>
/// <typeparam name="T2">The event arguments.</typeparam>
public sealed class AsyncEvent<T1, T2> where T2 : EventArgs
{
    private readonly List<AsyncEventHandler<T1, T2>> _handlers = new();

    /// <summary>
    /// The event handler used for registration of new handlers.
    /// </summary>
    public event AsyncEventHandler<T1, T2> Handler
    {
        add => _handlers.Add(value);
        remove => _handlers.Remove(value);
    }

    /// <summary>
    /// Gets the invocation list of this event, in invocation order.
    /// </summary>
    public IReadOnlyList<AsyncEventHandler<T1, T2>> InvocationList
        => _handlers;

    /// <summary>
    /// Invokes the handlers registered in this event and waits for
    /// all of them to complete execution.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="args">The event arguments.</param>
    public Task InvokeAsync(T1 sender, T2 args)
    {
        var tasks = _handlers.Select(x => x.Invoke(sender, args));
        return Task.WhenAll(tasks);
    }
}