# Kotz.DependencyInjection

- Dependencies:
    - `Microsoft.Extensions.DependencyInjection.Abstractions`

Defines the following extension methods:

- **IServiceCollection Extensions**
    - RegisterServices: Registers all classes and structs from an assembly that have a `ServiceAttributeBase` attribute applied to them.
- **IServiceProvider and IServiceScopeFactory Extensions**
    - GetParameterizedService: Gets a service that requires arguments that are not registered in the IoC container.
    - GetScopedService: Gets a service that needs to be resolved from a scope.

Defines the following types:

- ServiceAttributeBase: It's the base class for dependency injection registrations.
- ServiceAttribute: Marks a class or struct for registration via the `IServiceCollection.RegisterServices` extension method.
- ServiceAttribute\<T>: Marks a class or struct for registration under the type `T` via the `IServiceCollection.RegisterServices` extension method.

## Using the registration attributes

1. Registering the type directly
```cs
// Registration.
[Service(ServiceLifetime.Singleton)]
public sealed class MyService { ... }

// Resolution.
serviceProvider.GetRequiredService<MyService>();
```

2. Registering the type through another type.
```cs
// Registration.
[Service<ISomething>(ServiceLifetime.Scoped)]
public sealed class MyService : ISomething { ... }

// Resolution.
serviceProvider.GetRequiredService<ISomething>();
```

3. Registering multiple types under the same type.
```cs
[Service<ISomething>(ServiceLifetime.Transient, true)]
public sealed class MyService1 : ISomething { ... }

[Service<ISomething>(ServiceLifetime.Transient, true)]
public sealed class MyService2 : ISomething { ... }

[Service<ISomething>(ServiceLifetime.Transient, true)]
public sealed class MyService3 : ISomething { ... }
```

4. Registering the services in the `IServiceCollection`.
```cs
// To register services from the current assembly.
serviceCollection.RegisterServices();

// OR

// To register services from a specific assembly.
serviceCollection.RegisterServices(someAssembly);
```