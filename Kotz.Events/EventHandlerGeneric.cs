namespace Kotz.Events;

/// <summary>
/// Generic version of <see cref="EventHandler"/>.
/// </summary>
/// <typeparam name="T1">The type of the object that published the event.</typeparam>
/// <typeparam name="T2">The type of the event arguments returned by the event.</typeparam>
/// <param name="sender">The object that published the event.</param>
/// <param name="eventArgs">The event arguments returned by the event.</param>
public delegate void EventHandler<T1, T2>(T1 sender, T2 eventArgs) where T2 : EventArgs;