---
sidebar_position: 4
---

# Working with the context

Each machine allows you to mutate a value when events happen. For example, you can count how many times a state has been reached.

## Defining the context type

The context datatype is defined as the third generic type of the machine. In the following example we will use an `int` value:

```csharp
var machineBlueprint = StateMachine<string, string, int>.Factory((builder) =>
{
    /* ... */
});
```

You can define the initial context value by using the `SetContext` method available on the `builder`.

```csharp
var machineBlueprint = StateMachine<string, string, int>.Factory((builder) =>
{
    builder.SetContext(1);

    /* ... */
});
```

:::warning Be careful

If you set the default context value using the `Factory` (as the example above does) you need to do it before defining events.

:::

## Mutating the context

You might want to mutate the context value when some events happen. You can do so by using the `SetContext` and `MutateContext` methods.

- `SetContext`: accepts a context value. It updates the context setting it to whatever value you passed as an argument.
- `MutateContext`: accepts a function. The function provides as a parameter the current context value and updates the context with the return value.

Lets see how:

```csharp
var machine = StateMachine<string, string, int>.Factory((builder) =>
{
    /* Define day state & transition to night */
    var dayState = builder.AddState("day");
    dayState.AddTransition(ACTION, "night");

    /* Define night state & transition to day */
    var nightState = builder.AddState("night");
    nightState.AddTransition(ACTION, "day");

    /* We add an event that increments the context by one when we enter the day state */
    dayState.OnEnter((info) => {
        var machine = info.Machine;
        machine.MutateContext((context) => context + 1);
    });
});
```

This example shows us a state machine that switchs between _day_ and _night_ and counts how many days have passed.
