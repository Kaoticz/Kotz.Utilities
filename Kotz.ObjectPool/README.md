# Kotz.ObjectPool

- Dependencies:
    - `Microsoft.Extensions.ObjectPool`

Defines the following types:

- FluentObjectPool\<T\>: wrapper for `DefaultObjectPool<T>` with a more functional-oriented interface.
- FluentObjectPool: static class containing methods to create an instance of a `FluentObjectPool<T>`.

## How to use
```cs
// Basic usage. Let's create a pool of StringBuilders as an example.
var pool = new FluentObjectPool<StringBuilder>(() => new StringBuilder());

// Full usage. Let's create a pool of StringBuilders where:
// - StringBuilders get automatically cleared every time they are returned to the pool.
// - StringBuilders are denied entry to the pool if their string exceeds 50000 in length.
// - The pool cannot hold more than 10 StringBuilders.
var pool = new FluentObjectPool<StringBuilder>(
    () => new StringBuilder(),  // How the object should be instantiated.
    stringBuilder => stringBuilder.Clear(),  // Optional: what should be done to all objects that are returned to the pool.
    stringBuilder => stringBuilder.Length <= 50000,  // Optional: what objects should be accepted when they are returned to the pool. If false, the object is not returned to the pool and may become eligible for garbage collection.
    10  // Optional: the maximum amount of objects the pool is allowed to hold.  Set to zero to disable the limit.
);
```

Alternatively, you can call `FluentObjectPool.Create()` to create an object pool.