namespace StateNet.Tests.FullCases
{
    public abstract class AMachineTester<S, T> : IMachineTester where T : notnull where S : notnull
    {
        public void TestStates()
        {
            var machine = GetMachine();

            Assert.Equal(machine.GetStates(), GetStates());
        }

        protected StateMachine<S, T> GetMachine() => GetMachineBlueprint()(GetInitialState());

        // To implement

        protected abstract Func<S, StateMachine<S, T>> GetMachineBlueprint();
        protected abstract S[] GetStates();
        protected abstract S GetInitialState();
        public abstract void TestTransitions();
        public abstract void TestEvents();
    }
}
