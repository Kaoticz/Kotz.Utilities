using Kotz.Extensions;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed partial class LinqExtTests
{
    [Fact]
    internal void FillTest()
    {
        var sample = new string[] { "Some strings", "This should create 5 slots" }
            .Select(x => x.Split(' '))
            .NestedFill(string.Empty);

        Assert.Equal(2, sample.Count);

        foreach (var collection in sample)
            Assert.Equal(5, collection.Count);

        // Empty collection
        sample = Array.Empty<string>()
            .Select(x => x.Split(' '))
            .NestedFill(string.Empty);

        Assert.Empty(sample);

        // Null elements
        Assert.Throws<ArgumentNullException>(() => new string[] { null!, "This should create 5 slots" }.Select(x => x?.Split(' ')!).NestedFill(string.Empty));

        // Null input
        string[] nullStrings = null!;
        Assert.Throws<ArgumentNullException>(() => nullStrings.Select(x => x?.Split(' ')!).NestedFill(string.Empty));
        Assert.Throws<ArgumentNullException>(() => Array.Empty<string>().Select(x => x.Split(' ')).NestedFill<string, IEnumerable<string>>(null!));
    }
}