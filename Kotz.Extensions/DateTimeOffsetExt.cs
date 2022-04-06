namespace Kotz.Extensions;

public static class DateTimeOffsetExt
{
    /// <summary>
    /// Gets the beginning of the day of this date.
    /// </summary>
    /// <param name="date">This date time.</param>
    /// <param name="offset">UTC offset.</param>
    /// <returns>This <see cref="DateTimeOffset"/> with its <see cref="DateTimeOffset.TimeOfDay"/> set to zero.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="offset"/> is less than -14 hours or greater than 14 hours.</exception>
    public static DateTimeOffset StartOfDay(this DateTimeOffset date, TimeSpan offset = default)
        => new(date.Year, date.Month, date.Day, 0, 0, 0, 0, TimeSpan.FromMinutes((long)((offset == default && date.Offset != default) ? date.Offset.TotalMinutes : offset.TotalMinutes)));
}