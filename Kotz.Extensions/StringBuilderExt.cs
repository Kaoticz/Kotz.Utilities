using System.Runtime.CompilerServices;
using System.Text;

namespace Kotz.Extensions;

public static class StringBuilderExt
{
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
    public static string ToStringAndClear(this StringBuilder stringBuilder, in int startIndex, in int length)
    {
        var result = stringBuilder.ToString(startIndex, length);
        stringBuilder.Clear();

        return result;
    }
}
