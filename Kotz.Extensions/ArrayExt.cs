using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Kotz.Extensions;

/// <summary>
/// Provides extension methods for arrays from the class <see cref="Array"/>.
/// </summary>
public static class ArrayExt
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// Performs an in-place shuffle of this array.
    /// </summary>
    /// <param name="array">The array to shuffle.</param>
    /// <param name="random">The <see cref="Random"/> object to suffle with.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>The shuffled <paramref name="array"/>.</returns>
    public static T[] Shuffle<T>(this T[] array, Random? random)
    {
        random ??= Random.Shared;
        random.Shuffle(array);

        return array;
    }
#endif

    /// <summary>
    /// Creates a new <see cref="ReadOnlySpan{T}"/> over the entirety of the specified <paramref name="array"/>.
    /// </summary>
    /// <param name="array">The array from which to create the <see cref="ReadOnlySpan{T}"/>.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>This <paramref name="array"/> wrapped in a <see cref="ReadOnlySpan{T}"/>.</returns>
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array)
        => (array.Length is 0) ? ReadOnlySpan<T>.Empty : new(array);

    /// <summary>
    /// Creates a new <see cref="ReadOnlySpan{T}"/> that includes a specified number of elements of an <paramref name="array"/>
    /// starting at a specified index.
    /// </summary>
    /// <param name="array">The array from which to create the <see cref="ReadOnlySpan{T}"/>.</param>
    /// <param name="startIndex">The index of the first element to include.</param>
    /// <param name="length">The number of elements to include.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>A segment of this <paramref name="array"/> wrapped in a <see cref="ReadOnlySpan{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="startIndex"/> is outside the bounds of the <paramref name="array"/> or
    /// <paramref name="startIndex"/> and <paramref name="length"/> exceed the number of elements in the <paramref name="array"/>.
    /// </exception>
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int startIndex, int length)
        => (array.Length is 0 || length is 0) ? ReadOnlySpan<T>.Empty : new(array, startIndex, length);

    /// <summary>
    /// Returns a read-only wrapper for the current array.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based array to wrap in a read-only <see cref="ReadOnlyCollection{T}"/> wrapper.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>A read-only <see cref="ReadOnlyCollection{T}"/> wrapper for the current array.</returns>
    public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
        => Array.AsReadOnly(array);

    /// <summary>
    /// Searches an entire one-dimensional sorted array for a specific elemenT1, T2sing the <see cref="IComparable{T}"/>
    /// generic interface implemented by each element of the <see cref="Array"/> and by the specified object.
    /// </summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The index of the specified value in the specified <paramref name="array"/>, if <paramref name="value"/> is found;
    /// otherwise, a negative number. If value is not found and value is less than one or more elements in array, the
    /// negative number returned is the bitwise complement of the index of the first element that is larger than value.
    /// If value is not found and value is greater than all elements in array, the negative number returned is the bitwise
    /// complement of (the index of the last element plus 1). If this method is called with a non-sorted array, the return
    /// value can be incorrect and a negative number could be returned, even if value is present in array.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    public static int BinarySearch<T>(this T[] array, T value) where T : IComparable<T>
        => Array.BinarySearch(array, value);

    /// <summary>
    /// Searches an entire one-dimensional sorted array for a value using the specified <see cref="IComparer{T}"/> generic interface.
    /// </summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> implementation to use when comparing elements. -or- null to use the <see cref="IComparable{T}"/>
    /// implementation of each element.
    /// </param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The index of the specified value in the specified <paramref name="array"/>, if value is found; otherwise, a negative number.
    /// If <paramref name="value"/> is not found and value is less than one or more elements in array, the negative number returned
    /// is the bitwise complement of the index of the first element that is larger than value. If value is not found and value is
    /// greater than all elements in array, the negative number returned is the bitwise complement of (the index of the last element
    /// plus 1). If this method is called with a non-sorted array, the return value can be incorrect and a negative number could be
    /// returned, even if value is present in array.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="comparer"/> is <see langword="null"/>, and <typeparamref name="T"/> does not implement the
    /// <see cref="IComparable{T}"/> generic interface.
    /// </exception>
    public static int BinarySearch<T>(this T[] array, T value, IComparer<T> comparer)
        => Array.BinarySearch(array, value, comparer);

    /// <summary>
    /// Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="IComparer{T}"/>
    /// generic interface.
    /// </summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> implementation to use when comparing elements. -or- null to use the <see cref="IComparable{T}"/>
    /// implementation of each element.
    /// </param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The index of the specified value in the specified <paramref name="array"/>, if value is found; otherwise, a negative number.
    /// If <paramref name="value"/> is not found and value is less than one or more elements in array, the negative number returned
    /// is the bitwise complement of the index of the first element that is larger than value. If value is not found and value is
    /// greater than all elements in array, the negative number returned is the bitwise complement of (the index of the last element
    /// plus 1). If this method is called with a non-sorted array, the return value can be incorrect and a negative number could be
    /// returned, even if value is present in array.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when index is less than the lower bound of <paramref name="array"/>. -or- length is less than zero.</exception>
    /// <exception cref="ArgumentException">
    /// Occurs when index and length do not specify a valid range in <paramref name="array"/>. -or- comparer is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="comparer"/> is <see langword="null"/> and <typeparamref name="T"/> does not implement the
    /// <see cref="IComparable{T}"/> generic interface.
    /// </exception>
    public static int BinarySearch<T>(this T[] array, int index, int length, T value, IComparer<T> comparer)
        => Array.BinarySearch(array, index, length, value, comparer);

    /// <summary>
    /// Searches a range of elements in a one-dimensional sorted array for a value, using the <see cref="IComparable{T}"/> generic
    /// interface implemented by each element of the <see cref="Array"/> and by the specified value.
    /// </summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The index of the specified value in the specified <paramref name="array"/>, if value is found; otherwise, a negative number.
    /// If <paramref name="value"/> is not found and value is less than one or more elements in array, the negative number returned
    /// is the bitwise complement of the index of the first element that is larger than value. If value is not found and value is
    /// greater than all elements in array, the negative number returned is the bitwise complement of (the index of the last element
    /// plus 1). If this method is called with a non-sorted array, the return value can be incorrect and a negative number could be
    /// returned, even if value is present in array.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when index is less than the lower bound of <paramref name="array"/>. -or- length is less than zero.</exception>
    /// <exception cref="ArgumentException">Occurs when index and length do not specify a valid range in <paramref name="array"/>.</exception>
    public static int BinarySearch<T>(this T[] array, int index, int length, T value) where T : IComparable<T>
        => Array.BinarySearch(array, index, length, value);

    /// <summary>
    /// Clears the contents of an array.
    /// </summary>
    /// <param name="array">The array to clear.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    public static void Clear<T>(this T[] array) where T : struct    // Safety measure, T[] should not contain T? elements
        => Array.Clear(array);

    /// <summary>
    /// Sets a range of elements in an array to the default value of each element type.
    /// </summary>
    /// <param name="array">The array whose elements need to be cleared.</param>
    /// <param name="index">The starting index of the range of elements to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Occurs when index is less than the lower bound of array. -or- length is less than zero. -or- The sum of index and
    /// length is greater than the size of array.
    /// </exception>
    public static void Clear<T>(this T[] array, int index, int length) where T : struct // Safety measure, T[] should not contain T? elements
        => Array.Clear(array, index, length);

    /// <summary>
    /// Copies a range of elements from an <see cref="Array"/> starting at the specified source index and pastes them to another
    /// <see cref="Array"/> starting at the specified destination index. Guarantees that all changes are undone if the copy does
    /// not succeed completely.
    /// </summary>
    /// <param name="sourceArray">The <see cref="Array"/> that contains the data to copy.</param>
    /// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray"/> at which copying begins.</param>
    /// <param name="destinationArray">The <see cref="Array"/> that receives the data.</param>
    /// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray"/> at which storing begins.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="sourceArray"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="sourceArray"/> is null. -or- <paramref name="destinationArray"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="sourceIndex"/> is less than the lower bound of the first dimension of <paramref name="sourceArray"/>.
    /// -or- <paramref name="destinationIndex"/> is less than the lower bound of the first dimension of <paramref name="destinationArray"/>.
    /// -or- <paramref name="length"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="length"/> is greater than the number of elements from <paramref name="sourceIndex"/> to the end of
    /// <paramref name="sourceArray"/>. -or- <paramref name="length"/> is greater than the number of elements from <paramref name="destinationIndex"/>
    /// to the end of <paramref name="destinationArray"/>.
    /// </exception>
    public static void ConstrainedCopy<T>(this T[] sourceArray, int sourceIndex, T[] destinationArray, int destinationIndex, int length)
        => Array.ConstrainedCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

    /// <summary>
    /// Converts an array of one type to an array of another type.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to convert to a target type.</param>
    /// <param name="converter">A <see cref="Converter{TInput, TOutput}"/>> that converts each element from one type to another type.</param>
    /// <typeparam name="T1">The type of the elements of the source array.</typeparam>
    /// <typeparam name="T2">The type of the elements of the target array.</typeparam>
    /// <returns>An array of the target type containing the converted elements from the source array.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="converter"/> is null.</exception>
    public static T2[] ConvertAll<T1, T2>(this T1[] array, Converter<T1, T2> converter)
        => Array.ConvertAll(array, converter);

    /// <summary>
    /// Determines whether the specified array contains elements that match the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the elements to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// <see langword="true"/> if <paramref name="array"/> contains one or more elements that match the conditions defined by
    /// the specified predicate; otherwise, <see langword="true"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static bool Exists<T>(this T[] array, Predicate<T> match)
        => Array.Exists(array, match);

    /// <summary>
    /// Assigns the given value of type <typeparamref name="T"/> to the elements of the specified <paramref name="array"/>
    /// which are within the range of <paramref name="startIndex"/> (inclusive) and the next <paramref name="count"/> number
    /// of indices.
    /// </summary>
    /// <param name="array">The <see cref="Array"/> to be filled.</param>
    /// <param name="value">The new value for the elements in the specified range.</param>
    /// <param name="startIndex">A 32-bit integer that represents the index in the <see cref="Array"/> at which filling begins.</param>
    /// <param name="count">The number of elements to copy.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    public static void Fill<T>(this T[] array, T value, int startIndex, int count)
        => Array.Fill(array, value, startIndex, count);

    /// <summary>
    /// Assigns the given value of type <typeparamref name="T"/> to each element of the specified <paramref name="array"/>.
    /// </summary>
    /// <param name="array">The array to be filled.</param>
    /// <param name="value">The value to assign to each array element.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    public static void Fill<T>(this T[] array, T value)
        => Array.Fill(array, value);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence
    /// within the entire <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based array to search.</param>
    /// <param name="match">The predicate that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value
    /// for type <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static T? Find<T>(this T[] array, Predicate<T> match)
        => Array.Find(array, match);

    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the elements to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// An <see cref="Array"/> containing all the elements that match the conditions defined by the specified predicate, if
    /// found; otherwise, an empty <see cref="Array"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static T[] FindAll<T>(this T[] array, Predicate<T> match)
        => Array.FindAll(array, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first
    /// occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number
    /// of elements.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found;
    /// otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>. -or- <paramref name="count"/>
    /// is less than zero. -or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in <paramref name="array"/>.
    /// </exception>
    public static int FindIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        => Array.FindIndex(array, startIndex, count, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
    /// index of the first occurrence within the range of elements in the <see cref="Array"/> that extends from the specified
    /// index to the last element.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>.</exception>
    public static int FindIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        => Array.FindIndex(array, startIndex, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
    /// index of the first occurrence within the entire <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static int FindIndex<T>(this T[] array, Predicate<T> match)
        => Array.FindIndex(array, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence
    /// within the entire <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value
    /// for type <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static T? FindLast<T>(this T[] array, Predicate<T> match)
        => Array.FindLast(array, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index
    /// of the last occurrence within the range of elements in the <see cref="Array"/> that contains the specified number of
    /// elements and ends at the specified index.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>. -or-
    /// <paramref name="count"/> is less than zero. -or- <paramref name="startIndex"/> and <paramref name="count"/> do not
    /// specify a valid section in <paramref name="array"/>.
    /// </exception>
    public static int FindLastIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        => Array.FindLastIndex(array, startIndex, count, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index
    /// of the last occurrence within the range of elements in the <see cref="Array"/> that extends from the first element to the
    /// specified index.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>.</exception>
    public static int FindLastIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        => Array.FindLastIndex(array, startIndex, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index
    /// of the last occurrence within the entire <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.</exception>
    public static int FindLastIndex<T>(this T[] array, Predicate<T> match)
        => Array.FindLastIndex(array, match);

    /// <summary>
    /// Performs the specified action on each element of the specified array.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> on whose elements the action is to be performed.</param>
    /// <param name="action">The <see cref="Action{T}"/> to perform on each element of array.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="action"/> is null.</exception>
    public static void ForEach<T>(this T[] array, Action<T> action)
        => Array.ForEach(array, action);

    /// <summary>
    /// Searches for the specified object and returns the index of its first occurrence in a one-dimensional array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>The zero-based index of the first occurrence of <paramref name="value"/> in the entire <paramref name="array"/>, if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this T[] array, T value)
        => Array.IndexOf(array, value);

    /// <summary>
    /// Searches for the specified object in a range of elements of a one dimensional array, and returns the index
    /// of its first occurrence. The range extends from a specified index to the end of the array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty <paramref name="array"/>.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="value"/> within the range of elements in
    /// <paramref name="array"/> that extends from <paramref name="startIndex"/> to the last element, if found;
    /// otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>.</exception>
    public static int IndexOf<T>(this T[] array, T value, int startIndex)
        => Array.IndexOf(array, value, startIndex);

    /// <summary>
    /// Searches for the specified object in a range of elements of a one-dimensional array, and returns the index
    /// of its first occurrence. The range extends from a specified index for a specified number of elements.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty <paramref name="array"/>.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="value"/> within the range of elements in
    /// <paramref name="array"/> that starts at <paramref name="startIndex"/> and contains the number of elements
    /// specified in <paramref name="count"/>, if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>. -or- count is less
    /// than zero. -or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in <paramref name="array"/>.
    /// </exception>
    /// <exception cref="RankException">Occurs when <paramref name="array"/> is multidimensional.</exception>
    public static int IndexOf<T>(this T[] array, T value, int startIndex, int count)
        => Array.IndexOf(array, value, startIndex, count);

    /// <summary>
    /// Searches for the specified object and returns the index of the last occurrence within the entire <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>The zero-based index of the last occurrence of <paramref name="value"/> within the entire <paramref name="array"/>, if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    public static int LastIndexOf<T>(this T[] array, T value)
        => Array.LastIndexOf(array, value);

    /// <summary>
    /// Searches for the specified object and returns the index of the last occurrence within the range of elements in
    /// the <see cref="Array"/> that extends from the first element to the specified index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the last occurrence of <paramref name="value"/> within the range of elements in
    /// <paramref name="array"/> that extends from the first element to <paramref name="startIndex"/>, if found;
    /// otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>.</exception>
    public static int LastIndexOf<T>(this T[] array, T value, int startIndex)
        => Array.LastIndexOf(array, value, startIndex);

    /// <summary>
    /// Searches for the specified object and returns the index of the last occurrence within the range of elements in
    /// the <see cref="Array"/> that contains the specified number of elements and ends at the specified index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> to search.</param>
    /// <param name="value">The object to locate in the <paramref name="array"/>.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// The zero-based index of the last occurrence of value within the range of elements in <paramref name="array"/> that
    /// contains the number of elements specified in <paramref name="count"/> and ends at <paramref name="startIndex"/>,
    /// if found; otherwise, -1.
    /// </returns>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="startIndex"/> is outside the range of valid indexes for <paramref name="array"/>. -or- count is less
    /// than zero. -or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in <paramref name="array"/>.
    /// </exception>
    /// <exception cref="RankException">Occurs when <paramref name="array"/> is multidimensional.</exception>
    public static int LastIndexOf<T>(this T[] array, T value, int startIndex, int count)
        => Array.LastIndexOf(array, value, startIndex, count);

    /// <summary>
    /// Changes the number of elements of this one-dimensional array to the specified new size.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based array to resize, or null to create a new array with the specified size.</param>
    /// <param name="newSize">The size of the new array.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="newSize"/> is less than zero.</exception>
    public static void Resize<T>([NotNull] this T[]? array, int newSize)
        => Array.Resize(ref array, newSize);

    /// <summary>
    /// Reverses the sequence of a subset of the elements in the one-dimensional generic array.
    /// </summary>
    /// <param name="array">The one-dimensional array of elements to reverse.</param>
    /// <param name="index">The starting index of the section to reverse.</param>
    /// <param name="length">The number of elements in the section to reverse.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="index"/> is less than the lower bound of <paramref name="array"/>. -or- <paramref name="length"/>
    /// is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="index"/> and <paramref name="length"/> do not specify a valid range in <paramref name="array"/>.
    /// </exception>
    public static void Reverse<T>(this T[] array, int index, int length)
        => Array.Reverse(array, index, length);

    /// <summary>
    /// Reverses the sequence of the elements in the one-dimensional generic array.
    /// </summary>
    /// <param name="array">The one-dimensional array of elements to reverse.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    public static void Reverse<T>(this T[] array)
        => Array.Reverse(array);

    /// <summary>
    /// Sorts the elements in a range of elements in an <see cref="Array"/> using the specified <see cref="IComparer{T}"/> generic
    /// interface.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> generic interface implementation to use when comparing elements, or <see langword="null"/>
    /// to use the <see cref="IComparable{T}"/> generic interface implementation of each element.
    /// </param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="index"/> is less than the lower bound of <paramref name="array"/>. -or- <paramref name="length"/>
    /// is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="index"/> and <paramref name="length"/> do not specify a valid range in <paramref name="array"/>.
    /// -or- The implementation of <paramref name="comparer"/> caused an error during the sort. For example, <paramref name="comparer"/>
    /// might not return 0 when comparing an item with itself.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="comparer"/> is <see langword="null"/>, and one or more elements in <paramref name="array"/> do not
    /// implement the <see cref="IComparable{T}"/> generic interface.
    /// </exception>
    public static void Sort<T>(this T[] array, int index, int length, IComparer<T> comparer)
        => Array.Sort(array, index, length, comparer);

    /// <summary>
    /// Sorts the elements in an <see cref="Array"/> using the specified <see cref="Comparison{T}"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to sort.</param>
    /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null. -or- <paramref name="comparison"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Occurs when the implementation of <paramref name="comparison"/> caused an error during the sort. For example,
    /// <paramref name="comparison"/> might not return 0 when comparing an item with itself.
    /// </exception>
    public static void Sort<T>(this T[] array, Comparison<T> comparison)
        => Array.Sort(array, comparison);

    /// <summary>
    /// Sorts the elements in a range of elements in an <see cref="Array"/> using the <see cref="IComparable{T}"/> generic
    /// interface implementation of each element of the <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="index"/> is less than the lower bound of <paramref name="array"/>.
    /// -or- <paramref name="length"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="index"/> and <paramref name="length"/> do not specify a valid range in array.
    /// </exception>
    public static void Sort<T>(this T[] array, int index, int length) where T : IComparable<T>
        => Array.Sort(array, index, length);

    /// <summary>
    /// Sorts a pair of <see cref="Array"/> objects (one contains the keys and the other contains the corresponding items) based
    /// on the keys in the first <see cref="Array"/> using the <see cref="IComparable{T}"/> generic interface implementation of
    /// each key.
    /// </summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="Array"/> that contains the keys to sort.</param>
    /// <param name="items">
    /// The one-dimensional, zero-based <see cref="Array"/> that contains the items that correspond to the keys in
    /// <paramref name="keys"/>, or <see langword="null"/> to sort only keys.
    /// </param>
    /// <typeparam name="T1">Data type contained in the <paramref name="keys"/> array.</typeparam>
    /// <typeparam name="T2">Data type contained in the <paramref name="items"/> array.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="keys"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="items"/> is not null, and the lower bound of keys does not match the lower bound of items.
    /// -or- <paramref name="items"/> is not null, and the length of <paramref name="keys"/> is greater than the length of items.
    /// </exception>
    public static void Sort<T1, T2>(this T1[] keys, T2[]? items) where T1 : IComparable<T1>
        => Array.Sort(keys, items);

    /// <summary>
    /// Sorts the elements in an <see cref="Array"/> using the specified <see cref="IComparer{T}"/> generic interface.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-base <see cref="Array"/> to sort.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> generic interface implementation to use when comparing elements, or null to use
    /// the <see cref="IComparable{T}"/> generic interface implementation of each element.
    /// </param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null.</exception>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="comparer"/> is null, and one or more elements in <paramref name="array"/> do not
    /// implement the <see cref="IComparable{T}"/> generic interface.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when the implementation of <paramref name="comparer"/> caused an error during the sort. For example,
    /// <paramref name="comparer"/> might not return 0 when comparing an item with itself.
    /// </exception>
    public static void Sort<T>(this T[] array, IComparer<T> comparer)
        => Array.Sort(array, comparer);

    /// <summary>
    /// Sorts a range of elements in a pair of <see cref="Array"/> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="Array"/> using the <see cref="IComparable{T}"/> generic interface implementation of each key.
    /// </summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="Array"/> that contains the keys to sort.</param>
    /// <param name="items">
    /// The one-dimensional, zero-based <see cref="Array"/> that contains the items that correspond to the keys in <paramref name="keys"/>,
    /// or <see langword="null"/> to sort only keys.
    /// </param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <typeparam name="T1">Data type contained in the <paramref name="keys"/> array.</typeparam>
    /// <typeparam name="T2">Data type contained in the <paramref name="items"/> array.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="keys"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="index"/> is less than the lower bound of keys. -or- <paramref name="length"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="items"/> is not null, and the lower bound of <paramref name="keys"/> does not match the lower bound
    /// of <paramref name="items"/>. -or- <paramref name="items"/> is not null, and the <paramref name="length"/> of keys is greater than
    /// the length of <paramref name="items"/>. -or- <paramref name="index"/> and <paramref name="length"/> do not specify a valid range
    /// in the <paramref name="keys"/> <see cref="Array"/>. -or- <paramref name="items"/> is not null, and <paramref name="index"/> and
    /// <paramref name="length"/> do not specify a valid range in the <paramref name="items"/> <see cref="Array"/>.
    /// </exception>
    public static void Sort<T1, T2>(this T1[] keys, T2[]? items, int index, int length) where T1 : IComparable<T1>
        => Array.Sort(keys, items, index, length);

    /// <summary>
    /// Sorts a range of elements in a pair of <see cref="Array"/> objects (one contains the keys and the other contains
    /// the corresponding items) based on the keys in the first <see cref="Array"/> using the specified <see cref="IComparer{T}"/>
    /// generic interface.
    /// </summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="Array"/> that contains the keys to sort.</param>
    /// <param name="items">
    /// The one-dimensional, zero-based <see cref="Array"/> that contains the items that correspond to the keys in keys,
    /// or <see langword="null"/> to sort only keys.
    /// </param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> generic interface implementation to use when comparing elements, or <see langword="null"/> to
    /// use the <see cref="IComparable{T}"/> generic interface implementation of each element.
    /// </param>
    /// <typeparam name="T1">Data type contained in the <paramref name="keys"/> array.</typeparam>
    /// <typeparam name="T2">Data type contained in the <paramref name="items"/> array.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="keys"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Occurs when <paramref name="index"/> is less than the lower bound of keys. -or- <paramref name="length"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="items"/> is not null, and the lower bound of <paramref name="keys"/> does not match the lower
    /// bound of <paramref name="items"/>. -or- <paramref name="items"/> is not null, and the length of <paramref name="keys"/> is
    /// greater than the length of <paramref name="items"/>. -or- <paramref name="index"/> and <paramref name="length"/> do not
    /// specify a valid range in the <paramref name="keys"/> <see cref="Array"/>. -or- <paramref name="items"/> is not null, and
    /// <paramref name="index"/> and <paramref name="length"/> do not specify a valid range in the <paramref name="items"/> <see cref="Array"/>.
    /// -or- The implementation of <paramref name="comparer"/> caused an error during the sort. For example, comparer might not
    /// return 0 when comparing an item with itself.
    /// </exception>
    public static void Sort<T1, T2>(this T1[] keys, T2[]? items, int index, int length, IComparer<T1> comparer) where T1 : IComparable<T1>
        => Array.Sort(keys, items, index, length, comparer);

    /// <summary>
    /// Sorts the elements in an entire <see cref="Array"/> using the <see cref="IComparable{T}"/> generic interface
    /// implementation of each element of the <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to sort.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="array"/> is null.</exception>
    public static void Sort<T>(this T[] array) where T : IComparable<T>
        => Array.Sort(array);

    /// <summary>
    /// Sorts a pair of <see cref="Array"/> objects (one contains the keys and the other contains the corresponding items) based
    /// on the keys in the first <see cref="Array"/> using the specified <see cref="IComparer{T}"/> generic interface.
    /// </summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="Array"/> that contains the keys to sort.</param>
    /// <param name="items">
    /// The one-dimensional, zero-based <see cref="Array"/> that contains the items that correspond to the keys in
    /// <paramref name="keys"/>, or null to sort only keys.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> generic interface implementation to use when comparing elements, or <see langword="null"/>
    /// to use the <see cref="IComparable{T}"/> generic interface implementation of each element.
    /// </param>
    /// <typeparam name="T1">Data type contained in the <paramref name="keys"/> array.</typeparam>
    /// <typeparam name="T2">Data type contained in the <paramref name="items"/> array.</typeparam>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="keys"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Occurs when <paramref name="items"/> is not null, and the lower bound of <paramref name="keys"/> does not match the lower
    /// bound of <paramref name="items"/>. -or- <paramref name="items"/> is not null, and the length of <paramref name="keys"/> is
    /// greater than the length of <paramref name="items"/>. -or- The implementation of <paramref name="comparer"/> caused an error
    /// during the sort. For example, <paramref name="comparer"/> might not return 0 when comparing an item with itself.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Occurs when <paramref name="comparer"/> is null, and one or more elements in the <paramref name="keys"/> <see cref="Array"/>
    /// do not implement the <see cref="IComparable{T}"/> generic interface.
    /// </exception>
    public static void Sort<T1, T2>(this T1[] keys, T2[]? items, IComparer<T1> comparer)
        => Array.Sort(keys, items, comparer);

    /// <summary>
    /// Determines whether every element in the array matches the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="Array"/> to check against the conditions.</param>
    /// <param name="match">The predicate that defines the conditions to check against the elements.</param>
    /// <typeparam name="T">Data type contained in the <paramref name="array"/>.</typeparam>
    /// <returns>
    /// <see langword="true"/> if every element in <paramref name="array"/> matches the conditions defined by the
    /// specified predicate; otherwise, <see langword="false"/>. If there are no elements in the <paramref name="array"/>,
    /// the return value is <see langword="true"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Occurs when <paramref name="array"/> is null. -or- <paramref name="match"/> is null.
    /// </exception>
    public static bool TrueForAll<T>(this T[] array, Predicate<T> match)
        => Array.TrueForAll(array, match);
}