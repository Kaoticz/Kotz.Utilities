namespace Kotz.Events;

/// <summary>
/// Generic version of <see cref="AsyncEventHandler"/>.
/// </summary>
/// <param name="sender">The object that published the event.</param>
/// <param name="eventArgs">The event arguments returned by the event.</param>
public delegate Task AsyncEventHandler(object? sender, EventArgs eventArgs);