using Kotz.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kotz.DependencyInjection.Extensions;

/// <summary>
/// Defines extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class IServiceCollectionExt
{
    /// <summary>
    /// Registers all types in the current assembly that are marked with a
    /// <see cref="ServiceAttributeBase"/> attribute to this service collection.
    /// </summary>
    /// <param name="serviceCollection">This service collection.</param>
    /// <returns>This service collection with the services registered in it.</returns>
    /// <exception cref="InvalidOperationException">
    /// Occurs when a service is registered under an abstract type it has no relation with.
    /// </exception>
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        => RegisterServices(serviceCollection, Assembly.GetCallingAssembly());

    /// <summary>
    /// Registers all types in the specifyed <paramref name="assembly"/> that are marked
    /// with a <see cref="ServiceAttributeBase"/> attribute to this service collection.
    /// </summary>
    /// <param name="serviceCollection">This service collection.</param>
    /// <param name="assembly">The assembly to get the types from.</param>
    /// <returns>This service collection with the services registered in it.</returns>
    /// <exception cref="InvalidOperationException">
    /// Occurs when a service is registered under an abstract type it has no relation with.
    /// </exception>
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var typesAndAttributes = assembly.GetConcreteTypesWithAttribute<ServiceAttributeBase>()
            .Select(x => (Type: x, Attribute: x.GetCustomAttribute<ServiceAttributeBase>()!));

        foreach (var (type, attribute) in typesAndAttributes)
            attribute.RegisterService(serviceCollection, type);

        return serviceCollection;
    }
}