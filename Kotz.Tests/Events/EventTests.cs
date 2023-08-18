using Kotz.Events;

namespace Kotz.Tests.Events;

public sealed class EventTests
{
    internal Guid Id { get; } = Guid.NewGuid();

    internal int Counter { get; private set; }

    /// <summary>
    /// A generic event handler.
    /// </summary>
    internal event EventHandler<EventTests, EventArgs>? GenericEventHandler;

    /// <summary>
    /// A generic async event handler.
    /// </summary>
    internal event AsyncEventHandler<EventTests, EventArgs>? GenericAsyncEventHandler;

    /// <summary>
    /// An async event handler.
    /// </summary>
    internal event AsyncEventHandler? ObjectAsyncEventHandler;

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    internal void GenericTest(int registrationAmount)
    {
        // Registration
        for (var counter = 0; counter < registrationAmount; counter++)
            GenericEventHandler += TargetMethod;

        // Invocation
        GenericEventHandler?.Invoke(this, EventArgs.Empty);

        Assert.Equal(registrationAmount, Counter);

        // Deregistration
        for (var counter = 0; counter < Counter; counter++)
            GenericEventHandler -= TargetMethod;

        // Invocation
        Assert.Throws<NullReferenceException>(() => GenericEventHandler!.Invoke(this, EventArgs.Empty));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    internal async Task GenericTestAsync(int registrationAmount)
    {
        // Registration
        for (var counter = 0; counter < registrationAmount; counter++)
            GenericAsyncEventHandler += TargetMethodAsync;

        // Invocation
        await (GenericAsyncEventHandler?.Invoke(this, EventArgs.Empty) ?? Task.CompletedTask);

        Assert.Equal(registrationAmount, Counter);

        // Deregistration
        for (var counter = 0; counter < Counter; counter++)
            GenericAsyncEventHandler -= TargetMethodAsync;

        // Invocation
        Assert.Null(GenericAsyncEventHandler?.Invoke(this, EventArgs.Empty));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    internal async Task TestAsync(int registrationAmount)
    {
        // Registration
        for (var counter = 0; counter < registrationAmount; counter++)
            ObjectAsyncEventHandler += TargetMethodAsync;

        // Invocation
        await (ObjectAsyncEventHandler?.Invoke(this, EventArgs.Empty) ?? Task.CompletedTask);

        Assert.Equal(registrationAmount, Counter);

        // Deregistration
        for (var counter = 0; counter < Counter; counter++)
            ObjectAsyncEventHandler -= TargetMethodAsync;

        // Invocation
        Assert.Null(ObjectAsyncEventHandler?.Invoke(this, EventArgs.Empty));
    }

    /// <summary>
    /// Invocation test for <see cref="EventHandler{T1, T2}"/>
    /// </summary>
    /// <exception cref="InvalidOperationException" />.
    private void TargetMethod(EventTests sender, EventArgs eventArgs)
    {
        if (Id != sender.Id)
            throw new InvalidOperationException("Event test ID did not match.");

        Counter += 1;
    }

    /// <summary>
    /// Invocation test for <see cref="AsyncEventHandler{T1, T2}"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    private Task TargetMethodAsync(EventTests sender, EventArgs eventArgs)
    {
        if (Id != sender.Id)
            throw new InvalidOperationException("Event test ID did not match.");

        Counter += 1;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Invocation test for <see cref="AsyncEventHandler"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    private Task TargetMethodAsync(object? sender, EventArgs eventArgs)
    {
        if (Id != (sender as EventTests)?.Id)
            throw new InvalidOperationException("Event test ID did not match.");

        Counter += 1;

        return Task.CompletedTask;
    }
}