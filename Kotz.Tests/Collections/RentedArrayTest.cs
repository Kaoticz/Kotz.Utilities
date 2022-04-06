using Kotz.Collections;
using Kotz.Tests.TestData;
using Kotz.Tests.Models;
using Xunit;

namespace Kotz.Tests.Collections;

public sealed class RentedArrayTest
{
    [Theory]
    [MemberData(nameof(MockCollectionTestData.NullCollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void InitializationTest(MockObject[] collection)
    {
        if (collection is null)
        {
            Assert.Throws<ArgumentNullException>(() => new RentedArray<MockObject>(collection!));
            return;
        }

        using var rentedArray = new RentedArray<MockObject>(collection);

        Assert.Equal(collection.Length, rentedArray.Count);

        for (var index = 0; index < collection.Length; index++)
            Assert.Equal(collection[index], rentedArray[index]);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    internal void AddTest(MockObject[] collection)
    {
        using var rentedArray = new RentedArray<MockObject>(collection);
        Assert.Throws<NotSupportedException>(() => rentedArray.Add(collection[0]));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void EnumerationTest(MockObject[] collection)
    {
        var counter = 0;
        using var rentedArray = new RentedArray<MockObject>(collection);

        foreach (var rentedItem in rentedArray)
            Assert.Equal(collection[counter++], rentedItem);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void InsertTest(MockObject[] collection)
    {
        var index = 4;
        var toInsert = new MockObject(666, "inserted");
        using var rentedArray = new RentedArray<MockObject>(collection);

        rentedArray.Insert(index, toInsert);

        Assert.Equal(toInsert, rentedArray[index]);
        Assert.Throws<ArgumentOutOfRangeException>(() => rentedArray.Insert(-1, toInsert));
        Assert.Throws<ArgumentOutOfRangeException>(() => rentedArray.Insert(rentedArray.Count + 1, toInsert));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void RemoveAtTest(MockObject[] collection)
    {
        var index = 4;
        using var rentedArray = new RentedArray<MockObject>(collection);

        rentedArray.RemoveAt(index);

        Assert.Equal(default, rentedArray[index]);
        Assert.Throws<ArgumentOutOfRangeException>(() => rentedArray.RemoveAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => rentedArray.RemoveAt(rentedArray.Count + 1));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.EmptyCollection), MemberType = typeof(MockCollectionTestData))]
    internal void ClearTest(MockObject[] collection)
    {
        using var rentedArray = new RentedArray<MockObject>(collection);
        rentedArray.Clear();

        for (var index = 0; index < rentedArray.Count; index++)
            Assert.Equal(default, rentedArray[index]);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.TrueSubcollection), MemberType = typeof(MockCollectionTestData))]
    internal void ContainsTest(MockObject[] collection, MockObject[] subCollection)
    {
        using var rentedArray = new RentedArray<MockObject>(subCollection);

        foreach (var item in collection)
            Assert.Equal(subCollection.Contains(item), rentedArray.Contains(item));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void RemoveTest(MockObject[] collection)
    {
        var index = 4;
        using var rentedArray = new RentedArray<MockObject>(collection);

        rentedArray.Remove(collection[index]);

        Assert.Equal(default, rentedArray[index]);
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.Collection), MemberType = typeof(MockCollectionTestData))]
    [MemberData(nameof(MockCollectionTestData.CollectionWithNull), MemberType = typeof(MockCollectionTestData))]
    internal void IndexOfTest(MockObject[] collection)
    {
        var index = 4;
        var notFound = new MockObject(31, "A");
        using var rentedArray = new RentedArray<MockObject>(collection);

        Assert.Equal(index, rentedArray.IndexOf(collection[index]));
        Assert.Equal(-1, rentedArray.IndexOf(notFound));
    }

    [Theory]
    [MemberData(nameof(MockCollectionTestData.TrueSubcollection), MemberType = typeof(MockCollectionTestData))]
    internal void TryGetValueTest(MockObject[] collection, MockObject[] subCollection)
    {
        using var rentedArray = new RentedArray<MockObject>(subCollection);

        foreach (var item in collection)
            Assert.Equal(subCollection.Contains(item), rentedArray.TryGetValue(x => x?.Equals(item) is true, out _));

        Assert.True(rentedArray.TryGetValue(4, out _));
        Assert.False(rentedArray.TryGetValue(-1, out _));
        Assert.False(rentedArray.TryGetValue(rentedArray.Count + 1, out _));
        Assert.False(rentedArray.TryGetValue(x => x == default, out _));
    }
}