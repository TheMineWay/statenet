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
                    builder.AddState(state);
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
                    builder.AddState(states[i], new() { {"transitate", new(transitatesTo)} });
                }
            }, states[0]);

            var machine = machineBlueprint();

            Assert.Equal(machine.GetStates(), states);
        }
    }
}