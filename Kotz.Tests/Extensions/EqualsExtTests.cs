using Kotz.Extensions;
using Kotz.Tests.Models;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class EqualsAnyTests
{
    private static readonly List<MockObject> _dummies = Enumerable.Range(0, 10)
        .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
        .ToList();

    [Theory]
    [InlineData(1, "B", true)]
    [InlineData(2, "C", true)]
    [InlineData(9, "J", true)]
    [InlineData(-1, "H", false)]
    [InlineData(999, "D", false)]
    [InlineData(4, "C", false)]
    [InlineData(2, null, false)]
    internal void EqualsAnyTest(int id, string name, bool result)
        => Assert.Equal(result, new MockObject(id, name).EqualsAny(_dummies));

    [Theory]
    [InlineData(null, false)]
    internal void EqualsAnyNullTests(object thisObject, bool result)
    {
        Assert.Equal(result, thisObject.EqualsAny(_dummies));
        Assert.Equal(result, _dummies.EqualsAny(thisObject));
        Assert.True(thisObject.EqualsAny(null!));
    }
}