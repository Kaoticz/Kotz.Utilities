using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents a transient service.
/// </summary>
/// <param name="EmptyService">An empty service.</param>
/// <param name="Id">An identifying value.</param>
/// <param name="Name">An identifying value.</param>
[Service(ServiceLifetime.Transient)]
internal sealed record MockTransientPartialService(EmptySingletonService EmptyService, int Id, string Name);