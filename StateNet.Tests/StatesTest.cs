namespace StateNet.Tests
{
    public class StatesTest
    {
        [Fact]
        public void TestStatesCreation()
        {
            string[] states = ["state1", "state2", "state3"];
            var machineBlueprint = StateMachine<string, string, string>.Factory((builder) =>
            {
                foreach (var state in states)
                {
                    var s = builder.AddState(state);

                    TestNoEventsAreCalled(s);
                }
            }, states[0]);

            var machine = machineBlueprint();

            Assert.Equal(machine.GetStates(), states);
        }

        [Fact]
        public void TestStatesCreationWithTransitions()
        {
            string[] states = ["state1", "state2", "state3"];
            var machineBlueprint = StateMachine<string, string, string>.Factory((builder) =>
            {
                for (short i = 0; i < states.Length; i++)
                {
                    var transitatesTo = states[(i + 1) >= states.Length ? 0 : i];
                    var s = builder.AddState(states[i], new() { {"transitate", [new(transitatesTo)]} });

                    TestNoEventsAreCalled(s);
                }
            }, states[0]);

            var machine = machineBlueprint();

            Assert.Equal(machine.GetStates(), states);
        }

        #region Utils

        static void TestNoEventsAreCalled<S, A, C>(State<S, A, C> state) where S : IComparable where A : IComparable
        {
            // Test no state event is triggered as this test only checks states list.
            // It should never call any state event as no trigger fn is called.
            state.OnEnter((info) => throw new Exception("OnEnter state event has been called"));
            state.OnExit((info) => throw new Exception("OnExit state event has been called"));
        }

        #endregion
    }
}