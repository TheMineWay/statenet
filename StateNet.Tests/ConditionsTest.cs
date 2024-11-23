namespace StateNet.Tests
{
    public class ConditionsTest
    {
        [Fact]
        public void SimpleConditions()
        {
            // This tests that the machine is able to count how many days have passed by mutating its context each day
            // This time, count stops at day 10

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

        [Fact]
        public void ConditionDependantTransitions()
        {
            // This tests that the machine is able to count how many days have passed by mutating its context each day
            // This time, count stops at day 10

            const string ACTION = "action";

            var machine = StateMachine<string, string, int>.Factory((mb) =>
            {
                var a = mb.AddState("A");
                var b = mb.AddState("B");
                var c = mb.AddState("C");

                // Same state with two transitions with same action
                a.AddTransition(ACTION, "B");
                a.AddTransition(ACTION, "C").When((info) => info.Machine.Context == 0);

                // Back to A
                c.AddTransition(ACTION, "A");
                b.AddTransition(ACTION, "A");

                // Events
                a.OnExit((info) =>
                {
                    info.Machine.MutateContext((c) => c == 0 ? 1 : 0);
                });
            }, "A", 0)();

            Assert.Equal(0, machine.Context);
            machine.Trigger(ACTION);
            Assert.Equal("C", machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal("A", machine.CurrentState);

            // Now it should go througth state B
            machine.Trigger(ACTION);
            Assert.Equal("B", machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal("A", machine.CurrentState);

            // Now, again via state C
            machine.Trigger(ACTION);
            Assert.Equal("C", machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal("A", machine.CurrentState);
        }

        [Fact]
        public void ConditionalStates() {
            const string ACTION = "switch";
            const string GREEN = "GREEN", YELLOW = "YELLOW", RED = "RED";

            var machine = StateMachine<string, string, Dictionary<string, bool>>.Factory((mb) =>
            {
                static bool isEnabledFn(Info.TransitionInfo<string, string, Dictionary<string, bool>> info) => info.Machine.Context?.GetValueOrDefault("enabled", false) ?? false;

                var greenState = mb.AddState(GREEN);
                var yellowState = mb.AddState(YELLOW);
                var redState = mb.AddState(RED);

                greenState.AddTransition(ACTION, YELLOW).When(isEnabledFn).When((info) => info.Machine.Context?.GetValueOrDefault("hasYellow", false) == true);
                greenState.AddTransition(ACTION, RED).When(isEnabledFn).When((info) => info.Machine.Context?.GetValueOrDefault("hasYellow", false) == false);
                yellowState.AddTransition(ACTION, RED).When(isEnabledFn);
                redState.AddTransition(ACTION, GREEN).When(isEnabledFn);

            }, GREEN, new() { { "enabled", true }, { "hasYellow", true } })();

            // Simple machine
            Assert.Equal(GREEN, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(YELLOW, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(RED, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(GREEN, machine.CurrentState);

            // Tweak settings (disable switch)

            machine.MutateContext((context) => {
                context["enabled"] = false;
                return context;
            });

            machine.Trigger(ACTION);
            Assert.Equal(GREEN, machine.CurrentState);

            // Tweak settings (enable switch)

            machine.MutateContext((context) => {
                context["enabled"] = true;
                return context;
            });

            machine.Trigger(ACTION);
            Assert.Equal(YELLOW, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(RED, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(GREEN, machine.CurrentState);

            // Tweak settings (disable hasYellow)

            machine.MutateContext((context) => {
                context["hasYellow"] = false;
                return context;
            });

            Assert.Equal(GREEN, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(RED, machine.CurrentState);
            machine.Trigger(ACTION);
            Assert.Equal(GREEN, machine.CurrentState);
        }
    }
}
