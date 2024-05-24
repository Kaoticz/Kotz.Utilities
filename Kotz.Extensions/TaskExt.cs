namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Task"/> and <see cref="Task{T}"/>.
/// </summary>
public static class TaskExt
{
    /// <summary>
    /// Awaits all tasks in the current collection and returns when all of them have completed.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task WhenAllAsync(this IEnumerable<Task> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        await Task.WhenAll(collection).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns their results in an array.
    /// </summary>
    /// <typeparam name="T">The data that needs to be awaited.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task<T[]> WhenAllAsync<T>(this IEnumerable<Task<T>> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return await Task.WhenAll(collection).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns when any of them have completed.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task WhenAnyAsync(this IEnumerable<Task> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        await (await Task.WhenAny(collection).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits the first task that completes in the current collection and returns its result.
    /// </summary>
    /// <typeparam name="T">The data that needs to be awaited.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <returns>The <typeparamref name="T"/> object of the task that first finished executing.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task<T> WhenAnyAsync<T>(this IEnumerable<Task<T>> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return await (await Task.WhenAny(collection).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes this task asynchronously and safely returns the awaited task.
    /// </summary>
    /// <param name="task">This task.</param>
    /// <returns>This awaited task.</returns>
    public static async Task<Task> AwaitAsync(this Task task)
    {
        try
        {
            await task.ConfigureAwait(false);
            return task;
        }
        catch
        {
            return task;
        }
    }

    /// <summary>
    /// Executes this task asynchronously and safely returns the awaited task.
    /// </summary>
    /// <typeparam name="T">Data type held by <paramref name="task"/>.</typeparam>
    /// <param name="task">This task.</param>
    /// <returns>This awaited task.</returns>
    public static async Task<Task<T>> AwaitAsync<T>(this Task<T> task)
    {
        try
        {
            await task.ConfigureAwait(false);
            return task;
        }
        catch
        {
            return task;
        }
    }
}