namespace StateNet.Tests.FullCases
{
    public abstract class AMachineTester<S, T, C> : IMachineTester where S : IComparable where T : IComparable
    {
        public void TestStates()
        {
            var machine = GetMachine();

            Assert.Equal(machine.GetStates(), GetStates());
        }

        protected StateMachine<S, T, C> GetMachine() => GetMachineBlueprint()();

        // To implement

        protected abstract Func<StateMachine<S, T, C>> GetMachineBlueprint();
        protected abstract S[] GetStates();
        protected abstract S GetInitialState();
        protected abstract C GetInitialContext();
        public abstract void TestTransitions();
        public abstract void TestEvents();
    }
}
