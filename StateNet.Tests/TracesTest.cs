namespace StateNet.Tests
{
    public class TracesTest
    {
        [Fact]
        public void SimpleTracesTest()
        {
            const string act = "TRANSITION";
            var machineBlueprint = StateMachine<string, string, string>.Factory((mb) => {
                var state1 = mb.AddState("S1");
                var state2 = mb.AddState("S2");
                var state3_1 = mb.AddState("S3.1");
                var state3_2 = mb.AddState("S3.2");

                // Transitions
                state1.AddTransition(act, state2);
                state2.AddTransition(act, state3_1);
                state3_1.AddTransition(act, state1);
                state3_2.AddTransition(act, state1);
                state2.AddTransition("diverse", state3_2);
            }, "S1");

            var machine = machineBlueprint();

            // Test normal machine flow
            Assert.Equal("S1", machine.CurrentState);
            machine.Trigger(act);
            Assert.Equal("S2", machine.CurrentState);
            machine.Trigger(act);
            Assert.Equal("S3.1", machine.CurrentState);

            machine.Trigger(act);
            Assert.Equal("S1", machine.CurrentState);
            machine.Trigger(act);
            Assert.Equal("S2", machine.CurrentState);
            machine.Trigger("diverse");
            Assert.Equal("S3.2", machine.CurrentState);

            // Test trace
            Assert.Equal(machine.GetTrace().GetStates(), new string[] { "S1", "S2", "S3.1", "S1", "S2", "S3.2" });
        }
    }
}
