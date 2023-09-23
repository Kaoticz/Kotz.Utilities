using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents a scoped service.
/// </summary>
/// <param name="Id">An identifying value.</param>
/// <param name="Name">An identifying value.</param>
[Service(ServiceLifetime.Scoped)]
internal sealed record MockScopedService(int Id, string Name);
