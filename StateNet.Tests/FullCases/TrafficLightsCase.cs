
namespace StateNet.Tests.FullCases
{
    internal class TrafficLightsCase : AMachineTester<TrafficLightsState, TrafficLightsAction>
    {
        protected override Func<TrafficLightsState, StateMachine<TrafficLightsState, TrafficLightsAction>> GetMachineBlueprint()
        {
            return StateMachine<TrafficLightsState, TrafficLightsAction>.Factory((mb) => {
                var redState = mb.AddState(TrafficLightsState.RED);
                var yellowState = mb.AddState(TrafficLightsState.YELLOW);
                var greenState = mb.AddState(TrafficLightsState.GREEN);

                redState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.GREEN);
                yellowState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.RED);
                greenState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.YELLOW);
            });
        }

        public override void TestTransitions()
        {
            var machine = GetMachineBlueprint()(TrafficLightsState.RED);
            Assert.Equal(TrafficLightsState.RED, machine.currentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.GREEN, machine.currentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.YELLOW, machine.currentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.RED, machine.currentState);
        }

        public override void TestEvents()
        {
            
        }

        protected override TrafficLightsState[] GetStates() => [TrafficLightsState.RED, TrafficLightsState.YELLOW, TrafficLightsState.GREEN];
        protected override TrafficLightsState GetInitialState() => TrafficLightsState.RED;
    }

    internal enum TrafficLightsState
    {
        RED,
        GREEN,
        YELLOW,
    }

    internal enum TrafficLightsAction
    {
        CHANGE
    }
}
