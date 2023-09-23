using System.Reflection;

namespace Kotz.DependencyInjection.Extensions;

/// <summary>
/// Contains internal extension methods for <see cref="Assembly"/>.
/// </summary>
internal static class AssemblyExt
{
    /// <summary>
    /// Gets a collection of all concrete classes that contain an attribute of type <typeparamref name="T"/> in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to search from.</param>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <returns>A collection of all concrete types with an attribute of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">Occurs when <typeparamref name="T"/> is not an attribute.</exception>
    internal static IEnumerable<Type> GetConcreteTypesWithAttribute<T>(this Assembly assembly) where T : Attribute
        => GetConcreteTypesWithAttribute(assembly, typeof(T));

    /// <summary>
    /// Gets a collection of all concrete classes that contain an attribute of the specified type in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to search from.</param>
    /// <param name="attributeType">The type of the attribute to search for.</param>
    /// <returns>A collection of all concrete types with an attribute of <paramref name="attributeType"/>.</returns>
    /// <exception cref="ArgumentException">Occurs when <paramref name="attributeType"/> is not an attribute.</exception>
    internal static IEnumerable<Type> GetConcreteTypesWithAttribute(this Assembly assembly, Type attributeType)
    {
        return (!attributeType.IsAssignableTo(typeof(Attribute)))
            ? throw new ArgumentException("Type must be an attribute.", nameof(attributeType))
            : assembly.GetTypes()
                .Where(type =>
                    !type.IsInterface
                    && !type.IsAbstract
                    && !type.IsNested
                    && type.CustomAttributes.Any(x => x.AttributeType == attributeType || x.AttributeType.IsAssignableTo(attributeType))
            );
    }
}