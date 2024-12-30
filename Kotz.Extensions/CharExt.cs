using System.Globalization;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see langword="char"/> static methods.
/// </summary>
public static class CharExt
{
    /// <summary>
    /// Converts the current Unicode code point into a UTF-16 encoded string.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// A string consisting of one char object or a surrogate pair of char
    /// objects equivalent to the code point specified by the UTF-32 parameter.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException" />
    public static string ConvertFromUtf32(this char character)
        => char.ConvertFromUtf32(character);

    /// <summary>
    /// Converts the current numeric Unicode character to a double-precision floating point
    /// number.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>The numeric value if this character represents a number; otherwise, "-1.0".</returns>
    public static double GetNumericValue(this char character)
        => char.GetNumericValue(character);

    /// <summary>
    /// Categorizes the current Unicode character into a group identified by one of the
    /// UnicodeCategory values.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>A UnicodeCategory value that identifies the group of this character.</returns>
    public static UnicodeCategory GetUnicodeCategory(this char character)
        => char.GetUnicodeCategory(character);

    /// <summary>
    /// Returns true if this character is an ASCII character ([ U+0000..U+007F ]).
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an ASCII character, <see langword="false"/>
    /// otherwise.
    /// </returns>
    public static bool IsAscii(this char character)
        => char.IsAscii(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an ASCII digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiDigit(this char character)
        => char.IsAsciiDigit(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII hexadecimal digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an ASCII hexadecimal digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiHexDigit(this char character)
        => char.IsAsciiHexDigit(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII lower-case hexadecimal digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a lower-case hexadecimal digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiHexDigitLower(this char character)
        => char.IsAsciiHexDigitLower(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII upper-case hexadecimal digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a hexadecimal digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiHexDigitUpper(this char character)
        => char.IsAsciiHexDigitUpper(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an ASCII letter,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiLetter(this char character)
        => char.IsAsciiLetter(character);

    /// <summary>
    /// Indicates whether this character is categorized as a lowercase ASCII letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a lowercase ASCII letter,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiLetterLower(this char character)
        => char.IsAsciiLetterLower(character);

    /// <summary>
    /// Indicates whether this character is categorized as an ASCII letter or digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an ASCII letter or digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiLetterOrDigit(this char character)
        => char.IsAsciiLetterOrDigit(character);

    /// <summary>
    /// Indicates whether this character is categorized as an uppercase ASCII letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is an uppercase ASCII letter,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsAsciiLetterUpper(this char character)
        => char.IsAsciiLetterUpper(character);

    /// <summary>
    /// Indicates whether this character is within the specified inclusive range.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <param name="minInclusive">The lower bound, inclusive.</param>
    /// <param name="maxInclusive">The upper bound, inclusive.</param>
    /// <returns>
    /// <see langword="true"/> if this character is within the specified range,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsBetween(this char character, char minInclusive, char maxInclusive)
        => char.IsBetween(character, minInclusive, maxInclusive);

    /// <summary>
    /// Indicates whether this character is categorized as a control character.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a control character,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsControl(this char character)
        => char.IsControl(character);

    /// <summary>
    /// Indicates whether this character is categorized as a decimal digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a decimal digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsDigit(this char character)
        => char.IsDigit(character);

    /// <summary>
    /// Indicates whether this character is a high surrogate.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if the numeric value of this character parameter ranges from
    /// U+D800 through U+DBFF, <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsHighSurrogate(this char character)
        => char.IsHighSurrogate(character);

    /// <summary>
    /// Indicates whether this character is categorized as a letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is a letter, <see langword="false"/> otherwise.</returns>
    public static bool IsLetter(this char character)
        => char.IsLetter(character);

    /// <summary>
    /// Indicates whether this character is categorized as a letter or a decimal digit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a letter or a decimal digit,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsLetterOrDigit(this char character)
        => char.IsLetterOrDigit(character);

    /// <summary>
    /// Indicates whether this character is categorized as a lowercase letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is a lowercase letter, <see langword="false"/> otherwise.</returns>
    public static bool IsLower(this char character)
        => char.IsLower(character);

    /// <summary>
    /// Indicates whether this character is a low surrogate.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if the numeric value of this character ranges from
    /// U+DC00 through U+DFFF, <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsLowSurrogate(this char character)
        => char.IsLowSurrogate(character);

    /// <summary>
    /// Indicates whether this character is categorized as a number.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is a number, <see langword="false"/> otherwise.</returns>
    public static bool IsNumber(this char character)
        => char.IsNumber(character);

    /// <summary>
    /// Indicates whether this character is categorized as a punctuation mark.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a punctuation mark,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsPunctuation(this char character)
        => char.IsPunctuation(character);

    /// <summary>
    /// Indicates whether this character is categorized as a separator character.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is a separator character,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsSeparator(this char character)
        => char.IsSeparator(character);

    /// <summary>
    /// Indicates whether this character has a surrogate code unit.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// <see langword="true"/> if this character is either a high surrogate
    /// or a low surrogate, <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsSurrogate(this char character)
        => char.IsSurrogate(character);

    /// <summary>
    /// Indicates whether this character is categorized as a symbol character.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is a symbol character, <see langword="false"/> otherwise.</returns>
    public static bool IsSymbol(this char character)
        => char.IsSymbol(character);

    /// <summary>
    /// Indicates whether this character is categorized as an uppercase letter.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is an uppercase letter, <see langword="false"/> otherwise.</returns>
    public static bool IsUpper(this char character)
        => char.IsUpper(character);

    /// <summary>
    /// Indicates whether this character is categorized as white space.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns><see langword="true"/> if this character is white space, <see langword="false"/> otherwise.</returns>
    public static bool IsWhiteSpace(this char character)
        => char.IsWhiteSpace(character);

    /// <summary>
    /// Converts the value of this character to its lowercase equivalent.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// The lowercase equivalent of this character, or the same character unchanged,
    /// if the character is already lowercase or not alphabetic.
    /// </returns>
    public static char ToLower(this char character)
        => char.ToLower(character, CultureInfo.CurrentCulture);

    /// <summary>
    /// Converts the value of this character to its lowercase equivalent using the specified 
    /// culture-specific formatting information.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <param name="culture">The culture-specific casing rules.</param>
    /// <returns>
    /// The lowercase equivalent of this character, modified according to culture, or the
    /// unchanged value of the character if it's already lowercase or not alphabetic.
    /// </returns>
    /// <exception cref="ArgumentNullException" />
    public static char ToLower(this char character, CultureInfo culture)
        => char.ToLower(character, culture);

    /// <summary>
    /// Converts the value of this character to its lowercase equivalent using the casing
    /// rules of the invariant culture.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// The lowercase equivalent of this character, or the unchanged value of the character
    /// if it's already lowercase or not alphabetic.
    /// </returns>
    public static char ToLowerInvariant(this char character)
        => char.ToLowerInvariant(character);

    /// <summary>
    /// Converts the value of this character to its uppercase equivalent.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// The uppercase equivalent of this character, or the unchanged value of the character
    /// if it's already uppercase, has no uppercase equivalent, or is not alphabetic.
    /// </returns>
    public static char ToUpper(this char character)
        => char.ToUpper(character, CultureInfo.CurrentCulture);

    /// <summary>
    /// Converts the value of this character to its uppercase equivalent using the specified
    /// culture-specific formatting information.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <param name="culture">The culture-specific casing rules.</param>
    /// <returns>
    /// The uppercase equivalent of this character, modified according to culture, or the
    /// unchanged value of the character if it's already uppercase, has no uppercase
    /// equivalent, or is not alphabetic.
    /// </returns>
    /// <exception cref="ArgumentNullException" />
    public static char ToUpper(this char character, CultureInfo culture)
        => char.ToUpper(character, culture);

    /// <summary>
    /// Converts the value of this character to its uppercase equivalent using the casing
    /// rules of the invariant culture.
    /// </summary>
    /// <param name="character">This character.</param>
    /// <returns>
    /// The uppercase equivalent of this character, or the unchanged value of the character
    /// if it's already uppercase or not alphabetic.
    /// </returns>
    public static char ToUpperInvariant(this char character)
        => char.ToUpperInvariant(character);
}