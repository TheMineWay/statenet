namespace StateNet.Tests.FullCases
{
    public abstract class AMachineTester<S, A, C> : IMachineTester where S : notnull, IComparable where A : notnull, IComparable
    {
        public void TestStates()
        {
            var machine = GetMachine();

            Assert.Equal(machine.GetStates(), GetStates());
        }

        protected StateMachine<S, A, C> GetMachine() => GetMachineBlueprint()();

        // To implement

        protected abstract Func<StateMachine<S, A, C>> GetMachineBlueprint();
        protected abstract S[] GetStates();
        protected abstract S GetInitialState();
        protected abstract C GetInitialContext();
        public abstract void TestTransitions();
        public abstract void TestEvents();
    }
}
