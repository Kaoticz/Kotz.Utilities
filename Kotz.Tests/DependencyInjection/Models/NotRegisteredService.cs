using Kotz.DependencyInjection.Abstractions;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents a service that has not been registered with the <see cref="ServiceBaseAttribute"/> attribute.
/// </summary>
internal sealed record NotRegisteredService();