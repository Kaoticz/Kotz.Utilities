using Kotz.Collections.Extensions;
using Kotz.Tests.Models;
using Kotz.Tests.TestData;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToRingBufferNullTest(IEnumerable<MockObject> collection)
    {
        Assert.Throws<ArgumentNullException>(() => collection.ToRingBuffer());
        Assert.Throws<ArgumentNullException>(() => collection.ToRingBuffer(10));
        Assert.Throws<ArgumentOutOfRangeException>(() => collection.ToRingBuffer(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => collection.ToRingBuffer(-10));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ToRingBufferEmptyTest(IEnumerable<MockObject> collection)
    {
        var result = collection.ToRingBuffer();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void ToRingBufferTest(IEnumerable<MockObject> collection)
    {
        var sample = collection.ToRingBuffer();

        Assert.True(sample.All(x => collection.Contains(x)));   // Verify if elements exist in the original collection
        Assert.True(collection.All(x => sample.Contains(x)));   // Verify if all original elements are in the sample
    }
}