namespace StateNet
{
    public class StateMachine<S, T, C> where S : notnull where T : notnull where C : notnull
    {
        #region Factory
        internal StateMachine(S initialState, C initialContext) {
            CurrentState = initialState;
            Context = initialContext;
        }

        public static Func<S, C, StateMachine<S, T, C>> Factory(Action<MutableStateMachine<S, T, C>> builder) => (S initialState, C initialContext) =>
        {
            var machine = new MutableStateMachine<S, T, C>(initialState, initialContext);
            builder(machine);
            return machine;
        };

        #endregion

        #region API

        public void Trigger(T action)
        {
            // Check if machine reached a end state
            if (!states.ContainsKey(CurrentState)) return;
            var oldState = states[CurrentState];

            // Check if the triggered action can trigger a transition
            if (!oldState.transitions.ContainsKey(action)) return;
            var transition = oldState.transitions[action];

            // Trigger transition
            transition.Transitate(CurrentState);
            CurrentState = transition.targetState; // CHange current state

            // Invoke state events
            oldState.InvokeOnExit(action);
            states[CurrentState].InvokeOnEnter(action);
        }

        // Info API

        public S[] GetStates() => [.. states.Keys];

        #endregion

        #region Info

        public S CurrentState { get; protected set; }
        readonly protected Dictionary<S, State<S,T>> states = [];

        public C Context { get; protected set; }

        #endregion
    }
}
