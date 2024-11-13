namespace StateNet.Tests
{
    public class ContextTest
    {
        [Fact]
        public void ContextMutates()
        {
            // This tests that the machine is able to count how many days have passed by mutatint its context each day

            const string ACTION = "pass_time";
            var initialContext = 0;

            var machine = StateMachine<string, string, int>.Factory((builder) =>
            {
                var dayState = builder.AddState("day");
                dayState.AddTransition(ACTION, "night");

                var nightState = builder.AddState("night");
                nightState.AddTransition(ACTION, "day");

                dayState.OnEnter((info) => {
                    var machine = info.Machine;
                    machine.MutateContext((context) => context + 1);
                });
            })("day", initialContext);

            Assert.Equal(0, machine.Context);
            machine.Trigger(ACTION);
            Assert.Equal(0, machine.Context);

            machine.Trigger(ACTION);
            Assert.Equal(1, machine.Context);
            machine.Trigger(ACTION);
            Assert.Equal(1, machine.Context);

            machine.Trigger(ACTION);
            Assert.Equal(2, machine.Context);
            machine.Trigger(ACTION);
            Assert.Equal(2, machine.Context);
        }
    }
}
