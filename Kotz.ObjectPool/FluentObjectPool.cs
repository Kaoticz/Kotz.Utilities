using Microsoft.Extensions.ObjectPool;

namespace Kotz.ObjectPool;

/// <summary>
/// Defines <see langword="static"/> methods for creating <see cref="FluentObjectPool{T}"/> instances.
/// </summary>
public static class FluentObjectPool
{
    /// <summary>
    /// Creates a default fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="objectPolicy">The pooling policy to use.</param>
    /// <param name="maximumRetained">The maximum number of objects to retain in the pool.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="objectPolicy"/> is <see langword="null"/>.</exception>
    public static FluentObjectPool<T> Create<T>(IPooledObjectPolicy<T> objectPolicy, int maximumRetained = default) where T : class
        => new(objectPolicy, maximumRetained);

    /// <summary>
    /// Creates a default fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectResetter">
    /// The method responsible for resetting the internal state of <typeparamref name="T"/> objects that are being returned to the pool.
    /// </param>
    /// <param name="objectFilters">
    /// The filters to be applied to an object that is being returned to the pool. <br />
    /// If all filters return <see langword="true"/>, the object is returned to the pool, otherwise it is not and the object may become
    /// eligible for garbage collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Occurs when <paramref name="objectFactory"/> or <paramref name="objectFilters"/> are <see langword="null"/>.
    /// </exception>
    public static FluentObjectPool<T> Create<T>(Func<T> objectFactory, Func<T, T>? objectResetter, params Func<T, bool>[] objectFilters) where T : class
        => new(objectFactory, objectResetter, objectFilters);

    /// <summary>
    /// Creates a default fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="maximumRetained">The maximum number of objects to retain in the pool.</param>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectResetter">
    /// The method responsible for resetting the internal state of <typeparamref name="T"/> objects that are being returned to the pool.
    /// </param>
    /// <param name="objectFilters">
    /// The filters to be applied to an object that is being returned to the pool. <br />
    /// If all filters return <see langword="true"/>, the object is returned to the pool, otherwise it is not and the object may become
    /// eligible for garbage collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Occurs when <paramref name="objectFactory"/> or <paramref name="objectFilters"/> are <see langword="null"/>.
    /// </exception>
    public static FluentObjectPool<T> Create<T>(int maximumRetained, Func<T> objectFactory, Func<T, T>? objectResetter, params Func<T, bool>[] objectFilters) where T : class
        => new(maximumRetained, objectFactory, objectResetter, objectFilters);

    /// <summary>
    /// Creates a default fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectResetter">
    /// The method responsible for resetting the internal state of <typeparamref name="T"/> objects that are being returned to the pool.
    /// </param>
    /// <param name="objectFilters">
    /// The filters to be applied to an object that is being returned to the pool. <br />
    /// If all filters return <see langword="true"/>, the object is returned to the pool, otherwise it is not and the object may become
    /// eligible for garbage collection.
    /// </param>
    /// <param name="maximumRetained">The maximum number of objects to retain in the pool.</param>
    /// <exception cref="ArgumentNullException">
    /// Occurs when <paramref name="objectFactory"/> or <paramref name="objectFilters"/> are <see langword="null"/>.
    /// </exception>
    public static FluentObjectPool<T> Create<T>(Func<T> objectFactory, Func<T, T>? objectResetter, IReadOnlyCollection<Func<T, bool>> objectFilters, int maximumRetained = default) where T : class
        => new(objectFactory, objectResetter, objectFilters, maximumRetained);
}