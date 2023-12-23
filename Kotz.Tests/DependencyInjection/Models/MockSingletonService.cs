using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents a singleton service.
/// </summary>
/// <param name="Id">An identifying value.</param>
/// <param name="Name">An identifying value.</param>
[Service(ServiceLifetime.Singleton)]
internal sealed record MockSingletonService(int Id, string Name);