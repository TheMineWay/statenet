---
sidebar_position: 2
---

# Examples

See some examples of state machines.

## Traffic lights system

```csharp
// Define the blueprint for the traffic light state machine
var trafficLightBlueprint = StateMachine<LightStates, LightActions>.Factory((eb) => {
    var redState = eb.AddState(LightStates.RED)
                     .OnState((action) => Console.WriteLine("Light is RED"));
    var greenState = eb.AddState(LightStates.GREEN)
                       .OnState((action) => Console.WriteLine("Light is GREEN"));
    var yellowState = eb.AddState(LightStates.YELLOW)
                        .OnState((action) => Console.WriteLine("Light is YELLOW"));

    // Define transitions for traffic light states
    redState.AddTransition(LightActions.TIMER_EXPIRED, LightStates.GREEN);
    greenState.AddTransition(LightActions.TIMER_EXPIRED, LightStates.YELLOW);
    yellowState.AddTransition(LightActions.TIMER_EXPIRED, LightStates.RED);

    // Emergency action transitions all states to RED
    greenState.AddTransition(LightActions.EMERGENCY, LightStates.RED);
    yellowState.AddTransition(LightActions.EMERGENCY, LightStates.RED);
});

// Create an instance of the traffic light state machine starting at RED
var trafficLightMachine = trafficLightBlueprint(LightStates.RED);

// Simulate the timer expiring multiple times
Console.WriteLine("Traffic Light Simulation:");

// Transition from RED -> GREEN
trafficLightMachine.Trigger(LightActions.TIMER_EXPIRED);

// Transition from GREEN -> YELLOW
trafficLightMachine.Trigger(LightActions.TIMER_EXPIRED);

// Transition from YELLOW -> RED
trafficLightMachine.Trigger(LightActions.TIMER_EXPIRED);

// Trigger an emergency, should go to RED regardless of the current state
trafficLightMachine.Trigger(LightActions.EMERGENCY);
```
