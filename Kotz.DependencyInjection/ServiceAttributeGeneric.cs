using Kotz.DependencyInjection.Abstractions;
using Kotz.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.DependencyInjection;

/// <summary>
/// This attribute marks the class or struct it is applied to for
/// registration in the IoC container for dependency injection.
/// </summary>
/// <typeparam name="T">The abstract type the service should be registered under.</typeparam>
public sealed class ServiceAttribute<T> : ServiceAttributeBase
{
    /// <summary>
    /// Defines the type this service should be registered as.
    /// </summary>
    public Type AbstractionType { get; } = typeof(T);

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

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="concreteType"/> is not assignable to <see cref="ServiceAttribute{T}.AbstractionType"/>.
    /// </exception>
    public override void RegisterService(IServiceCollection ioc, Type concreteType)
    {
        if (!concreteType.IsAssignableTo(AbstractionType))
            throw new InvalidOperationException(GetRegistrationInheritanceError(concreteType));

        if (!AllowMultiple && ioc.Any(x => x.ImplementationType?.EqualsAny(AbstractionType, concreteType) is true || x.ServiceType.EqualsAny(AbstractionType, concreteType)))
            throw new ArgumentException(GetRegistrationPresenceError(ioc, concreteType, AbstractionType));

        base.RegisterService(ioc, concreteType);
    }

    /// <inheritdoc />
    protected override void RegisterSingleton(IServiceCollection ioc, Type concreteType)
        => ioc.AddSingleton(AbstractionType, concreteType);

    /// <inheritdoc />
    protected override void RegisterScoped(IServiceCollection ioc, Type concreteType)
        => ioc.AddScoped(AbstractionType, concreteType);

    /// <inheritdoc />
    protected override void RegisterTransient(IServiceCollection ioc, Type concreteType)
        => ioc.AddTransient(AbstractionType, concreteType);

    /// <summary>
    /// Gets the error message for when the service type doesn't match the provided interface.
    /// </summary>
    /// <param name="concreteType">The type of the service to be registered.</param>
    /// <returns>The error message.</returns>
    private string GetRegistrationInheritanceError(Type concreteType)
        => @$"Type ""{concreteType.Name}"" does not {((AbstractionType.IsInterface) ? "implement" : "inherit")} ""{AbstractionType.Name}"".";
}