using Microsoft.Extensions.DependencyInjection;

namespace Kotz.DependencyInjection.Extensions;

/// <summary>
/// Defines extension methods for <see cref="IServiceScopeFactory"/>.
/// </summary>
public static class IServiceScopeFactoryExt
{
    /// <summary>
    /// Gets a service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The concrete type of the service to be fetched.</typeparam>
    /// <param name="scopeFactory">The IoC's scope factory.</param>
    /// <param name="arguments">The arguments needed for the service to be initialized.</param>
    /// <remarks>
    /// Do not use abstract types in the type argument! <br />
    /// This method always initializes a new instance, regardless of its registered lifetime.
    /// </remarks>
    /// <returns>A service of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="scopeFactory"/> or <paramref name="arguments"/> are <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Occurs when there is no concrete service of type <typeparamref name="T"/> or when the arguments are wrong.</exception>
    public static T GetParameterizedService<T>(this IServiceScopeFactory scopeFactory, params object[] arguments)
        => (T)GetParameterizedService(scopeFactory, typeof(T), arguments);

    /// <summary>
    /// Gets a service of the specified type from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="scopeFactory">The IoC's scope factory.</param>
    /// <param name="serviceConcreteType">The concrete type of the service to be fetched.</param>
    /// <param name="arguments">The arguments needed for the service to be initialized.</param>
    /// <remarks>This method always initializes a new instance, regardless of its registered lifetime.</remarks>
    /// <returns>A service of type <paramref name="serviceConcreteType"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="scopeFactory"/> or <paramref name="arguments"/> are <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Occurs when there is no concrete service of type <paramref name="serviceConcreteType"/> or when the arguments are wrong.</exception>
    public static object GetParameterizedService(this IServiceScopeFactory scopeFactory, Type serviceConcreteType, params object[] arguments)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory, nameof(scopeFactory));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));

        using var scope = scopeFactory.CreateScope();
        var result = ActivatorUtilities.CreateInstance(scope.ServiceProvider, serviceConcreteType, arguments);

        return (result is null)
            ? throw new InvalidOperationException($"There is no service of type {serviceConcreteType.Name} in the IoC or the arguments were incorrect.")
            : result;
    }

    /// <summary>
    /// Gets a scoped or transient service of type <typeparamref name="T"/> from this <see cref="IServiceScopeFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <param name="scopeFactory">The IoC's scope factory.</param>
    /// <param name="service">The requested scoped service. It will be <see langword="null"/> if <typeparamref name="T"/> is not registered.</param>
    /// <returns>An <see cref="IServiceScope"/> to be disposed after use.</returns>
    /// <exception cref="InvalidOperationException">Occurs when the service of type <typeparamref name="T"/> is not found.</exception>
    public static IServiceScope GetScopedService<T>(this IServiceScopeFactory scopeFactory, out T service) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(scopeFactory, nameof(scopeFactory));

        var scope = scopeFactory.CreateScope();
        service = scope.ServiceProvider.GetRequiredService<T>();

        return scope;
    }
}