using Microsoft.Extensions.ObjectPool;

namespace Kotz.ObjectPool;

/// <summary>
/// Default fluent implementation for <see cref="PooledObjectPolicy{T}"/>.
/// </summary>
/// <typeparam name="T">The data type of the pooled objects.</typeparam>
public sealed class DefaultFluentPooledObjectPolicy<T> : PooledObjectPolicy<T> where T : notnull
{
    private readonly Func<T> _objectFactory;
    private readonly Func<T, bool>[] _objectFilters;

    /// <summary>
    /// Creates a default fluent <see cref="PooledObjectPolicy{T}"/>.
    /// </summary>
    /// <param name="objectFactory">The method responsible for instantiating a new <typeparamref name="T"/> object.</param>
    /// <param name="objectFilters">The filters to be applied to an object that is being returned to the pool.</param>
    /// <exception cref="ArgumentNullException">Occurs when one of the parameters is <see langword="null"/>.</exception>
    internal DefaultFluentPooledObjectPolicy(Func<T> objectFactory, IReadOnlyCollection<Func<T, bool>> objectFilters)
    {
        ArgumentNullException.ThrowIfNull(objectFactory, nameof(objectFactory));
        ArgumentNullException.ThrowIfNull(objectFilters, nameof(objectFilters));

        _objectFactory = objectFactory;
        _objectFilters = (objectFilters is Func<T, bool>[] filtersArray)
            ? filtersArray
            : objectFilters.ToArray();
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
    {
        foreach (var objectFilter in _objectFilters)
        {
            if (!objectFilter(obj))
                return false;
        }

        return true;
    }
}