using System.Runtime.CompilerServices;
using System.Text;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="StringBuilder"/>.
/// </summary>
public static class StringBuilderExt
{
    /// <summary>
    /// Replaces, within a substring of this instance, all occurrences of <paramref name="oldText"/> with <paramref name="newText"/>,
    /// even if <paramref name="newText"/> is a substring of <paramref name="oldText"/>.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="oldText">The string to replace.</param>
    /// <param name="newText">The string that replaces <paramref name="oldText"/>.</param>
    /// <returns>A reference to this instance with all instances of <paramref name="oldText"/> replaced by <paramref name="newText"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="newText"/> is longer than <paramref name="oldText"/> and contains parts of <paramref name="oldText"/>.
    /// </exception>
    public static StringBuilder ReplaceAll(this StringBuilder stringBuilder, string oldText, string newText)
        => stringBuilder.ReplaceAll(oldText, newText, 0, stringBuilder.Length);

    /// <summary>
    /// Replaces, within a substring of this instance, all occurrences of <paramref name="oldText"/> with <paramref name="newText"/>,
    /// even if <paramref name="newText"/> is a substring of <paramref name="oldText"/>.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="oldText">The string to replace.</param>
    /// <param name="newText">The string that replaces <paramref name="oldText"/>.</param>
    /// <param name="startIndex">The position in this instance where the <paramref name="oldText"/> begins.</param>
    /// <param name="count">The length of the substring.</param>
    /// <returns>
    /// A reference to this instance with all instances of <paramref name="oldText"/> replaced by <paramref name="newText"/> in the
    /// range from <paramref name="startIndex"/> to <paramref name="startIndex"/> + <paramref name="count"/> - 1.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="newText"/> is longer than <paramref name="oldText"/> and contains parts of <paramref name="oldText"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="count"/> is equal or less than 0.</exception>
    public static StringBuilder ReplaceAll(this StringBuilder stringBuilder, string oldText, string newText, int startIndex, int count)
    {
        if (newText.Length > oldText.Length && newText.Contains(oldText))
            throw new ArgumentException($"{nameof(newText)} must not contain {nameof(oldText)} while having greater length than {nameof(oldText)}.", nameof(newText));
        else if (stringBuilder.Length is 0)
            return stringBuilder;

        int length;

        do
        {
            length = stringBuilder.Length;
            stringBuilder.Replace(oldText, newText, startIndex, Math.Min(count, length - startIndex));
        } while (length != stringBuilder.Length);

        return stringBuilder;
    }

    /// <summary>
    /// Checks if <paramref name="text"/> is present in this string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="text">The text to check for.</param>
    /// <returns><see langword="true"/> if <paramref name="text"/> is present, <see langword="false"/> otherwise.</returns>
    public static bool Contains(this StringBuilder stringBuilder, ReadOnlySpan<char> text)
    {
        if (stringBuilder.Length is 0 || text.Length is 0)
            return false;

        var textIndex = 0;

        foreach (var chunk in stringBuilder.GetChunks())
        {
            for (var chunkIndex = 0; chunkIndex < chunk.Span.Length; chunkIndex++)
            {
                textIndex = (chunk.Span[chunkIndex] == text[textIndex])
                    ? textIndex + 1
                    : 0;

                if (textIndex == text.Length)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes the specified text chunks from this string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="chunksToRemove">The text chunks to be removed.</param>
    /// <returns>This string builder with the text chunks removed.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="chunksToRemove"/> is <see langword="null"/>.</exception>
    public static StringBuilder Remove(this StringBuilder stringBuilder, params string[] chunksToRemove)
        => Remove(stringBuilder, chunksToRemove.AsEnumerable());

    /// <summary>
    /// Removes the specified text chunks from this string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="chunksToRemove">The text chunks to be removed.</param>
    /// <returns>This string builder with the text chunks removed.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="chunksToRemove"/> is <see langword="null"/>.</exception>
    public static StringBuilder Remove(this StringBuilder stringBuilder, IEnumerable<string> chunksToRemove)
    {
        ArgumentNullException.ThrowIfNull(chunksToRemove);

        foreach (var textChunk in chunksToRemove.Where(x => !string.IsNullOrEmpty(x)))
            stringBuilder.Replace(textChunk, string.Empty);

        return stringBuilder;
    }

    /// <summary>
    /// Removes the character at the specified <paramref name="index"/> from this string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="index">The index of the character to be removed.</param>
    /// <returns>A reference to this instance with the character removed.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the index is less than zero or  greater than the length of the string builder.</exception>
    public static StringBuilder RemoveAt(this StringBuilder stringBuilder, int index)
    {
        return (index >= stringBuilder.Length)
            ? throw new ArgumentOutOfRangeException(nameof(index), index, "Index cannot be less than zero or greater than the length of the StringBuilder.")
            : stringBuilder.Remove(index, 1);
    }

    /// <summary>
    /// Converts the value of this instance to a <see langword="string"/>, then clears its buffer.
    /// </summary>
    /// <param name="stringBuilder">This builder.</param>
    /// <returns>A string whose value is the same as this instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringAndClear(this StringBuilder stringBuilder)
    {
        var result = stringBuilder.ToString();
        stringBuilder.Clear();

        return result;
    }

    /// <summary>
    /// Converts the value of this instance to a <see langword="string"/>, then clears its buffer.
    /// </summary>
    /// <param name="stringBuilder">This builder.</param>
    /// <param name="startIndex">The starting position of the substring in this instance.</param>
    /// <param name="length">The length of the substring.</param>
    /// <returns>A string whose value is the same as the specified substring of this instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the specified range is invalid.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringAndClear(this StringBuilder stringBuilder, int startIndex, int length)
    {
        var result = stringBuilder.ToString(startIndex, length);
        stringBuilder.Clear();

        return result;
    }

    /// <summary>
    /// Removes all leading and trailing instances of a character from the current string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="trimChar">A Unicode character to remove.</param>
    /// <returns>This string builder with all leading and trailing instances of <paramref name="trimChar"/> removed.</returns>
    public static StringBuilder Trim(this StringBuilder stringBuilder, char trimChar = ' ')
        => stringBuilder.TrimStart(trimChar).TrimEnd(trimChar);

    /// <summary>
    /// Removes all trailing instances of a character from the current string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="trimChar">A Unicode character to remove.</param>
    /// <returns>This string builder with all trailing instances of <paramref name="trimChar"/> removed.</returns>
    public static StringBuilder TrimEnd(this StringBuilder stringBuilder, char trimChar = ' ')
    {
        if (stringBuilder.Length is 0)
            return stringBuilder;

        var counter = 0;

        for (var index = stringBuilder.Length - 1; index > 0 && stringBuilder[index] == trimChar; index--)
            counter++;

        if (counter > 0)
            stringBuilder.Remove(stringBuilder.Length - counter, counter);

        return stringBuilder;
    }

    /// <summary>
    /// Removes all leading instances of a character from the current string builder.
    /// </summary>
    /// <param name="stringBuilder">This string builder.</param>
    /// <param name="trimChar">A Unicode character to remove.</param>
    /// <returns>This string builder with all leading instances of <paramref name="trimChar"/> removed.</returns>
    public static StringBuilder TrimStart(this StringBuilder stringBuilder, char trimChar = ' ')
    {
        if (stringBuilder.Length is 0)
            return stringBuilder;

        var counter = 0;

        for (var index = 0; index < stringBuilder.Length - 1 && stringBuilder[index] == trimChar; index++)
            counter++;

        if (counter > 0)
            stringBuilder.Remove(0, counter);

        return stringBuilder;
    }
}