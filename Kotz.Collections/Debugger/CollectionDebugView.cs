using System.Diagnostics;

namespace Kotz.Collections.Debugger;

/// <summary>
/// Defines a linear view of a collection in the debugger dropdown window.
/// </summary>
/// <typeparam name="T">The data type of the elements in the collection.</typeparam>
/// <remarks>
/// Code sourced from <a href="https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/ICollectionDebugView.cs">here</a>.
/// </remarks>
internal sealed class CollectionDebugView<T>
{
    private readonly ICollection<T> _collection;

    public CollectionDebugView(ICollection<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        _collection = collection;
    }

    /// <summary>
    /// The items to be shown in the debugger window.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items
        => _collection.ToArray();
}