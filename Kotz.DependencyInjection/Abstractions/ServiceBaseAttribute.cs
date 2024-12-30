using Kotz.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Kotz.DependencyInjection.Abstractions;

/// <summary>
/// Defines the base interface for an attribute that registers a service.
/// </summary>
/// <remarks>
/// If you are inheriting from a class that contains this attribute,
/// don't forget to also apply this attribute to the derived class.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public abstract class ServiceBaseAttribute : Attribute
{
    private readonly Action<IServiceCollection, Type> _serviceRegister;

    /// <summary>
    /// Defines whether the registered type is allowed
    /// to have multiple implementations in the IoC.
    /// </summary>
    protected bool AllowMultiple { get; }

    /// <summary>
    /// Defines the lifetime of this service.
    /// </summary>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    /// Creates the base interface for an attribute that registers a service.
    /// </summary>
    /// <param name="lifespan">The lifespan of the service.</param>
    /// <param name="allowMultiple">
    /// <see langword="true"/> if the service can be registered alongside other
    /// services under the same interface, <see langword="false"/> otherwise.
    /// </param>
    /// <exception cref="UnreachableException"> Occurs when a value for <paramref name="lifespan"/> is not implemented.</exception>
    protected ServiceBaseAttribute(ServiceLifetime lifespan, bool allowMultiple)
    {
        Lifetime = lifespan;
        AllowMultiple = allowMultiple;

        _serviceRegister = lifespan switch
        {
            ServiceLifetime.Singleton => RegisterSingleton,
            ServiceLifetime.Scoped => RegisterScoped,
            ServiceLifetime.Transient => RegisterTransient,
            _ => throw new UnreachableException($"There is no registration available for {lifespan} services."),
        };
    }

    /// <summary>
    /// Registers the specified type as a service in the IoC container.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The type of the service to be registered.</param>
    /// <exception cref="ArgumentException">Occurs when a service of type <paramref name="concreteType"/> is already registered.</exception>
    public virtual void RegisterService(IServiceCollection ioc, Type concreteType)
    {
        if (!AllowMultiple && ioc.Any(x => x.ImplementationType == concreteType || x.ServiceType == concreteType))
            throw new ArgumentException(GetRegistrationPresenceError(ioc, concreteType));

        _serviceRegister(ioc, concreteType);
    }

    /// <summary>
    /// Registers the specified type as a singleton service.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The type of the service to be registered.</param>
    protected abstract void RegisterSingleton(IServiceCollection ioc, Type concreteType);

    /// <summary>
    /// Registers the specified type as a scoped service.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The type of the service to be registered.</param>
    protected abstract void RegisterScoped(IServiceCollection ioc, Type concreteType);

    /// <summary>
    /// Registers the specified type as a transient service.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The type of the service to be registered.</param>
    protected abstract void RegisterTransient(IServiceCollection ioc, Type concreteType);

    /// <summary>
    /// Gets the formatted error message for a service that has already been registered.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The concrete type of the service.</param>
    /// <param name="abstractType">The interface of the service, or <see langword="null"/> if there isn't one.</param>
    /// <returns>The error message.</returns>
    protected static string GetRegistrationPresenceError(IServiceCollection ioc, Type concreteType, Type? abstractType = default)
    {
        var (serviceName, relationshipTypeNames) = GetRegisteredServiceError(ioc, concreteType, abstractType);

        return @$"Service of type ""{serviceName}"" is already registered." + Environment.NewLine +
            @"If this was intended, set ""allowMultiple"" in the attribute's constructor to ""true""." + Environment.NewLine +
            $"[Present] {relationshipTypeNames}" + Environment.NewLine +
            $"[Attempted] " + ((abstractType is null) ? concreteType.Name : $"{abstractType.Name}: {concreteType.Name}");
    }

    /// <summary>
    /// Gets the error message for a service in the IoC container.
    /// </summary>
    /// <param name="ioc">The IoC container.</param>
    /// <param name="concreteType">The concrete type of the service.</param>
    /// <param name="abstractType">The interface of the service, or <see langword="null"/> if there isn't one.</param>
    /// <returns>The name of the registered concrete type and the relationship of the registered types.</returns>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="concreteType"/> and <paramref name="abstractType"/> have not been registered to the <paramref name="ioc"/>.
    /// </exception>
    private static (string, string) GetRegisteredServiceError(IServiceCollection ioc, Type concreteType, Type? abstractType = default)
    {
        var registeredService = ioc.First(x => x.ImplementationType?.EqualsAny(concreteType, abstractType) is true || x.ServiceType.EqualsAny(concreteType, abstractType));
        var registeredConcreteType = (registeredService.ImplementationInstance is null)
            ? registeredService.ImplementationType?.Name
            : registeredService.ImplementationInstance.GetType().Name + " (instance)";

        return (registeredConcreteType ?? registeredService.ServiceType.Name,
            registeredService.ServiceType.Name +
            ((string.IsNullOrWhiteSpace(registeredConcreteType) || registeredService.ServiceType.Name.Equals(registeredConcreteType, StringComparison.Ordinal))
                ? string.Empty
                : ": " + registeredConcreteType));
    }
}