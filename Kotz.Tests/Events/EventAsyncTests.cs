using Kotz.Events;

namespace Kotz.Tests.Events;

public sealed class EventAyncTests
{
    private readonly AsyncEvent<EventAyncTests, EventArgs> _asyncEvent = new();

    internal int Count { get; private set; }

    [Theory]
    [InlineData(100, 400, 800)]
    [InlineData(100, 800, 400)]
    [InlineData(800, 400, 100)]
    [InlineData(800, 100, 400)]
    internal async Task WaitForAllHandlersToCompleteTestAsync(params int[] milliseconds)
    {
        var longest = milliseconds.Max();

        foreach (var second in milliseconds)
            _asyncEvent.Handler += (_, _) => WaitAndSetCountAsync(TimeSpan.FromMilliseconds(second));

        foreach (var millisecond in milliseconds)
        {
            // Invoke all handlers.
            // Then wait for the current one to finish executing and assert that it actually ran.
            // Then wait for all of them to complete, and redo the checks for the next registered handler.
            var invocationTask = _asyncEvent.InvokeAsync(this, EventArgs.Empty);

            await Task.WhenAny(Task.Delay(TimeSpan.FromMilliseconds(millisecond + 150)), invocationTask);
            Assert.Equal(millisecond, Count);

            await invocationTask;
            Assert.Equal(longest, Count);
        }
    }

    /// <summary>
    /// Waits for the specified amount of <paramref name="time"/>,
    /// then set <see cref="Count"/> to the time in milliseconds.
    /// </summary>
    /// <param name="time">The time to wait for.</param>
    private async Task WaitAndSetCountAsync(TimeSpan time)
    {
        await Task.Delay(time);

        Count = time.Milliseconds;

        await Task.Delay(TimeSpan.FromMilliseconds(30));
    }
}