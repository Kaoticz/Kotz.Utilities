using System.Reflection;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Assembly"/>.
/// </summary>
public static class AssemblyExt
{
    /// <summary>
    /// Gets all concrete types from this assembly.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <returns>A collection of concrete types.</returns>
    public static IEnumerable<Type> GetConcreteTypes(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        return assembly.GetTypes()
            .Where(type => !type.IsInterface && !type.IsAbstract);
    }

    /// <summary>
    /// Gets all abstract types from this assembly.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <remarks>This includes interfaces and abstract classes.</remarks>
    /// <returns>A collection of abstract types.</returns>
    public static IEnumerable<Type> GetAbstractTypes(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        return assembly.GetTypes()
            .Where(type => type.IsInterface || type.IsAbstract);
    }

    /// <summary>
    /// Gets concrete types from this assembly that derive from <typeparamref name="T"/>.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <typeparam name="T">The parent type to search for.</typeparam>
    /// <remarks>If <typeparamref name="T"/> is a concrete type, it will be returned in the result.</remarks>
    /// <returns>A collection of concrete types derived from <typeparamref name="T"/>.</returns>
    public static IEnumerable<Type> GetConcreteTypesOf<T>(this Assembly assembly)
        => GetConcreteTypesOf(assembly, typeof(T));

    /// <summary>
    /// Gets concrete types from this assembly that derive from <paramref name="implementationType"/>.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <param name="implementationType">The parent type to search for.</param>
    /// <remarks>If <paramref name="implementationType"/> is a concrete type, it will be returned in the result.</remarks>
    /// <returns>A collection of concrete types derived from <paramref name="implementationType"/>.</returns>
    public static IEnumerable<Type> GetConcreteTypesOf(this Assembly assembly, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType, nameof(implementationType));
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        return assembly.GetTypes()
            .Where(type => !type.IsInterface && !type.IsAbstract && implementationType.IsAssignableFrom(type));
    }

    /// <summary>
    /// Gets abstract types from this assembly that derive from <typeparamref name="T"/>.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <typeparam name="T">The parent type to search for.</typeparam>
    /// <remarks>
    /// This includes interfaces and abstract classes. <br />
    /// If <typeparamref name="T"/> is an abstract type, it will be returned in the result.
    /// </remarks>
    /// <returns>A collection of abstract types derived from <typeparamref name="T"/>.</returns>
    public static IEnumerable<Type> GetAbstractTypesOf<T>(this Assembly assembly)
        => GetAbstractTypesOf(assembly, typeof(T));

    /// <summary>
    /// Gets abstract types from this assembly that derive from <paramref name="implementationType"/>.
    /// </summary>
    /// <param name="assembly">This assembly.</param>
    /// <param name="implementationType">The parent type to search for.</param>
    /// <remarks>
    /// This includes interfaces and abstract classes. <br />
    /// If <paramref name="implementationType"/> is an abstract type, it will be returned in the result.
    /// </remarks>
    /// <returns>A collection of abstract types derived from <paramref name="implementationType"/>.</returns>
    public static IEnumerable<Type> GetAbstractTypesOf(this Assembly assembly, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType, nameof(implementationType));
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        return assembly.GetTypes()
            .Where(type => (type.IsInterface || type.IsAbstract) && implementationType.IsAssignableFrom(type));
    }
}