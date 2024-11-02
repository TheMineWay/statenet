---
sidebar_position: 4
---

# Triggering actions

Once you have a [machine instance](./instantiating-machines.md) you can start triggering actions.

## What actions are?

In a state machine context, actions are operations or triggers that drive transitions between states. Actions are typically the "inputs" that cause the state machine to change from one state to another according to defined rules.

In your `StateMachine<S, A>` class, the generic parameter A represents the type of actions used within the state machine. This allows you to define specific action types that the state machine can handle, making it possible to customize behavior based on different kinds of events or inputs, represented by A. This approach helps maintain type safety and allows flexibility in defining what each action does within the machine.

See here an example of actions defined as an `enum`:

```csharp
enum Actions {
    START,
    STOP
}

// In this example states are string values, and actions are Actions (from the enum defined above)
var machineBlueprint = StateMachine<string, Actions>.Factory((eb) => {
    // States
    var idleState = eb.AddState("RUNNING");
    var runningState = eb.AddState("RUNNING");

    // Transitions
    idleState.AddTransition(Actions.START, "RUNNING");
    runningState.AddTransition(Actions.STOP, "RUNNING");
});
```
