using System.Collections;

namespace Kotz.Tests.TestData;

/// <summary>
/// Contains test data for the offset correction tests for <see cref="DateTimeOffsetExtTest"/>.
/// </summary>
internal sealed class OffsetCorrectionTestData : IEnumerable<object[]>
{
    // { Expected Offset, Calculated Offset }
    // Offset is in minutes
    private static readonly object[][] _testData = new object[][]
    {
            new object[] { 0, 0 },
            new object[] { 0, 0.9 },
            new object[] { 180, 180 },
            new object[] { 180, 180.1 },
            new object[] { 23, 23.3333 },
            new object[] { 59, 59.9 },
            new object[] { 123, 123.456789 },
            new object[] { 600, 600.000001 },
            new object[] { 1, 1.000000001 }
    };

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var subArray in _testData)
            yield return subArray;
    }
}