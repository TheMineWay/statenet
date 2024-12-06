namespace StateNet.Tests
{
    public class AnyStateTest
    {
        [Fact]
        public void TestTransition()
        {
            var (machineBlueprint, states) = GetBasicMachine();
            var machine = machineBlueprint();

            Assert.Equal(machine.GetStates(), states);
            Assert.Equal(states[0], machine.CurrentState);

            // Do some loops
            for (byte i = 0; i < 1; i++)
            {
                machine.Trigger("switch");
                Assert.Equal(states[1], machine.CurrentState);

                machine.Trigger("switch");
                Assert.Equal(states[2], machine.CurrentState);
            }

            // Being in state2:
            machine.Trigger("reset");
            Assert.Equal(states[0], machine.CurrentState);
        }

        [Fact]
        public void TestTransitionPriorities()
        {
            Func<StateMachine<string, string, string>> Generate(ConditionCase conditionCase)
            {
                return StateMachine<string, string, string>.Factory((builder) =>
                {
                    var s1 = builder.AddState("s1");
                    var s2 = builder.AddState("s2");
                    var s3 = builder.AddState("s3");
                    var s4 = builder.AddState("s4");
                    var preferentS4 = builder.AddState("preferent-s4");

                    // We define some transitions (from s1 to all others)
                    s1.AddTransition("t-s2", s2);
                    s1.AddTransition("t-s3", s3);
                    s1.AddTransition("t-s4", s4);

                    // We define redundant transitions to test if order is stable
                    var unpreferentTransition = s1.AddTransition("r-s4", s4);
                    var preferentCondition = builder.AnyState().AddTransition("r-s4", preferentS4);

                    // We define a reset transition from any state to s1
                    var resetTransition = builder.AnyState().AddTransition("reset", s1);

                    // Events
                    s1.OnEnter((info) =>
                    {
                        if (info.FromState == "s1") Assert.Fail("Cannot perform transitions to itself");
                    });

                    // Add condition if needed. This makes it preferent, but as we are testing AnyState transitions this won't be prefered over AnyState transitions.
                    if (conditionCase.unpreferentCondition != null) unpreferentTransition.When((info) => conditionCase.unpreferentCondition());
                    if (conditionCase.preferentCondition != null) preferentCondition.When((info) => conditionCase.preferentCondition());
                }, "s1");
            }

            foreach (var conditionCase in new List<ConditionCase>() {
                new(),
                new()
                {
                    unpreferentCondition = () => true,
                },
                new()
                {
                    unpreferentCondition = () => false,
                },
                new()
                {
                    preferentCondition = () => true,
                },
                new()
                {
                    preferentCondition = () => false,
                    targets = "s4",
                },
                new()
                {
                    unpreferentCondition = () => true,
                    preferentCondition = () => true,
                },
                new()
                {
                    unpreferentCondition = () => false,
                    preferentCondition = () => false,
                    targets = "s1"
                }
            })
            {
                var machineBlueprint = Generate(conditionCase);

                var machine = machineBlueprint();

                Assert.Equal("s1", machine.CurrentState);
                machine.Trigger("reset");

                machine.Trigger("t-s2");
                Assert.Equal("s2", machine.CurrentState);

                machine.Trigger("reset");
                Assert.Equal("s1", machine.CurrentState);

                // Test priority

                machine.Trigger("r-s4");
                Assert.Equal(conditionCase.targets, machine.CurrentState);
            }
        }

        #region Utils

        private (Func<StateMachine<string, string, string>>, string[]) GetBasicMachine()
        {
            string[] states = ["initial", "state1", "state2"];
            var machineBlueprint = StateMachine<string, string, string>.Factory((builder) =>
            {
                var initial = builder.AddState(states[0]);
                var s1 = builder.AddState(states[1]);
                var s2 = builder.AddState(states[2]);

                initial.AddTransition("switch", s1);

                // Define loop
                s1.AddTransition("switch", s2);
                s2.AddTransition("switch", s1);

                builder.AnyState().AddTransition("reset", "initial");

            }, states[0]);

            return (machineBlueprint, states);
        }

        #endregion

        struct ConditionCase
        {
            public Func<bool>? unpreferentCondition = null;
            public Func<bool>? preferentCondition = null;
            public string targets = "preferent-s4";

            public ConditionCase() {}
        }
    }
}