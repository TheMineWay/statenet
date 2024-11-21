---
sidebar_position: 5
---

# Working with the context

Each machine allows you to mutate a value when events happen. For example, you can count how many times a state has been reached.

## Defining the context type

The context datatype is defined as the third generic type of the machine. In the following example we will use an `int` value:

```csharp
string initialState = "...";

var machineBlueprint = StateMachine<string, string, int>.Factory((builder) =>
{
    /* ... */
}, initialState);
```

You can define the initial context value by providing another argument after the `initialState`.

```csharp
string initialState = "...";
int initialContext = 0;

var machineBlueprint = StateMachine<string, string, int>.Factory((builder) =>
{
    /* ... */
}, initialState, initialContext);
```

## Mutating the context

You might want to mutate the context value when some events happen. You can do so by using the `SetContext` and `MutateContext` methods.

- `SetContext`: accepts a context value. It updates the context setting it to whatever value you passed as an argument.
- `MutateContext`: accepts a function. The function provides as a parameter the current context value and updates the context with the return value.

Lets see how:

```csharp
string initialState = "day";
int initialContext = 0;
string ACTION = "next";

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
}, initialState, initialContext);
```

This example shows us a state machine that switchs between _day_ and _night_ and counts how many days have passed.
