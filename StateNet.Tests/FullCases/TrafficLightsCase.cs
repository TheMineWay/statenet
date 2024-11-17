
namespace StateNet.Tests.FullCases
{
    internal class TrafficLightsCase : AMachineTester<TrafficLightsState, TrafficLightsAction, TrafficLightsContext>
    {
        protected override Func<StateMachine<TrafficLightsState, TrafficLightsAction, TrafficLightsContext>> GetMachineBlueprint()
        {
            return StateMachine<TrafficLightsState, TrafficLightsAction, TrafficLightsContext>.Factory((mb) => {
                var redState = mb.AddState(TrafficLightsState.RED);
                var yellowState = mb.AddState(TrafficLightsState.YELLOW);
                var greenState = mb.AddState(TrafficLightsState.GREEN);

                redState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.GREEN);
                yellowState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.RED);
                greenState.AddTransition(TrafficLightsAction.CHANGE, TrafficLightsState.YELLOW);
            }, GetInitialState());
        }

        public override void TestTransitions()
        {
            var machine = GetMachineBlueprint()();

            machine.SetContext(GetInitialContext());
            Assert.Equal(TrafficLightsState.RED, machine.CurrentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.GREEN, machine.CurrentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.YELLOW, machine.CurrentState);

            machine.Trigger(TrafficLightsAction.CHANGE);
            Assert.Equal(TrafficLightsState.RED, machine.CurrentState);
        }

        public override void TestEvents()
        {
            
        }

        protected override TrafficLightsState[] GetStates() => [TrafficLightsState.RED, TrafficLightsState.YELLOW, TrafficLightsState.GREEN];
        protected override TrafficLightsState GetInitialState() => TrafficLightsState.RED;
        protected override TrafficLightsContext GetInitialContext() => new() { timesChanged = 0 };
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

    internal struct TrafficLightsContext
    {
        public int timesChanged = 0;

        public TrafficLightsContext()
        {
        }
    }
}
