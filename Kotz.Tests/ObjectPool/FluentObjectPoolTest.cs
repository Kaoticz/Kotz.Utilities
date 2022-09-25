using Kotz.ObjectPool;
using System.Text;
using Xunit;

namespace Kotz.Tests.ObjectPool;

public sealed class FluentObjectPoolTest
{
    private readonly Func<StringBuilder> _objectFactory = () => new();
    private readonly Func<StringBuilder, StringBuilder> _objectResetter = x => x.Clear();
    private readonly Func<StringBuilder, bool> _objectFilter = x => x.Length % 2 is 0;

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    internal void CreateTest(int maximumRetained)
    {
        // Doesn't throw
        var policy = new DefaultFluentPooledObjectPolicy<StringBuilder>(_objectFactory, _objectFilter);
        _ = new FluentObjectPool<StringBuilder>(policy, maximumRetained);
        _ = new FluentObjectPool<StringBuilder>(_objectFactory, _objectResetter, _objectFilter);
        _ = new FluentObjectPool<StringBuilder>(_objectFactory, null, null);
        _ = new FluentObjectPool<StringBuilder>(_objectFactory, _objectResetter, _objectFilter, maximumRetained);
        _ = new FluentObjectPool<StringBuilder>(_objectFactory, null, null, maximumRetained);

        // Throws
        Assert.Throws<ArgumentNullException>(() => new FluentObjectPool<StringBuilder>(null!, maximumRetained));
        Assert.Throws<ArgumentNullException>(() => new FluentObjectPool<StringBuilder>(null!, _objectResetter, _objectFilter));
        Assert.Throws<ArgumentNullException>(() => new FluentObjectPool<StringBuilder>(null!, _objectResetter, _objectFilter, maximumRetained));
    }

    [Theory]
    [InlineData(true, 6)]
    [InlineData(false, 5)]
    internal void PoolCycleTest(bool isReused, int stringLength)
    {
        var testData = new string('-', stringLength);
        var pool = new FluentObjectPool<StringBuilder>(_objectFactory, null, _objectFilter);

        // Create new
        var obj = pool.Get();

        // Modify and return
        obj.Append(testData);
        pool.Return(obj);

        // Test
        var newObj = pool.Get();
        if (isReused)
        {
            Assert.Equal(testData, newObj.ToString());
            Assert.StrictEqual(obj, newObj);
        }
        else
        {
            Assert.NotEqual(testData, newObj.ToString());
            Assert.NotStrictEqual(obj, newObj);
        }
    }

    [Fact]
    internal void PoolCycleWithResetTest()
    {
        var testData = "test";
        var pool = new FluentObjectPool<StringBuilder>(_objectFactory, _objectResetter, _objectFilter);

        // Create new
        var obj = pool.Get();

        // Modify and return
        obj.Append(testData);
        pool.Return(obj);

        // Test
        var newObj = pool.Get();

        Assert.NotEqual(testData, newObj.ToString());
        Assert.StrictEqual(obj, newObj);
    }
}