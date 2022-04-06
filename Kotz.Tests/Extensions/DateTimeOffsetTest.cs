using Kotz.Extensions;
using Kotz.Tests.TestData;
using Xunit;

namespace Kotz.Tests.Extensions;

public sealed class DateTimeOffsetExtTest
{
    [Theory] // Offset is in minutes
    [ClassData(typeof(OffsetCorrectionTestData))]
    internal void StartOfDayTest(int goodOffset, double badOffset)
    {
        var today = DateTimeOffset.UtcNow;

        // Offsets
        var inputOffset = TimeSpan.FromMinutes(badOffset);
        var correctedOffset = TimeSpan.FromMinutes(goodOffset);

        // Test input
        Assert.Equal(new(today.Year, today.Month, today.Day, 0, 0, 0, 0, correctedOffset), today.StartOfDay(inputOffset));
    }

    [Theory] // Offset is in minutes
    [ClassData(typeof(OffsetCorrectionTestData))]
    internal void OffsetCorrectionTest(int expected, double actual)
        => Assert.Equal(TimeSpan.FromMinutes(expected), DateTimeOffset.UtcNow.StartOfDay(TimeSpan.FromMinutes(actual)).Offset);
}