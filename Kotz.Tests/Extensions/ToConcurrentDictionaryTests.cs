using Kotz.Extensions;
using Kotz.Tests.Models;
using Kotz.Tests.TestData;
using Xunit;

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
}