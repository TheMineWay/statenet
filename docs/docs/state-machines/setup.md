---
sidebar_position: 1
---

# Setting up a State Machine

This guide will walk you through the setup process for creating and managing state machines using the `StateNet` library. We’ll cover:

1. Using the `Factory` method to define states.
2. Creating transitions between states.
3. Setting up state-specific events.

## 1. Initializing with the `Factory` Method

To start building a state machine, use the `Factory` method provided by the `StateMachine` class. The `Factory` method accepts a machine builder (`mb`), which is an instance of `MutableStateMachine`. We'll explain `MutableStateMachine` in detail later, but for now, understand that `mb` is used to define and organize your machine’s states.

Example:

```csharp
string initialState = "..."; // <- We will define this later

var machineBlueprint = StateMachine<string, string, string>.Factory((mb) => {
    // Define states here
}, initialState);
```

Notice that the `Factory<S, A, C>` Method accepts two generic types.

- S: defines the states type.
- A: defines the actions type.
- C: defines the context type.

In the following examples both types will be set to `string`, but you can use whatever type you like (it is recommended to use enum values).

### Adding states

Inside the `Factory` method, use `mb.AddState` to define each state in your state machine. For instance:

```csharp
string initialState = "IDLE"; // Initial state must be a defined state (see below)

var machineBlueprint = StateMachine<string, string, string>.Factory((mb) => {
    // IDLE and RUNNING will be the state identifiers
    var idleState = mb.AddState("IDLE");
    var runningState = mb.AddState("RUNNING");
}, initialState);
```

## 2. Creating Transitions

Transitions determine the paths between states. They consist of a origin state, the action that should be triggered to initiate transition, and the target state.

Example:

```csharp
string initialState = "IDLE";

var machineBlueprint = StateMachine<string, string, string>.Factory((mb) => {
    var idleState = mb.AddState("IDLE");
    var runningState = mb.AddState("RUNNING");

    // The first parameter represents the action that triggers a change to the state from a defined state
    idleState.AddTransition("start", "RUNNING"); // When "start" is triggered while the machine is on "IDLE" state it will transition to "RUNNING"
    runningState.AddTransition("stop", "IDLE");
}, initialState);
```

## 3. Setting Up Events

Events in `StateNet` allow you to perform actions whenever the state machine enters a state.

Example:

```csharp
string initialState = "IDLE";

var machineBlueprint = StateMachine<string, string, string>.Factory((mb) => {
    var idleState = mb.AddState("IDLE");
    var runningState = mb.AddState("RUNNING");

    idleState.AddTransition("start", "RUNNING");
    runningState.AddTransition("stop", "IDLE");

    // On enter IDLE state
    idleState.OnState((info) => {
        Console.WriteLine($"Entered IDLE state from {info.FromState}");
    });

    // On enter RUNNING state
    runningState.OnState((info) => {
        Console.WriteLine($"Entered RUNNING state from {info.FromState}")
    });
}, initialState);
```

In this example, the `OnState` method defines an action that will be triggered each time the machine enters a specified state.
