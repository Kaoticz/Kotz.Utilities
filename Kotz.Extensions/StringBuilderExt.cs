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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringAndClear(this StringBuilder stringBuilder, int startIndex, int length)
    {
        var result = stringBuilder.ToString(startIndex, length);
        stringBuilder.Clear();

        return result;
    }
}