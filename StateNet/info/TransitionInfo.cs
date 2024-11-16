namespace StateNet.Info
{
    public struct TransitionInfo<S, A, C> where S : notnull where A : notnull where C : notnull
    {
        public S FromState { get; internal set; }
        public S ToState { get; internal set; }
        public A Via { get; internal set; }
        public StateMachine<S, A, C> Machine { get; internal set; }
    }
}
