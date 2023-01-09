namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToRentedArrayNullTest(IEnumerable<MockObject> collection)
        => Assert.Throws<ArgumentNullException>(() => collection.ToRentedArray());

    [Theory]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToRentedArrayEmptyTest(IEnumerable<MockObject> collection)
    {
        var result = collection.ToRentedArray();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void ToRentedArrayTest(IEnumerable<MockObject> collection)
    {
        var sample = collection.ToRentedArray();

        Assert.True(sample.All(x => collection.Contains(x)));   // Verify if elements exist in the original collection
        Assert.True(collection.All(x => sample.Contains(x)));   // Verify if all original elements are in the sample
    }
}