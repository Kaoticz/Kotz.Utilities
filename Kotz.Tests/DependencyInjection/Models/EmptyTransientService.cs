using Kotz.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents an empty transient service.
/// </summary>
[Service(ServiceLifetime.Transient)]
internal sealed record EmptyTransientService();