using System.Runtime.CompilerServices;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Enum"/> types.
/// </summary>
public static class EnumExt
{
    /// <summary>
    /// Adds <paramref name="flag"/> to this flag enum if it's not present or removes it if it's present.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">This enum.</param>
    /// <param name="flag">The enum value to be added or removed.</param>
    /// <returns>This enum with <paramref name="flag"/> added or removed from it.</returns>
    public static T ToggleFlag<T>(this T value, T flag) where T : struct, Enum
        => (ContainsFlags(value, flag)) ? SeparateFlags(value, flag) : CombineFlags(value, flag);

    /// <summary>
    /// Determines whether at least one of the provided bit fields is set in the current instance.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">This enum.</param>
    /// <param name="flag">An enumeration value.</param>
    /// <returns><see langword="true"/> if at least one bit field set in <paramref name="flag"/> is also set in the current instance, <see langword="false"/> otherwise.</returns>
    public static bool HasOneFlag<T>(this T value, T flag) where T : struct, Enum
        => ContainsFlags(value, flag);

    /// <summary>
    /// Gets the bitwise flags of this collection of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="values">This collection of enums.</param>
    /// <returns>A <typeparamref name="T"/> object with its flags set to the collection's enums.</returns>
    public static T ToFlags<T>(this IEnumerable<T> values) where T : struct, Enum
    {
        var result = default(T);

        foreach (var value in values)
            result = CombineFlags(result, value);

        return result;
    }

    /// <summary>
    /// Creates a collection of human-readable strings of this enum.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">This enum.</param>
    /// <param name="format">A format string ("G", "D", "X" or "F").</param>
    /// <remarks>Only works for enums marked with the <see cref="FlagsAttribute"/>.</remarks>
    /// <returns>The human-readable strings.</returns>
    public static IEnumerable<string> ToStrings<T>(this T value, string? format = default) where T : struct, Enum
    {
        return Enum.GetValues<T>()
            .Where(x => x.HasOneFlag(value))
            .Select(x => x.ToString(format))
            .OrderBy(x => x);
    }

    /// <summary>
    /// Gets all values of the marked bitflags in this enum.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">This enum.</param>
    /// <remarks>
    /// If the enum contains a value equal to 0 or values that aggregate multiple bitflags,
    /// they will be returned along with the marked bitflags.
    /// </remarks>
    /// <returns>All individual enum values contained in this enum or <see langword="default"/> if no bitflag is marked.</returns>
    public static IEnumerable<T> ToValues<T>(this T value) where T : struct, Enum
    {
        return Enum.GetValues<T>()
            .Where(x => value.HasFlag(x))
            .DefaultIfEmpty();
    }

    /// <summary>
    /// Combines two enum flags.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="x">The first flag.</param>
    /// <param name="y">The second flag.</param>
    /// <returns>The combined flags.</returns>
    /// <exception cref="NotSupportedException">Occurs when the enum can't be represented by a native CLR integer type.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T CombineFlags<T>(T x, T y) where T : struct, Enum
    {
        if (Unsafe.SizeOf<T>() is sizeof(byte))
        {
            var result = (byte)(Unsafe.As<T, byte>(ref x) | Unsafe.As<T, byte>(ref y));
            return Unsafe.As<byte, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(short))
        {
            var result = (short)(Unsafe.As<T, short>(ref x) | Unsafe.As<T, short>(ref y));
            return Unsafe.As<short, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(int))
        {
            var result = Unsafe.As<T, int>(ref x) | Unsafe.As<T, int>(ref y);
            return Unsafe.As<int, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(long))
        {
            var result = Unsafe.As<T, long>(ref x) | Unsafe.As<T, long>(ref y);
            return Unsafe.As<long, T>(ref result);
        }

        throw new NotSupportedException($"Enum of size {Unsafe.SizeOf<T>()} has no corresponding CLR integer type.");
    }

    /// <summary>
    /// Separates two enum flags.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="x">The first flag.</param>
    /// <param name="y">The second flag.</param>
    /// <returns>The combined flags.</returns>
    /// <exception cref="NotSupportedException">Occurs when the enum can't be represented by a native CLR integer type.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T SeparateFlags<T>(T x, T y) where T : struct, Enum
    {
        if (Unsafe.SizeOf<T>() is sizeof(byte))
        {
            var result = (byte)(Unsafe.As<T, byte>(ref x) & ~Unsafe.As<T, byte>(ref y));
            return Unsafe.As<byte, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(short))
        {
            var result = (short)(Unsafe.As<T, short>(ref x) & ~Unsafe.As<T, short>(ref y));
            return Unsafe.As<short, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(int))
        {
            var result = Unsafe.As<T, int>(ref x) & ~Unsafe.As<T, int>(ref y);
            return Unsafe.As<int, T>(ref result);
        }
        else if (Unsafe.SizeOf<T>() is sizeof(long))
        {
            var result = Unsafe.As<T, long>(ref x) & ~Unsafe.As<T, long>(ref y);
            return Unsafe.As<long, T>(ref result);
        }

        throw new NotSupportedException($"Enum of size {Unsafe.SizeOf<T>()} has no corresponding CLR integer type.");
    }

    /// <summary>
    /// Checks if <paramref name="x"/> contains the <paramref name="y"/> flag
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="x">The first flag.</param>
    /// <param name="y">The second flag.</param>
    /// <returns><see langword="true"/> if one of the flags in <paramref name="y"/> is found in <paramref name="x"/>, <see langword="false"/> otherwise.</returns>
    /// <exception cref="NotSupportedException">Occurs when the enum can't be represented by a native CLR integer type.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ContainsFlags<T>(T x, T y) where T : struct, Enum
    {
        return Unsafe.SizeOf<T>() switch
        {
            sizeof(byte) => ((byte)(Unsafe.As<T, byte>(ref x) & Unsafe.As<T, byte>(ref y))) > 0,
            sizeof(short) => ((short)(Unsafe.As<T, short>(ref x) & Unsafe.As<T, short>(ref y))) > 0,
            sizeof(int) => (Unsafe.As<T, int>(ref x) & Unsafe.As<T, int>(ref y)) > 0,
            sizeof(long) => (Unsafe.As<T, long>(ref x) & Unsafe.As<T, long>(ref y)) > 0,
            _ => throw new NotSupportedException($"Enum of size {Unsafe.SizeOf<T>()} has no corresponding CLR integer type.")
        };
    }
}