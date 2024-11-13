﻿namespace StateNet.Tests.FullCases
{
    public abstract class AMachineTester<S, T, C> : IMachineTester where T : notnull where S : notnull where C : notnull
    {
        public void TestStates()
        {
            var machine = GetMachine();

            Assert.Equal(machine.GetStates(), GetStates());
        }

        protected StateMachine<S, T, C> GetMachine() => GetMachineBlueprint()(GetInitialState(), GetInitialContext());

        // To implement

        protected abstract Func<S, C, StateMachine<S, T, C>> GetMachineBlueprint();
        protected abstract S[] GetStates();
        protected abstract S GetInitialState();
        protected abstract C GetInitialContext();
        public abstract void TestTransitions();
        public abstract void TestEvents();
    }
}
