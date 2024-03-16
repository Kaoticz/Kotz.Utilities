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
        await Task.WhenAll(collection);
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
        return await Task.WhenAll(collection);
    }

    /// <summary>
    /// Awaits all tasks in the current collection and returns when any of them have completed.
    /// </summary>
    /// <param name="collection">This collection.</param>
    /// <exception cref="ArgumentNullException">Occurs when the collection is <see langword="null"/>.</exception>
    public static async Task WhenAnyAsync(this IEnumerable<Task> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        await await Task.WhenAny(collection);
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
        return await await Task.WhenAny(collection);
    }
}