namespace Kotz.Tests.Extensions;

public sealed class TaskTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task AwaitAsyncTestAsync(bool succeed)
    {
        var originalTask = DelayAsync(TimeSpan.FromMilliseconds(100), succeed);
        var awaitedTask = await originalTask.AwaitAsync();

        Assert.Same(originalTask, awaitedTask);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task AwaitAsyncGenericTestAsync(bool succeed)
    {
        var originalTask = DelayAsync(10, TimeSpan.FromMilliseconds(100), succeed);
        var awaitedTask = await originalTask.AwaitAsync();

        Assert.Same(originalTask, awaitedTask);
    }

    /// <summary>
    /// Asynchronously wait for the specified amount of time.
    /// </summary>
    /// <param name="timeSpan">The time to wait for.</param>
    /// <param name="succeed">Defines whether an exception should be thrown after the wait time has elapsed.</param>
    /// <exception cref="InvalidOperationException" />
    private static async Task DelayAsync(TimeSpan timeSpan, bool succeed)
    {
        await Task.Delay(timeSpan);

        if (!succeed)
            throw new InvalidOperationException("Mock operation has failed.");
    }

    /// <summary>
    /// Asynchronously wait for the specified amount of time.
    /// </summary>
    /// <param name="toReturn">The value to return from the method.</param>
    /// <param name="timeSpan">The time to wait for.</param>
    /// <param name="succeed">Defines whether an exception should be thrown after the wait time has elapsed.</param>
    /// <exception cref="InvalidOperationException" />
    private static async Task<T> DelayAsync<T>(T toReturn, TimeSpan timeSpan, bool succeed)
    {
        await Task.Delay(timeSpan);

        return (succeed)
            ? toReturn
            : throw new InvalidOperationException("Mock operation has failed.");
    }
}