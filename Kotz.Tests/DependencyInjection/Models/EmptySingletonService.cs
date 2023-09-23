using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents an empty singleton service.
/// </summary>
[Service(ServiceLifetime.Singleton)]
internal sealed record EmptySingletonService();