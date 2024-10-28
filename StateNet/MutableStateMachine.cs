namespace StateNet
{
    public class MutableStateMachine<S, T> : StateMachine<S, T> where S : notnull where T : notnull
    {
        #region Factory
        internal MutableStateMachine(S initialState) : base(initialState) {}

        public static new Func<S, MutableStateMachine<S,T>> Factory(Action<MutableStateMachine<S,T>> builder) => (S initialState) =>
        {
            var machine = new MutableStateMachine<S, T>(initialState);
            builder(machine);
            return machine;
        };

        #endregion

        #region API.Build
        public State<S, T> AddState(S stateName, Dictionary<T, Transition<S>>? transitions = null)
        {
            var state = new State<S, T>(transitions);
            states[stateName] = state;
            return state;
        }

        public void RemoveState(S stateName) => states.Remove(stateName);

        #endregion
    }
}
