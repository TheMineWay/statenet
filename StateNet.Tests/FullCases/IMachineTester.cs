namespace StateNet.Tests.FullCases
{
    public interface IMachineTester
    {
        public void TestStates();
        public void TestTransitions();
        public void TestEvents();
    }
}
