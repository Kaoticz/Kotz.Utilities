using Kotz.DependencyInjection;
using Kotz.Tests.DependencyInjection.Models.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents an empty scoped service that implements <see cref="IEmpty"/>.
/// </summary>
[Service<IEmpty>(ServiceLifetime.Scoped, true)]
internal sealed record EmptyInterfacedScopedService() : IEmpty;