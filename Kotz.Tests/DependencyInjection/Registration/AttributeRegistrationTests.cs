using Kotz.DependencyInjection.Abstractions;
using Kotz.DependencyInjection.Extensions;
using Kotz.Tests.DependencyInjection.Models;
using Kotz.Tests.DependencyInjection.Models.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Kotz.Tests.DependencyInjection.Registration;

public sealed class AttributeRegistrationTests
{
    private static readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .RegisterServices()
        .BuildServiceProvider(true);

    [Fact]
    internal void NotRegisteredTest()
        => Assert.Throws<InvalidOperationException>(_serviceProvider.GetRequiredService<NotRegisteredService>);

    [Theory]
    [InlineData(typeof(EmptySingletonService))]
    [InlineData(typeof(EmptyScopedService))]
    [InlineData(typeof(EmptyTransientService))]
    internal void ConcreteResolutionTest(Type serviceType)
    {
        using var scope = _serviceProvider.CreateScope();
        var service1 = scope.ServiceProvider.GetRequiredService(serviceType);
        var service2 = scope.ServiceProvider.GetRequiredService(serviceType);
        var attribute = serviceType.GetCustomAttribute<ServiceAttributeBase>()!;

        if (attribute.Lifetime is ServiceLifetime.Transient)
            Assert.False(ReferenceEquals(service1, service2));
        else
            Assert.True(ReferenceEquals(service1, service2));
    }

    [Theory]
    [InlineData(typeof(EmptyInterfacedSingletonService), typeof(IEmpty))]
    [InlineData(typeof(EmptyInterfacedScopedService), typeof(IEmpty))]
    [InlineData(typeof(EmptyInterfacedTransientService), typeof(IEmpty))]
    internal void AbstractResolutionTest(Type serviceType, Type abstractType)
    {
        var attribute = serviceType.GetCustomAttribute<ServiceAttributeBase>()!;
        var serviceCollection = new ServiceCollection()
            .RegisterServices()
            .RemoveAll(abstractType);

        attribute.RegisterService(serviceCollection, serviceType);

        var serviceProvider = serviceCollection.BuildServiceProvider(true);
        using var scope = serviceProvider.CreateScope();
        var service1 = scope.ServiceProvider.GetRequiredService(abstractType);
        var service2 = scope.ServiceProvider.GetRequiredService(abstractType);

        Assert.IsType(serviceType, service1);
        Assert.True(service1.GetType().IsAssignableTo(abstractType));

        if (attribute.Lifetime is ServiceLifetime.Transient)
            Assert.False(ReferenceEquals(service1, service2));
        else
            Assert.True(ReferenceEquals(service1, service2));
    }
}