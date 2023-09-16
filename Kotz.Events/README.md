# Kotz.Events

Defines the following types:

- **EventHandler<T1, T2>**
    - Where T1 is the object that sends the event and T2 is the EventArgs.
- **AsyncEventHandler**
    - Takes an `object?` type and an EventArgs.
- **AsyncEventHandler<T1, T2>**
    - Where T1 is the object that sends the event and T2 is the EventArgs.
- **AsyncEvent<T1, T2>**
    - Where T1 is the object that sends the event and T2 is the EventArgs.
    - In contrast to AsyncEventHandler, AsyncEvent properly awaits for the execution of all handlers registered in it.