using Kotz.Extensions;
using Kotz.Tests.Models;
using Kotz.Tests.TestData;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    /// <summary>
    /// Contains data for splittable tests.
    /// </summary>
    public static IEnumerable<object[]> SplittableCollection { get; } = new object[][]
    {
        GetSplittableTestData(MockCollectionTestData.Collection),
        GetSplittableTestData(MockCollectionTestData.CollectionWithNull)
    };

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ChunkByNullTest(IEnumerable<MockObject> collection)
    {
        if (collection is null)
            Assert.Throws<ArgumentNullException>(() => collection!.ChunkBy(x => x.Id));

        Assert.Throws<ArgumentNullException>(() => collection!.ChunkBy<MockObject, int>(null!));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ChunkByEmptyTest(IEnumerable<MockObject> collection)
        => Assert.Empty(collection.ChunkBy(x => x.Id));

    [Theory]
    [MemberData(nameof(SplittableCollection))]
    internal void ChunkByTest(IEnumerable<MockObject> original, IEnumerable<MockObject> firstHalf, IEnumerable<MockObject> secondHalf)
    {
        Assert.True(original.Select(x => x.Name).ContainsSubcollection(firstHalf.Select(x => x.Name)));
        Assert.True(original.Select(x => x.Name).ContainsSubcollection(secondHalf.Select(x => x.Name)));

        Assert.True(firstHalf.All(x => x.Id == 1));
        Assert.True(secondHalf.All(x => x.Id == 2));

        Assert.Equal(original.Count() / 2, firstHalf.Count());
        Assert.Equal(original.Count() / 2, secondHalf.Count());
    }

    /// <summary>
    /// Gets splittable data for a given theory dataset.
    /// </summary>
    /// <param name="source">The theory data.</param>
    /// <returns>{ source[], firstHalf[], secondHalf[] }</returns>
    private static IEnumerable<MockObject>[] GetSplittableTestData(IEnumerable<object[]> source)
    {
        static IEnumerable<MockObject> GetMockQuery(IEnumerable<object[]> source)
            => source
                .First()
                .Cast<MockObject[]>()
                .SelectMany(x => x)
                .Where(x => x is not null);

        return GetMockQuery(source)
            .Select(x => new MockObject((x.Id % 2 is 0) ? 1 : 2, x.Name))
            .ChunkBy(x => x.Id)
            .Prepend(GetMockQuery(source))
            .ToArray();
    }
}