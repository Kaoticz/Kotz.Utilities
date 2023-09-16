namespace Kotz.Events;

/// <summary>
/// Generic version of <see cref="AsyncEventHandler"/>.
/// </summary>
/// <typeparam name="T1">The type of the object that published the event.</typeparam>
/// <typeparam name="T2">The type of the event arguments returned by the event.</typeparam>
/// <param name="sender">The object that published the event.</param>
/// <param name="eventArgs">The event arguments returned by the event.</param>
/// <remarks>
/// Awaiting <see cref="AsyncEventHandler{T1, T2}.Invoke(T1, T2)"/> will return when the last registered
/// delegate completes execution. <br /> If you want to await the completion of all registered delegates,
/// please consider using <see cref="AsyncEvent{TSender, TEventArgs}"/> instead.
/// </remarks>
public delegate Task AsyncEventHandler<in T1, in T2>(T1 sender, T2 eventArgs) where T2 : EventArgs;