using Kotz.Extensions;
using Kotz.Tests.Models;
using Xunit;

namespace Kotz.Tests.Extensions;

// Old code, needs to be refactored
public sealed partial class LinqExtTests
{
    private readonly List<MockObject> _nullCollection = null!;
    private readonly List<MockObject> _dummiesEmpty = Enumerable.Empty<MockObject>().ToList();

    private readonly List<MockObject> _dummies;
    private readonly List<MockObject> _dummiesSubcollectionTrue;
    private readonly List<MockObject> _dummiesSubcollectionFalse;
    private readonly List<MockObject> _dummiesWithNull;

    public LinqExtTests()
    {
        _dummies = Enumerable.Range(0, 10)
            .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
            .ToList();

        _dummiesSubcollectionTrue = Enumerable.Range(0, 10)
            .Where(x => x % 2 is 0)
            .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
            .ToList();

        _dummiesSubcollectionFalse = Enumerable.Range(0, 12)
            .Where(x => x % 2 is 0)
            .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
            .ToList();

        _dummiesWithNull = _dummies.ToList();
        _dummiesWithNull[3] = null!;
        _dummiesWithNull[7] = null!;
    }

    [Fact] // Possibly replaced in .NET 6
    internal void ExceptByTest()
    {
        // Should only contain odd ids
        var sample = _dummies
            .ExceptBy(_dummiesSubcollectionTrue, x => x.Id)
            .ToArray();

        Assert.Equal(5, sample.Length);
        Assert.DoesNotContain(sample, x => x.Id % 2 is 0);

        // Should only contain odd ids
        sample = _dummiesSubcollectionTrue
            .ExceptBy(_dummies, x => x.Id)
            .ToArray();

        Assert.Equal(5, sample.Length);
        Assert.DoesNotContain(sample, x => x.Id % 2 is 0);

        // Empty collections
        Assert.NotEmpty(_dummiesEmpty.ExceptBy(_dummies, x => x.Id).ToArray());
        Assert.NotEmpty(_dummies.ExceptBy(_dummiesEmpty, x => x.Id).ToArray());

        // Null elements
        Assert.NotEmpty(_dummies.ExceptBy(_dummiesWithNull, x => x?.Id).ToArray());

        // Null input
        Assert.Throws<ArgumentNullException>(() => _nullCollection.ExceptBy(_dummies, x => x.Id).ToArray());
        Assert.Throws<ArgumentNullException>(() => _dummies.ExceptBy(_nullCollection, x => x.Id).ToArray());
        Assert.Throws<ArgumentNullException>(() => _dummies.ExceptBy<MockObject, int>(_dummiesSubcollectionTrue, null!).ToArray());
        Assert.Throws<NullReferenceException>(() => _dummies.ExceptBy(_dummiesWithNull, x => x.Id).ToArray());
    }

    [Fact] // Possibly replaced in .NET 6
    internal void IntersectByTest()
    {
        // Should only contain even ids
        var sample = _dummies
            .IntersectBy(_dummiesSubcollectionFalse, x => x.Id)
            .ToArray();

        Assert.Equal(5, sample.Length);
        Assert.DoesNotContain(sample, x => x.Id % 2 is not 0);

        // Should only contain even ids
        sample = _dummiesSubcollectionFalse
            .IntersectBy(_dummies, x => x.Id)
            .ToArray();

        Assert.Equal(5, sample.Length);
        Assert.DoesNotContain(sample, x => x.Id % 2 is not 0);

        // Empty collections
        Assert.Empty(_dummiesEmpty.IntersectBy(_dummies, x => x.Id).ToArray());
        Assert.Empty(_dummies.IntersectBy(_dummiesEmpty, x => x.Id).ToArray());

        // Null elements
        Assert.NotEmpty(_dummies.IntersectBy(_dummiesWithNull, x => x?.Id).ToArray());

        // Null input
        Assert.Throws<ArgumentNullException>(() => _nullCollection.IntersectBy(_dummies, x => x.Id).ToArray());
        Assert.Throws<ArgumentNullException>(() => _dummies.IntersectBy(_nullCollection, x => x.Id).ToArray());
        Assert.Throws<ArgumentNullException>(() => _dummies.IntersectBy<MockObject, int>(_dummiesSubcollectionTrue, null!).ToArray());
        Assert.Throws<NullReferenceException>(() => _dummies.IntersectBy(_dummiesWithNull, x => x.Id).ToArray());
    }
}