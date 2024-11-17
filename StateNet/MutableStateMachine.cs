namespace StateNet
{
    public class MutableStateMachine<S, T, C> : StateMachine<S, T, C> where S : notnull where T : notnull
    {
        #region Factory
        internal MutableStateMachine(S initialState, C initialContext) : base(initialState, initialContext) {}

        public static new Func<S, C, MutableStateMachine<S,T, C>> Factory(Action<MutableStateMachine<S, T, C>> builder) => (S initialState, C initialContext) =>
        {
            var machine = new MutableStateMachine<S, T, C>(initialState, initialContext);
            builder(machine);
            return machine;
        };

        #endregion

        #region API.Build
        public State<S, T, C> AddState(S stateName, Dictionary<T, Transition<S, T, C>>? transitions = null)
        {
            var state = new State<S, T, C>(transitions);
            states[stateName] = state;
            return state;
        }

        public void RemoveState(S stateName) => states.Remove(stateName);

        #endregion
    }
}
