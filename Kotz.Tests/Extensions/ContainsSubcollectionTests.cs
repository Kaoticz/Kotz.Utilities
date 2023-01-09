namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.TrueSubcollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsSubcollectionTrueTest(IEnumerable<MockObject> original, IEnumerable<MockObject> subcollection)
        => Assert.True(original.ContainsSubcollection(subcollection));

    [Theory]
    [MemberData(nameof(MockCollectionTestData.FalseSubcollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsSubcollectionFalseTest(IEnumerable<MockObject> original, IEnumerable<MockObject> subcollection)
        => Assert.False(original.ContainsSubcollection(subcollection));

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsSubcollectionEmptyTest(IEnumerable<MockObject> collection)
    {
        Assert.False(collection.ContainsSubcollection(Enumerable.Empty<MockObject>()));
        Assert.False(Enumerable.Empty<MockObject>().ContainsSubcollection(collection));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsSubcollectionNullTest(IEnumerable<MockObject> collection)
    {
        IEnumerable<MockObject> nullCollection = null!;

        Assert.Throws<ArgumentNullException>(() => collection.ContainsSubcollection(null!));
        Assert.Throws<ArgumentNullException>(() => nullCollection.ContainsSubcollection(collection));
    }
}