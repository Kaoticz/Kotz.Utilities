using Microsoft.Extensions.ObjectPool;

namespace Kotz.ObjectPool;

/// <summary>
/// Default fluent implementation of <see cref="ObjectPool{T}"/>.
/// </summary>
/// <typeparam name="T">The data type of the pooled objects.</typeparam>
public sealed class FluentObjectPool<T> : ObjectPool<T> where T : class
{
    private readonly DefaultObjectPool<T> _internalPool;
    private readonly Func<T, T> _objectResetter;

    /// <summary>
    /// Creates a fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="objectPolicy">The pooling policy to use.</param>
    /// <param name="maximumRetained">The maximum number of objects to retain in the pool.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="objectPolicy"/> is <see langword="null"/>.</exception>
    public FluentObjectPool(IPooledObjectPolicy<T> objectPolicy, int maximumRetained = default)
    {
        ArgumentNullException.ThrowIfNull(objectPolicy, nameof(objectPolicy));

        _internalPool = (maximumRetained > 0) ? new(objectPolicy, maximumRetained) : new(objectPolicy);
        _objectResetter = static x => x;
    }

    /// <summary>
    /// Creates a fluent instance of <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectResetter">
    /// The method responsible for resetting the internal state of <typeparamref name="T"/> objects that are being returned to the pool.
    /// </param>
    /// <param name="objectFilter">
    /// The filter to be applied to an object that is being returned to the pool. <br />
    /// If the filter returns <see langword="true"/>, the object is returned to the pool, otherwise it is not and the object may become
    /// eligible for garbage collection.
    /// </param>
    /// <param name="maximumRetained">The maximum number of objects to retain in the pool.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="objectFactory"/> is <see langword="null"/>.</exception>
    public FluentObjectPool(Func<T> objectFactory, Func<T, T>? objectResetter = default, Func<T, bool>? objectFilter = default, int maximumRetained = default)
    {
        ArgumentNullException.ThrowIfNull(objectFactory, nameof(objectFactory));

        var objectPolicy = new DefaultFluentPooledObjectPolicy<T>(objectFactory, objectFilter);
        _internalPool = (maximumRetained > 0) ? new(objectPolicy, maximumRetained) : new(objectPolicy);
        _objectResetter = objectResetter ?? (static x => x);
    }

    /// <summary>
    /// Gets an object from the pool if one is available, otherwise creates one.
    /// </summary>
    /// <returns>A <typeparamref name="T"/>.</returns>
    public override T Get()
        => _internalPool.Get();

    /// <summary>
    /// Returns the specified object to the pool.
    /// </summary>
    /// <param name="obj">The object to be added to the pool.</param>
    public override void Return(T obj)
        => _internalPool.Return(_objectResetter(obj));
}