using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/> related to case formatting.
/// </summary>
public static class StringCaseExt
{
    /// <summary>
    /// Converts this string to the "Title Case" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <param name="cultureInfo">The culture info to be used. Defaults to <see cref="CultureInfo.CurrentCulture"/>.</param>
    /// <returns>This <see cref="string"/> converted to Title Case.</returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? ToTitleCase(this string? text, CultureInfo? cultureInfo = default)
    {
        return (string.IsNullOrWhiteSpace(text))
            ? text
            : (cultureInfo ?? CultureInfo.CurrentCulture).TextInfo.ToTitleCase(text);
    }

    /// <summary>
    /// Converts this string to the "PascalCase" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>This <see cref="string"/> converted to PascalCase.</returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? ToPascalCase(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var textSpan = text.AsSpan();
        var result = new StringBuilder(text.Length);
        var isNewWord = true;

        for (var index = 0; index < textSpan.Length; index++)
        {
            var currentChar = textSpan[index];

            if (!char.IsLetterOrDigit(currentChar))
            {
                isNewWord = true;
                continue;
            }

            if (isNewWord)
            {
                result.Append(char.ToUpperInvariant(currentChar));
                isNewWord = false;
            }
            else
            {
                result.Append(
                    (index < text.Length - 1 && char.IsUpper(currentChar) && char.IsLower(text[index + 1]))
                        ? currentChar
                        : char.ToLowerInvariant(currentChar)
                );
            }

            isNewWord &= index < text.Length - 1 && char.IsLower(text[index]) && char.IsUpper(text[index + 1]);
        }

        return result.ToStringAndClear();
    }

    /// <summary>
    /// Converts this string to the "snake_case" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>This <see cref="string"/> converted to snake_case.</returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? ToSnakeCase(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var textSpan = text.AsSpan();
        var buffer = new StringBuilder(Math.Max(text.Length + 5, 16));

        for (var index = 0; index < textSpan.Length; index++)
        {
            var chainLength = UpperChainLength(textSpan, index);

            if (chainLength <= 0)
            {
                buffer.Append(
                    (char.IsLetterOrDigit(textSpan[index]))
                        ? char.ToLowerInvariant(textSpan[index])
                        : '_'
                );

                continue;
            }

            buffer.Append('_');

            foreach (var upperLetter in textSpan.Slice(index, chainLength))
                buffer.Append(char.ToLowerInvariant(upperLetter));

            index += chainLength - 1;
        }

        buffer.Trim('_')
            .Replace(" _", " ")
            .Replace("_ ", "_")
            .Replace(' ', '_');

        buffer.ReplaceAll("__", "_");

        return buffer.ToStringAndClear();
    }

    /// <summary>
    /// Converts this string to the "camelCase" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>This <see cref="string"/> converted to camelCase.</returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? ToCamelCase(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = (text.All(x => char.IsUpper(x) || !char.IsLetterOrDigit(x)))
            ? text.ToLowerInvariant()
            : text;

        var result = new StringBuilder(text.Length);
        var setToUpper = false;

        foreach (var character in text.AsSpan())
        {
            if (!char.IsLetterOrDigit(character))
                setToUpper = true;
            else
            {
                result.Append(
                    (setToUpper)
                        ? char.ToUpperInvariant(character)
                        : character
                );

                setToUpper = false;
            }
        }

        if (result.Length > 0)
            result[0] = char.ToLowerInvariant(result[0]);

        return result.ToStringAndClear();
    }

    /// <summary>
    /// Converts this string to the "kebab-case" format.
    /// </summary>
    /// <param name="text">This string.</param>
    /// <returns>This <see cref="string"/> converted to kebab-case.</returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? ToKebabCase(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var result = new StringBuilder(Math.Max(text.Length + 5, 16));
        var textSpan = text.AsSpan();
        var previousCharIsSeparator = true;

        for (var index = 0; index < textSpan.Length; index++)
        {
            var currentChar = textSpan[index];

            if (char.IsUpper(currentChar) || char.IsDigit(currentChar))
            {
                // Add a hyphen if the previous character is not a separator and the current
                // character is preceded by a lowercase letter or followed by a lowercase letter
                if (!previousCharIsSeparator && (index > 0 && (char.IsLower(textSpan[index - 1]) || (index < textSpan.Length - 1 && char.IsLower(textSpan[index + 1])))))
                    result.Append('-');

                result.Append(char.ToLowerInvariant(currentChar));
                previousCharIsSeparator = false;
            }
            else if (char.IsLower(currentChar))
            {
                result.Append(currentChar);
                previousCharIsSeparator = false;
            }
            else if (!char.IsLetterOrDigit(currentChar))
            {
                if (!previousCharIsSeparator)
                    result.Append('-');

                previousCharIsSeparator = true;
            }
        }

        return result
            .Trim('-')
            .ToStringAndClear();
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