﻿using StateNet.States;

namespace StateNet.Tests
{
    public class AnyStateTest
    {
        [Fact]
        public void TestTransition()
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

            var machine = machineBlueprint();

            Assert.Equal(machine.GetStates(), states);
            Assert.Equal(states[0], machine.CurrentState);

            // Do some loops
            for (byte i = 0; i < 2; i++)
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
    }
}