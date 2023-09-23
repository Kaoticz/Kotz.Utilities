using Microsoft.Extensions.DependencyInjection;

namespace Kotz.DependencyInjection.Extensions;

/// <summary>
/// Defines extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class IServiceProviderExt
{
    /// <summary>
    /// Gets a service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The concrete type of the service to be fetched.</typeparam>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve the service from.</param>
    /// <param name="arguments">The arguments needed for the service to be initialized.</param>
    /// <remarks>
    /// Do not use abstract types in the type argument! <br />
    /// This method always initializes a new instance, regardless of its registered lifetime.
    /// </remarks>
    /// <returns>A service of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="serviceProvider"/> or <paramref name="arguments"/> are <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Occurs when there is no concrete service of type <typeparamref name="T"/> or when the arguments are wrong.</exception>
    public static T GetParameterizedService<T>(this IServiceProvider serviceProvider, params object[] arguments)
        => (T)GetParameterizedService(serviceProvider, typeof(T), arguments);

    /// <summary>
    /// Gets a service of the specified type from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve the service from.</param>
    /// <param name="serviceConcreteType">The concrete type of the service to be fetched.</param>
    /// <param name="arguments">The arguments needed for the service to be initialized.</param>
    /// <remarks>This method always initializes a new instance, regardless of its registered lifetime.</remarks>
    /// <returns>A service of type <paramref name="serviceConcreteType"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="serviceProvider"/> or <paramref name="arguments"/> are <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Occurs when there is no concrete service of type <paramref name="serviceConcreteType"/> or when the arguments are wrong.</exception>
    public static object GetParameterizedService(this IServiceProvider serviceProvider, Type serviceConcreteType, params object[] arguments)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));

        var result = ActivatorUtilities.CreateInstance(serviceProvider, serviceConcreteType, arguments);

        return (result is null)
            ? throw new InvalidOperationException($"There is no service of type {serviceConcreteType.Name} in the IoC or the arguments were incorrect.")
            : result;
    }

    /// <summary>
    /// Gets a scoped or transient service of type <typeparamref name="T"/> from this <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <param name="ioc">This service provider.</param>
    /// <param name="service">The requested scoped service. It will be <see langword="null"/> if <typeparamref name="T"/> is not registered.</param>
    /// <returns>An <see cref="IServiceScope"/> to be disposed after use.</returns>
    /// <exception cref="InvalidOperationException">Occurs when the service of type <typeparamref name="T"/> is not found.</exception>
    public static IServiceScope GetScopedService<T>(this IServiceProvider ioc, out T service) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(ioc, nameof(ioc));

        var scope = ioc.CreateScope();
        service = scope.ServiceProvider.GetRequiredService<T>();

        return scope;
    }
}