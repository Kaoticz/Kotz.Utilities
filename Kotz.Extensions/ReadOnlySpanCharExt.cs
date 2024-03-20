using System.Text;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see langword="ReadOnlySpan&lt;char&gt;"/>.
/// </summary>
public static class ReadOnlySpanCharExt
{
    /// <summary>
    /// Gets the amount of occurences of a given character in this string.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="target">The character to check for.</param>
    /// <returns>The amount of occurences of <paramref name="target"/> in this string.</returns>
    public static int Occurrences(this ReadOnlySpan<char> text, char target)
    {
        var counter = 0;

        foreach (var letter in text)
        {
            if (letter == target)
                counter++;
        }

        return counter;
    }

    /// <summary>
    /// Returns a string with all digits present in this string.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>A string with all digits of this string, <see cref="string.Empty"/> if none is found.</returns>
    public static string GetDigits(this ReadOnlySpan<char> text)
    {
        var result = new StringBuilder();

        foreach (var character in text)
        {
            if (char.IsDigit(character))
                result.Append(character);
        }

        return result.ToStringAndClear();
    }

    /// <summary>
    /// Returns the "Nth" index of the specified character.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="character">The character to get the index from.</param>
    /// <param name="occurrence">Defines how many occurrences should be skipped, starting from 0 (first match).</param>
    /// <returns>The index of the specified character or -1 if it was not found.</returns>
    /// <example>This returns 2: <code>"hello".FirstOccurrenceOf('l', 0)</code></example>
    /// <example>This returns 3: <code>"hello".FirstOccurrenceOf('l', 1)</code></example>
    /// <example>This returns -1: <code>"hello".FirstOccurrenceOf('l', 2)</code></example>
    /// <seealso cref="LastOccurrenceOf(ReadOnlySpan{char}, char, int)"/>
    public static int FirstOccurrenceOf(this ReadOnlySpan<char> text, char character, int occurrence = 0)
    {
        if (occurrence < 0)
            occurrence = 0;

        int counter = -1, result = -1;

        for (var index = 0; index < text.Length - 1; index++)
        {
            if (text[index].Equals(character) && ++counter == occurrence)
            {
                result = index;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns the last "Nth" index of the specified character.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="character">The character to get the index from.</param>
    /// <param name="occurrence">Defines how many occurrences should be skipped, starting from 0 (first match).</param>
    /// <returns>The last index of the specified character or -1 if it was not found.</returns>
    /// <example>This returns 3: <code>"hello".LastOccurrenceOf('l', 0)</code></example>
    /// <example>This returns 2: <code>"hello".LastOccurrenceOf('l', 1)</code></example>
    /// <example>This returns -1: <code>"hello".LastOccurrenceOf('l', 2)</code></example>
    /// <seealso cref="FirstOccurrenceOf(ReadOnlySpan{char}, char, int)"/>
    public static int LastOccurrenceOf(this ReadOnlySpan<char> text, char character, int occurrence = 0)
    {
        if (occurrence < 0)
            occurrence = 0;

        int counter = -1, result = -1;

        for (var index = text.Length - 1; index >= 0; index--)
        {
            if (text[index].Equals(character) && ++counter == occurrence)
            {
                result = index;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Checks if this string and <paramref name="sample"/> contain the same first word.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="sample">The string to compare to.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <returns><see langword="true"/> if both strings contain the same first word, <see langword="false"/> otherwise.</returns>
    public static bool HasFirstWordOf(this ReadOnlySpan<char> text, ReadOnlySpan<char> sample, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var textIndex = text.IndexOf(" ", StringComparison.InvariantCulture) - 1;
        var sampleIndex = sample.IndexOf(" ", StringComparison.InvariantCulture) - 1;

        if (textIndex is -2)
            textIndex = text.Length - 1;

        if (sampleIndex is -2)
            sampleIndex = sample.Length - 1;

        var firstTextWord = text[..textIndex];
        var firstSampleWord = sample[..sampleIndex];

        return firstTextWord.Equals(firstSampleWord, comparisonType);
    }

    /// <summary>
    /// Truncates the string to the maximum specified length.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="maxLength">The maximum length the string should have.</param>
    /// <returns>This string with length equal to or lower than <paramref name="maxLength"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="maxLength"/> is less than zero.</exception>
    public static ReadOnlySpan<char> MaxLength(this ReadOnlySpan<char> text, int maxLength)
        => text[..Math.Min(text.Length, maxLength)];
}