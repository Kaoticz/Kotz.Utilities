using Kotz.Tests.Models;

namespace Kotz.Tests.TestData;

/// <summary>
/// Contains data for tests that require collections.
/// </summary>
internal sealed class MockCollectionTestData
{
    private static readonly MockObject[] _normalCollection = Enumerable.Range(0, 10)
        .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
        .ToArray();

    private static readonly MockObject?[] _collectionWithNull = Enumerable.Range(0, 10)
        .Select(x => (x is 4 or 7) ? null : new MockObject(x, char.ConvertFromUtf32(65 + x)))
        .ToArray();

    private static readonly MockObject[] _trueSubcollection = Enumerable.Range(0, 10)
        .Where(x => x % 2 is 0)
        .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
        .ToArray();

    private static readonly MockObject[] _falseSubcollection = Enumerable.Range(0, 12)
        .Where(x => x % 2 is 0)
        .Select(x => new MockObject(x, char.ConvertFromUtf32(65 + x)))
        .ToArray();

    /// <summary>
    /// Represents a null collection.
    /// </summary>
    /// <value><see langword="null"/></value>
    public static IEnumerable<object?[]> NullCollection { get; } = new object?[][] { new object?[] { null } };

    /// <summary>
    /// Represents an empty collection.
    /// </summary>
    /// <value><see cref="MockObject"/>[0]</value>
    public static IEnumerable<object[]> EmptyCollection { get; } = new object[][] { new object[] { Array.Empty<MockObject>() } };

    /// <summary>
    /// Represents a collection where some of its elements are <see langword="null"/>.
    /// </summary>
    /// <value><see cref="MockObject"/>[10]</value>
    public static IEnumerable<object[]> CollectionWithNull { get; } = new object[][] { new object[] { _collectionWithNull } };

    /// <summary>
    /// Represents a collection with no bad data.
    /// </summary>
    /// <value><see cref="MockObject"/>[10]</value>
    public static IEnumerable<object[]> Collection { get; } = new object[][] { new object[] { _normalCollection } };

    /// <summary>
    /// Returns a collection and a subcollection of it.
    /// </summary>
    /// <value>{ <see cref="MockObject"/>[10], <see cref="MockObject"/>[5] }</value>
    public static IEnumerable<object[]> TrueSubcollection { get; } = new object[][] { new object[] { _normalCollection, _trueSubcollection } };

    /// <summary>
    /// Returns a collection and a false subcollection of it.
    /// </summary>
    /// <value>{ <see cref="MockObject"/>[10], <see cref="MockObject"/>[6] }</value>
    public static IEnumerable<object[]> FalseSubcollection { get; } = new object[][] { new object[] { _normalCollection, _falseSubcollection } };
}