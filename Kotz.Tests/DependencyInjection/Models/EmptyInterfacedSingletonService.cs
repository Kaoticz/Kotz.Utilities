using Kotz.DependencyInjection;
using Kotz.Tests.DependencyInjection.Models.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Models;

/// <summary>
/// Represents an empty singleton service that implements <see cref="IEmpty"/>.
/// </summary>
[Service<IEmpty>(ServiceLifetime.Singleton, true)]
internal sealed record EmptyInterfacedSingletonService() : IEmpty;