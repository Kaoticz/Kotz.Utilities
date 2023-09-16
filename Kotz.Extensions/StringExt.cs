using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/>.
/// </summary>
public static class StringExt
{
    /// <summary>
    /// Gets the amount of occurences of a given character in this string.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="target">The character to check for.</param>
    /// <returns>The amount of occurences of <paramref name="target"/> in this string.</returns>
    public static int Occurrences(this string text, char target)
        => ReadOnlySpanCharExt.Occurrences(text, target);

    /// <summary>
    /// Returns a string with all digits present in this string.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>A string with all digits of this string, <see cref="string.Empty"/> if none is found.</returns>
    public static string GetDigits(this string text)
        => ReadOnlySpanCharExt.GetDigits(text);

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
    /// <seealso cref="LastOccurrenceOf(string, char, int)"/>
    public static int FirstOccurrenceOf(this string text, char character, int occurrence = 0)
        => ReadOnlySpanCharExt.FirstOccurrenceOf(text, character, occurrence);

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
    /// <seealso cref="FirstOccurrenceOf(string, char, int)"/>
    public static int LastOccurrenceOf(this string text, char character, int occurrence = 0)
        => ReadOnlySpanCharExt.LastOccurrenceOf(text, character, occurrence);

    /// <summary>
    /// Checks if this string and <paramref name="sample"/> contain the same first word.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="sample">The string to compare to.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <returns><see langword="true"/> if both strings contain the same first word, <see langword="false"/> otherwise.</returns>
    public static bool HasFirstWordOf(this string text, string sample, StringComparison comparisonType = StringComparison.Ordinal)
        => ReadOnlySpanCharExt.HasFirstWordOf(text, sample, comparisonType);

    /// <summary>
    /// Truncates the string to the maximum specified length.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="maxLength">The maximum length the string should have.</param>
    /// <returns>This string with length equal to or lower than <paramref name="maxLength"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="maxLength"/> is less than zero.</exception>
    [return: NotNullIfNotNull("text")]
    public static string? MaxLength(this string? text, int maxLength)
        => text?[..Math.Min(text.Length, maxLength)];

    /// <summary>
    /// Truncates the string to the maximum specified length.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="maxLength">The maximum length the string should have.</param>
    /// <param name="append">The string to be appended to the end of the truncated string.</param>
    /// <remarks>The <paramref name="append"/> only gets added to the truncated string if this string exceeds <paramref name="maxLength"/> in length.</remarks>
    /// <returns>This string with length equal to or lower than <paramref name="maxLength"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="maxLength"/> is less than zero.</exception>
    [return: NotNullIfNotNull("text")]
    public static string? MaxLength(this string? text, int maxLength, string append)
        => (text is null || text.Length <= maxLength)
            ? text
            : (text.MaxLength(Math.Max(0, maxLength - append.Length)) + append)[..maxLength];

    /// <summary>
    /// Converts a string to the "Title Case" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="cultureInfo">The culture info to be used. Defaults to <see cref="CultureInfo.CurrentCulture"/>.</param>
    /// <returns>This <see cref="string"/> converted to Title Case.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? ToTitleCase(this string? text, CultureInfo? cultureInfo = default)
    {
        return (string.IsNullOrWhiteSpace(text))
            ? text
            : (cultureInfo ?? CultureInfo.CurrentCulture).TextInfo.ToTitleCase(text);
    }

    /// <summary>
    /// Converts a string to the "snake_case" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="joinSpaces"><see langword="true"/> to , <see langword="false"/></param>
    /// <returns>This <see cref="string"/> converted to snake_case.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? ToSnakeCase(this string? text, bool joinSpaces = false)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var textSpan = text.AsSpan();
        var buffer = new StringBuilder();

        for (var index = 0; index < textSpan.Length; index++)
        {
            var chainLength = UpperChainLength(textSpan, index);

            if (chainLength <= 0)
            {
                buffer.Append(char.ToLowerInvariant(textSpan[index]));
                continue;
            }

            buffer.Append('_');

            foreach (var upperLetter in textSpan.Slice(index, chainLength))
                buffer.Append(char.ToLowerInvariant(upperLetter));

            index += chainLength - 1;
        }

        if (buffer[0] is '_')
            buffer.Remove(0, 1);

        buffer.Replace(" _", " ")
            .Replace("_ ", "_");

        if (joinSpaces)
            buffer.Replace(' ', '_');

        buffer.ReplaceAll("__", "_");

        return buffer.ToStringAndClear();
    }

    /// <summary>
    /// Get the length of the longest string of this collection.
    /// </summary>
    /// <param name="collection">This collection of strings.</param>
    /// <returns>The length of the longest element.</returns>
    public static int MaxElementLength(this IEnumerable<string> collection)
    {
        var max = 0;

        foreach (var element in collection)
            max = Math.Max(max, element.Length);

        return max;
    }

    /// <summary>
    /// Checks whether this string is equal to any of the strings provided in <paramref name="samples"/>.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <param name="samples">The strings to compare to.</param>
    /// <returns><see langword="true"/> if this string equals one of the strings in <paramref name="samples"/>, <see langword="false"/> otherwise.</returns>
    public static bool Equals(this string text, StringComparison comparisonType, params string[] samples)
    {
        foreach (var sample in samples)
        {
            if (sample.Equals(text, comparisonType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if at least one entry in this collection matches the specified string and returns the match, if it exists.
    /// </summary>
    /// <param name="collection">This string collection.</param>
    /// <param name="target">The string to be compared with.</param>
    /// <param name="comparisonType">The comparison rules.</param>
    /// <param name="match">The resulting match in the collection or <see langword="null"/> if none was found.</param>
    /// <returns><see langword="true"/> if there was one matching entry, <see langword="false"/> otherwise.</returns>
    public static bool Equals(this IEnumerable<string> collection, string target, StringComparison comparisonType, [MaybeNullWhen(false)] out string? match)
    {
        foreach (var word in collection)
        {
            if (word.Equals(target, comparisonType))
            {
                match = word;
                return true;
            }
        }

        match = default;
        return false;
    }

    /// <summary>
    /// Checks if this string occurs within at least one of the entries in the specified collection.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="collection">The collection to compare to.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <returns><see langword="true"/> if a match occurred, <see langword="false"/> otherwise.</returns>
    public static bool Contains(this string text, IEnumerable<string> collection, StringComparison comparisonType = StringComparison.Ordinal)
    {
        foreach (var word in collection)
        {
            if (text.Contains(word, comparisonType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the end of this string matches any string stored in <paramref name="collection"/> when compared using the specified comparison option.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="collection">The collection to compare to.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <returns><see langword="true"/> if a match occurred, <see langword="false"/> otherwise.</returns>
    public static bool EndsWith(this string text, IEnumerable<string> collection, StringComparison comparisonType = StringComparison.Ordinal)
    {
        foreach (var element in collection)
        {
            if (text.EndsWith(element, comparisonType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the beginning of this string matches any string stored in <paramref name="collection"/> when compared using the specified comparison option.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="collection">The collection to compare to.</param>
    /// <param name="comparisonType">The type of string comparison to be used.</param>
    /// <returns><see langword="true"/> if a match occurred, <see langword="false"/> otherwise.</returns>
    public static bool StartsWith(this string text, IEnumerable<string> collection, StringComparison comparisonType = StringComparison.Ordinal)
    {
        foreach (var element in collection)
        {
            if (text.StartsWith(element, comparisonType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the length of an "ALL CAPS" substring in the specified span.
    /// </summary>
    /// <param name="text">The span to check.</param>
    /// <param name="startIndex">The index where the substring starts.</param>
    /// <returns>The length of the substring.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is greater than the length of the span.</exception>
    private static int UpperChainLength(ReadOnlySpan<char> text, int startIndex)
    {
        if (startIndex > text.Length - 1)
            throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, "Start index cannot be greater than length of the span.");
        else if (text.Length is 0)
            return 0;
        else if (startIndex < 0)
            startIndex = 0;

        var result = 0;

        for (var count = startIndex; count < text.Length; count++)
        {
            if (!char.IsLetter(text[count]) || char.IsLower(text[count]))
                break;

            result++;
        }

        return result;
    }
}