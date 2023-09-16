namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Theory]
    [InlineData(new[] { 10, 20 }, 'a', 'b')]
    [InlineData(new[] { 10, 20 }, 'a', 'b', 'c')]
    [InlineData(new[] { 10, 20 }, 'a')]
    [InlineData(new[] { 10, 20 })]
    [InlineData(new int[] { }, 'a')]
    [InlineData(new int[] { })]
    internal void ZipTest(int[] firstCollection, params char[] secondCollection)
    {
        var actualResult = firstCollection
            .Zip(secondCollection)
            .ToArray();

        Assert.Equal(Math.Min(firstCollection.Length, secondCollection.Length), actualResult.Length);

        var counter = 0;

        foreach (var (number, letter) in actualResult)
        {
            Assert.Equal(firstCollection[counter], number);
            Assert.Equal(secondCollection[counter], letter);
            counter++;
        }
    }

    [Theory]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b' }, default(int), default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b' }, 5, default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b' }, default(int), 'z')]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b' }, 6, 'w')]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b', 'c' }, default(int), default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b', 'c' }, 5, default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b', 'c' }, default(int), 'z')]
    [InlineData(new[] { 10, 20 }, new[] { 'a', 'b', 'c' }, 6, 'w')]
    [InlineData(new[] { 10, 20 }, new[] { 'a' }, default(int), default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a' }, 5, default(char))]
    [InlineData(new[] { 10, 20 }, new[] { 'a' }, default(int), 'z')]
    [InlineData(new[] { 10, 20 }, new[] { 'a' }, 6, 'w')]
    [InlineData(new[] { 10, 20 }, new char[] { }, default(int), default(char))]
    [InlineData(new int[] { }, new[] { 'a', 'b' }, default(int), default(char))]
    [InlineData(new int[] { }, new char[] { }, default(int), default(char))]
    internal void ZipOrDefaultTest(int[] firstCollection, char[] secondCollection, int firstDefault, char secondDefault)
    {
        var actualResult = firstCollection
            .ZipOrDefault(secondCollection, firstDefault, secondDefault)
            .ToArray();

        Assert.Equal(Math.Max(firstCollection.Length, secondCollection.Length), actualResult.Length);

        for (var counter = 0; counter < actualResult.Length; counter++)
        {
            var (number, letter) = actualResult[counter];
            var expectedNumber = (counter < firstCollection.Length) ? firstCollection[counter] : firstDefault;
            var expectedLetter = (counter < secondCollection.Length) ? secondCollection[counter] : secondDefault;

            Assert.Equal(expectedNumber, number);
            Assert.Equal(expectedLetter, letter);
        }
    }
}