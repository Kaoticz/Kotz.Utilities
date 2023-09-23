using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents a singleton service.
/// </summary>
/// <param name="EmptyService">An empty service.</param>
/// <param name="Id">An identifying value.</param>
/// <param name="Name">An identifying value.</param>
[Service(ServiceLifetime.Singleton)]
internal sealed record MockSingletonPartialService(EmptySingletonService EmptyService, int Id, string Name);