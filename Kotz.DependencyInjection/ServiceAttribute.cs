using Kotz.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.DependencyInjection;

/// <summary>
/// This attribute marks the class or struct it is applied to for
/// registration in the IoC container for dependency injection.
/// </summary>
public sealed class ServiceAttribute : ServiceAttributeBase
{
    /// <summary>
    /// Marks this class for registration in the IoC container for dependency injection.
    /// </summary>
    /// <param name="lifespan">The lifespan of the service.</param>
    /// <param name="allowMultiple">
    /// <see langword="true"/> if the service can be registered alongside other
    /// services under the same interface, <see langword="false"/> otherwise.
    /// </param>
    public ServiceAttribute(ServiceLifetime lifespan, bool allowMultiple = false) : base(lifespan, allowMultiple)
    {
    }

    /// <inheritdoc/>
    protected override void RegisterSingleton(IServiceCollection ioc, Type concreteType)
        => ioc.AddSingleton(concreteType);

    /// <inheritdoc/>
    protected override void RegisterScoped(IServiceCollection ioc, Type concreteType)
        => ioc.AddScoped(concreteType);

    /// <inheritdoc/>
    protected override void RegisterTransient(IServiceCollection ioc, Type concreteType)
        => ioc.AddTransient(concreteType);
}