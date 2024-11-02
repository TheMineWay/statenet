---
sidebar_position: 3
---

# Instantiating machines

Once you have your machine blueprint ([learn how to initializate it](./setup.md)) you can create a usable instance of the machine. Each instance will be independent, so you can have more than one instance active at the same time.

Instantiating a blueprint is as easy as calling the blueprint as a method, just as the following example:

```csharp
var machineBlueprint = StateMachine<string, string>.Factory((mb) => {
    // States definition
    // ...
});

// Instantiate the machine
var machine = machineBlueprint();
```

Once you have a machine instance you are ready to start working with states.
