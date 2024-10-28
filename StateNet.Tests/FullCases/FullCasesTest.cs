namespace StateNet.Tests.FullCases
{
    public class FullCasesTest
    {
        private readonly IMachineTester[] cases = [
            new TrafficLightsCase()
        ];

        [Fact]
        public void TestStates()
        {
            foreach (var c in cases) c.TestStates();
        }

        [Fact]
        public void TestTransitions()
        {
            foreach (var c in cases) c.TestTransitions();
        }
    }
}
