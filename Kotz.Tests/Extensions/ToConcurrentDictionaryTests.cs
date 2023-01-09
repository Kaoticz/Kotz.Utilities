namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryNullTest(IEnumerable<MockObject> collection)
    {
#nullable disable warnings
        Assert.Throws<ArgumentNullException>(() => collection.ToConcurrentDictionary(x => x?.Id));
#nullable enable warnings
        Assert.Throws<ArgumentNullException>(() => collection.ToConcurrentDictionary<int, MockObject>(null!));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryEmptyTest(IEnumerable<MockObject> collection)
    {
        var result = collection.ToConcurrentDictionary(x => x.Id);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryTest(IEnumerable<MockObject> collection)
    {
        var sample = collection.ToConcurrentDictionary(x => x.Id);

        Assert.True(sample.All(x => collection.Contains(x.Value)));    // Verify if elements exist in the original collection
        Assert.True(collection.All(x => sample.Values.Contains(x)));   // Verify if all original elements are in the sample
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryDoubleFuncNullTest(IEnumerable<MockObject> collection)
    {
#nullable disable warnings
        Assert.Throws<ArgumentNullException>(() => collection.ToConcurrentDictionary(x => x?.Id, x => x?.Name));
        Assert.Throws<ArgumentNullException>(() => collection.ToConcurrentDictionary<int, MockObject, string>(x => x?.Id ?? default, null!));
#nullable enable warnings
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryDoubleFuncEmptyTest(IEnumerable<MockObject> collection)
    {
        var result = collection.ToConcurrentDictionary(x => x.Id, x => x.Name);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    internal void ToConcurrentDictionaryDoubleFuncTest(IEnumerable<MockObject> collection)
    {
        var sample = collection.ToConcurrentDictionary(x => x.Id, x => x.Name);

        Assert.True(sample.All(x => collection.Select(x => x.Name).Contains(x.Value))); // Verify if elements exist in the original collection
        Assert.True(collection.All(x => sample.Values.Contains(x.Name)));               // Verify if all original elements are in the sample
    }
}