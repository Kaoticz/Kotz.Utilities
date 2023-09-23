using Kotz.DependencyInjection.Extensions;
using Kotz.Tests.DependencyInjection.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Kotz.Tests.DependencyInjection.Extensions;

public sealed class GetParameterizedServiceTests
{
    private static readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .RegisterServices()
        .BuildServiceProvider(true);

    [Theory]
    [InlineData(typeof(MockSingletonService), 0, "A")]
    [InlineData(typeof(MockScopedService), 1, "B")]
    [InlineData(typeof(MockTransientService), 2, "C")]
    internal void FullInitializationTest(Type serviceType, params object[] arguments)
    {
        var normalObject = Activator.CreateInstance(serviceType, arguments);
        var service1 = _serviceProvider.GetParameterizedService(serviceType, arguments);
        var service2 = _serviceProvider.GetParameterizedService(serviceType, arguments);

        Assert.Equivalent(normalObject, service1);
        Assert.StrictEqual(service1, service2);
        Assert.False(ReferenceEquals(normalObject, service1));
        Assert.False(ReferenceEquals(service1, service2));
    }

    [Theory]
    [InlineData(typeof(MockSingletonPartialService), typeof(EmptySingletonService), 0, "A")]
    [InlineData(typeof(MockScopedPartialService), typeof(EmptySingletonService), 1, "B")]
    [InlineData(typeof(MockTransientPartialService), typeof(EmptySingletonService), 2, "C")]
    internal void PartialInitializationTest(Type serviceType, Type dependencyType, params object[] arguments)
    {
        var normalObject = Activator.CreateInstance(serviceType, arguments.Prepend(_serviceProvider.GetRequiredService(dependencyType)).ToArray());
        var service1 = _serviceProvider.GetParameterizedService(serviceType, arguments);
        var service2 = _serviceProvider.GetParameterizedService(serviceType, arguments);

        Assert.Equivalent(normalObject, service1);
        Assert.StrictEqual(service1, service2);
        Assert.False(ReferenceEquals(normalObject, service1));
        Assert.False(ReferenceEquals(service1, service2));
    }

    [Theory]
    [InlineData(typeof(MockSingletonService))]
    [InlineData(typeof(MockScopedService))]
    [InlineData(typeof(MockTransientService))]
    [InlineData(typeof(MockSingletonPartialService))]
    [InlineData(typeof(MockScopedPartialService))]
    [InlineData(typeof(MockTransientPartialService))]
    internal void InitializationFailTest(Type serviceType)
        => Assert.Throws<InvalidOperationException>(() => _serviceProvider.GetParameterizedService(serviceType, Array.Empty<object>()));
}