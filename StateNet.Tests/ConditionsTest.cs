namespace StateNet.Tests
{
    public class ConditionsTest
    {
        [Fact]
        public void SimpleConditions()
        {
            // This tests that the machine is able to count how many days have passed by mutatint its context each day

            const string ACTION = "pass_time";
            int initialContext = 0;

            var machine = StateMachine<string, string, int>.Factory((builder) =>
            {
                var dayState = builder.AddState("day");
                var transition = dayState.AddTransition(ACTION, "night");

                var nightState = builder.AddState("night");
                nightState.AddTransition(ACTION, "day").When((info) => info.Machine.Context < 10);

                dayState.OnEnter((info) => {
                    var machine = info.Machine;
                    machine.MutateContext((context) => context + 1);
                });
            }, "day", initialContext)();

            Assert.Equal(initialContext, machine.Context);

            for (int i = 0; i <= 12; i++) {
                Assert.Equal(i <= 10 ? i : 10, machine.Context);
                machine.Trigger(ACTION);
                Assert.Equal(i <= 10 ? i : 10, machine.Context);
                machine.Trigger(ACTION);
            }
        }
    }
}
