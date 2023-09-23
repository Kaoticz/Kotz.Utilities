using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents an empty scoped service.
/// </summary>
[Service(ServiceLifetime.Scoped)]
internal sealed record EmptyScopedService();