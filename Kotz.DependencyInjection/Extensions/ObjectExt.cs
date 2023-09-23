namespace Kotz.DependencyInjection.Extensions;

/// <summary>
/// Provides internal extension methods for <see cref="object"/>.
/// </summary>
internal static class ObjectExt
{
    /// <summary>
    /// Determines whether this object is equal to any element in <paramref name="objects"/>.
    /// </summary>
    /// <param name="thisObj">This object.</param>
    /// <param name="objects">Collection of objects to compare to.</param>
    /// <returns><see langword="true"/> if this object is equal to any element in <paramref name="objects"/>, <see langword="false"/> otherwise.</returns>
    internal static bool EqualsAny<T>(this T thisObj, params T?[] objects)
        => EqualsAny(thisObj, objects.AsEnumerable());

    /// <summary>
    /// Determines whether this object is equal to any element in <paramref name="objects"/>.
    /// </summary>
    /// <param name="thisObj">This object.</param>
    /// <param name="objects">Collection of objects to compare to.</param>
    /// <returns><see langword="true"/> if this object is equal to any element in <paramref name="objects"/>, <see langword="false"/> otherwise.</returns>
    internal static bool EqualsAny<T>(this T thisObj, IEnumerable<T?> objects)
    {
        if (thisObj is null && objects is null)
            return true;
        else if (objects is null)
            return false;

        foreach (var obj in objects)
        {
            if (EqualityComparer<T>.Default.Equals(thisObj, obj))
                return true;
        }

        return false;
    }
}