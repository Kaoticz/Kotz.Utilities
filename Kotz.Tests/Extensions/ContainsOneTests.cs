namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsOneNullTest(IEnumerable<object> collection)
    {
        IEnumerable<object> nullCollection = null!;

        Assert.Throws<ArgumentNullException>(() => collection.ContainsOne(null!));
        Assert.Throws<ArgumentNullException>(() => nullCollection.ContainsOne(collection));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.TrueSubcollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.FalseSubcollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsOneTrueTest(IEnumerable<object> original, IEnumerable<object> subCollection)
    {
        Assert.True(original.ContainsOne(new MockObject(0, char.ConvertFromUtf32(65))));
        Assert.True(original.ContainsOne(subCollection));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsOneFalseTest(IEnumerable<object> collection)
    {
        Assert.False(collection.ContainsOne());
        Assert.False(collection.ContainsOne(Enumerable.Empty<MockObject>()));
        Assert.False(Enumerable.Empty<MockObject>().ContainsOne(collection));
    }
}