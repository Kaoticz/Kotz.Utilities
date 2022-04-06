namespace Kotz.Collections.Extensions;

public static class CollectionsExt
{
    /// <summary>
    /// Saves an <see cref="IEnumerable{T}"/> collection to an array rented from the <see cref="ArrayPool{T}"/>.
    /// </summary>
    /// <typeparam name="T">Data type contained in the collection.</typeparam>
    /// <param name="collection">This collection.</param>
    /// <remarks>
    /// Use this for short-lived arrays that exceed 1000 bytes in size or methods whose Gen0 allocation exceeds 1000 bytes.
    /// Don't forget to call <see cref="RentedArray{T}.Dispose"/> when you're done using it.
    /// </remarks>
    /// <returns>An array rented from the <see cref="ArrayPool{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Occurs when the <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static RentedArray<T> ToRentedArray<T>(this IEnumerable<T> collection)
        => new(collection);
}
