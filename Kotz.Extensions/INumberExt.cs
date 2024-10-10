using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kotz.Extensions;

/// <summary>
/// Provides methods for converting numbers between base 10 and other numeric bases.
/// </summary>
public static class NumberBaseExt
{
    /// <summary>
    /// Converts the string representation of a number in the specified <paramref name="base"/> to an integer of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The integer type to convert the string to.</typeparam>
    /// <param name="number">The string representing the number.</param>
    /// <param name="base">The base of the <paramref name="number"/> representation in the string.</param>
    /// <remarks>Negative numbers from certain bases cannot be converted and will throw an <see cref="InvalidOperationException"/>.</remarks>
    /// <returns>The <paramref name="number"/> converted to an integer of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="number"/> is not representable in <paramref name="base"/>.
    /// Occurs when <paramref name="number"/> is not representable in <typeparamref name="T"/>.
    /// Occurs when <paramref name="number"/> contains characters that are not valid ASCII letters or digits.
    /// </exception>
    /// <exception cref="InvalidOperationException">Occurs when a negative number could not be converted to decimal.</exception>
    /// <exception cref="OverflowException">Occurs when the equivalent of <typeparamref name="T"/>.MaxValue or <typeparamref name="T"/>.MinValue are provided.</exception>
    /// <exception cref="UnreachableException">Occurs when <paramref name="number"/> contains invalid characters for the given base.</exception>
    public static T FromDigits<T>(this string number, int @base) where T : struct, IBinaryInteger<T>
    {
        if (!number.All(char.IsAsciiLetterOrDigit))
            throw new ArgumentException("The string must contain ASCII letters or digits only.", nameof(number));
        
        if (number.Any(x => (char.IsAsciiDigit(x)) ? x >= '0' + @base : (char.IsAsciiLetterUpper(x)) ? x >= 'A' + @base - 10 : x >= 'a' + @base - 10))
            throw new ArgumentException($"The number '{number}' cannot be represented in base {@base}", nameof(@base));

        if (number.Length > FractionalDigits(Unsafe.SizeOf<T>(), @base))
            throw new ArgumentException($"The number '{number}({@base})' cannot be represented as a {nameof(T)} type.", nameof(number));

        if (number.All(x => x is '0'))
            return T.Zero;

        if (@base is 10)
            return T.Parse(number, NumberStyles.Integer, null);

        var positiveValue = FromPositiveDigits<T>(number, @base);

        // I could not get this to work consistently for negative numbers
        // If you have a solution, please submit a pull request!
        return (number.Equals(positiveValue.ToDigits(@base), StringComparison.OrdinalIgnoreCase))
            ? positiveValue
            : throw new InvalidOperationException($"The negative number '{number}' could not be converted from base {@base}.");

        // If the number is negative, subtract from the maximum value representable
        // var maxRepresentableValue = T.CreateTruncating(Math.Pow(@base, number.Length));

        // Calculate the two's complement value (negative)
        // return positiveValue - maxRepresentableValue;
    }

    /// <summary>
    /// Converts the specified <paramref name="number"/> to its representation in the given <paramref name="base"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <paramref name="number"/>.</typeparam>
    /// <param name="number">The number to convert.</param>
    /// <param name="base">The base for the conversion.</param>
    /// <returns>A string representing the <paramref name="number"/> in the specified <paramref name="base"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="base"/> is less than 2 or greater than 36.</exception>
    /// <exception cref="InvalidOperationException">Occurs when <paramref name="base"/> is 10 and <typeparamref name="T"/> returns <see langword="null"/> from <see cref="object.ToString()"/>>.</exception>
    /// <exception cref="OverflowException">Occurs when <typeparamref name="T"/>.MinValue is provided.</exception>
    public static string ToDigits<T>(this T number, int @base) where T : struct, IBinaryInteger<T>
    {
        if (@base is 10)
            return number.ToString() ?? throw new InvalidOperationException($"Type {nameof(T)} returned null for ToString().");

        if (T.IsZero(number))
            return "0";

        // Representations can span from 0-9 (base 2 to 10) to A-Z (base 11 to 36).
        if (@base is < 2 or > 36)
            throw new ArgumentOutOfRangeException(nameof(@base), @base, "Base must be between 2 and 36.");

        Span<char> buffer = stackalloc char[FractionalDigits(Unsafe.SizeOf<T>(), @base)];

        var result = (int.IsPow2(@base))
            ? ToArbitraryBaseOptimized(number, @base, buffer)
            : ToArbitraryBase(number, @base, buffer);

        return (result.Length > 1)
            ? result.TrimStart('0').ToString()
            : result.ToString();
    }

    /// <summary>
    /// Converts a <paramref name="number"/> to the specified base, which must be a power of two.
    /// If <paramref name="number"/> is negative, it will be represented using Two's Complement.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <paramref name="number"/>.</typeparam>
    /// <param name="number">The number to convert.</param>
    /// <param name="base">The base (power of two) for the conversion.</param>
    /// <param name="buffer">The buffer that will store the digits of the converted number.</param>
    /// <returns>A span containing the digits of <paramref name="number"/> in the specified <paramref name="base"/>.</returns>
    private static Span<char> ToArbitraryBaseOptimized<T>(T number, int @base, Span<char> buffer) where T : IBinaryInteger<T>
    {
        var truncatedBase = T.CreateTruncating(@base);
        var counter = buffer.Length;
        var bits = int.Log2(@base);

        do
        {
            var nextChar = '0' + uint.CreateTruncating((number & (truncatedBase - T.One)));
            buffer[--counter] = (char)((nextChar > '9') ? nextChar + 7 : nextChar);
            number >>>= bits;
        } while (!T.IsZero(number));

        return buffer[counter..];
    }

    /// <summary>
    /// Converts a <paramref name="number"/> to the specified <paramref name="base"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <paramref name="number"/>.</typeparam>
    /// <param name="number">The number to convert.</param>
    /// <param name="base">The base for the conversion.</param>
    /// <param name="buffer">The buffer that will store the digits of the converted number.</param>
    /// <returns>A span containing the digits of <paramref name="number"/> in the specified <paramref name="base"/>.</returns>
    /// <exception cref="OverflowException">Occurs when <typeparamref name="T"/>.MinValue is provided.</exception>
    private static Span<char> ToArbitraryBase<T>(T number, int @base, Span<char> buffer) where T : IBinaryInteger<T>
    {
        if (T.IsNegative(number))
            return ToTwosComplement(number, @base, buffer);

        var truncatedBase = T.CreateTruncating(@base);
        var counter = buffer.Length;

        do
        {
            var nextChar = '0' + uint.CreateTruncating(number % truncatedBase);
            buffer[--counter] = (char)((nextChar > '9') ? nextChar + 7 : nextChar);
            number /= @truncatedBase;

        } while (!T.IsZero(number));

        return buffer[counter..];
    }

    /// <summary>
    /// Converts a negative <paramref name="number"/> to the specified base using Two's Complement representation.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <paramref name="number"/>.</typeparam>
    /// <param name="number">The negative number to convert.</param>
    /// <param name="base">The base (power of two) for the conversion.</param>
    /// <param name="buffer">The buffer that will store the digits of the converted number.</param>
    /// <returns>
    /// A span containing the digits of <paramref name="number"/> in the specified <paramref name="base"/>,
    /// represented using Two's Complement.
    /// </returns>
    /// <exception cref="OverflowException">Occurs when <typeparamref name="T"/>.MinValue is provided.</exception>
    private static Span<char> ToTwosComplement<T>(T number, int @base, Span<char> buffer) where T : IBinaryInteger<T>
    {
        var truncatedNumber = T.Abs(number);
        var truncatedBase = T.CreateTruncating(@base);

        // Convert number to @base while complementing it.
        for (var index = buffer.Length - 1; index >= 0; index--)
        {
            var nextChar = '0' + uint.CreateTruncating((truncatedBase - T.One) - (truncatedNumber % truncatedBase));
            buffer[index] = (char)((nextChar > '9') ? nextChar + 7 : nextChar);
            truncatedNumber /= truncatedBase;
        }

        // Add 1 to the result in @base;
        for (var index = buffer.Length - 1; index >= 0; index--)
        {
            if (++buffer[index] != @base + '0')
                break;
                
            buffer[index] = '0';
        }

        return buffer;
    }

    /// <summary>
    /// Converts a positive <paramref name="number"/> from the specified <see langword="base"/> to a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <paramref name="number"/>.</typeparam>
    /// <param name="number">The negative number to convert.</param>
    /// <param name="base">The base for the conversion.</param>
    /// <returns>The decimal representation of the <paramref name="number"/>.</returns>
    /// <exception cref="UnreachableException">Occurs when <paramref name="number"/> contains invalid digits.</exception>
    private static T FromPositiveDigits<T>(string number, int @base) where T : IBinaryInteger<T>
    {
        var result = T.Zero;
        var truncatedBase = T.CreateTruncating(@base);
        var digitOffset = T.CreateTruncating('0');
        var upperOffset = T.CreateTruncating('A' - 10);
        var lowerOffset = T.CreateTruncating('a' - 10);

        foreach (var digit in number)
        {
            var offset = digit switch
            {
                >= '0' and <= '9' => digitOffset,
                >= 'A' and <= 'Z' => upperOffset,
                >= 'a' and <= 'z' => lowerOffset,
                _ => throw new UnreachableException($"Invalid character: {digit}")
            };

            result = (result * truncatedBase) + T.CreateTruncating(digit) - offset;
        }

        return result;
    }

    /// <summary>
    /// Calculates the maximum amount of fractional digits a number can have for
    /// a given base and type size.
    /// </summary>
    /// <param name="typeSize">The size of the type, in bytes.</param>
    /// <param name="targetBase">The base to calculate the digits for.</param>
    /// <returns>The amount of fractional digits in the number.</returns>
    private static int FractionalDigits(int typeSize, int targetBase)
        => (int)Math.Ceiling((typeSize * 8 * Math.Log(2)) / Math.Log(targetBase));
}