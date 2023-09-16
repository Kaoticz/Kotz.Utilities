namespace Kotz.Events;

/// <summary>
/// Represents the method that will handle an asynchronous event that has no event data.
/// </summary>
/// <param name="sender">The object that published the event.</param>
/// <param name="eventArgs">The event arguments returned by the event.</param>
/// <remarks>
/// Awaiting <see cref="AsyncEventHandler.Invoke(object?, EventArgs)"/> will return when the last registered
/// delegate completes execution. <br /> If you want to await the completion of all registered delegates,
/// please consider using <see cref="AsyncEvent{T1, T2}"/> instead.
/// </remarks>
public delegate Task AsyncEventHandler(object? sender, EventArgs eventArgs);