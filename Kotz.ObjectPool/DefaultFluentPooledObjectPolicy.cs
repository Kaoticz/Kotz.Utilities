using Microsoft.Extensions.ObjectPool;

namespace Kotz.ObjectPool;

/// <summary>
/// Default fluent implementation for <see cref="PooledObjectPolicy{T}"/>.
/// </summary>
/// <typeparam name="T">The data type of the pooled objects.</typeparam>
public sealed class DefaultFluentPooledObjectPolicy<T> : PooledObjectPolicy<T> where T : notnull
{
    private readonly Func<T> _objectFactory;
    private readonly Func<T, bool> _objectFilter;

    /// <summary>
    /// Creates a default fluent <see cref="PooledObjectPolicy{T}"/>.
    /// </summary>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectFilter">The filter to be applied to an object that is being returned to the pool.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="objectFactory"/> is <see langword="null"/>.</exception>
    internal DefaultFluentPooledObjectPolicy(Func<T> objectFactory, Func<T, bool>? objectFilter = default)
    {
        ArgumentNullException.ThrowIfNull(objectFactory, nameof(objectFactory));

        _objectFactory = objectFactory;
        _objectFilter = objectFilter ?? (static _ => true);
    }

    /// <summary>
    /// Creates a new <typeparamref name="T"/> object from the internal factory method.
    /// </summary>
    /// <returns>A new <typeparamref name="T"/> object.</returns>
    public override T Create()
        => _objectFactory();

    /// <summary>
    /// Determines if <paramref name="obj"/> can be returned to the pool.
    /// </summary>
    /// <param name="obj">The object to be returned.</param>
    /// <returns><see langword="true"/> if the object can be returned to the pool, <see langword="false"/> otherwise.</returns>
    public override bool Return(T obj)
        => _objectFilter(obj);
}