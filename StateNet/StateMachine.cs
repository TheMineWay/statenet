namespace StateNet
{
    public class StateMachine<S, T> where S : notnull where T : notnull
    {
        #region Factory
        internal StateMachine(S initialState) {
            currentState = initialState;
        }

        public static Func<S, StateMachine<S, T>> Factory(Action<MutableStateMachine<S, T>> builder) => (S initialState) =>
        {
            var machine = new MutableStateMachine<S, T>(initialState);
            builder(machine);
            return machine;
        };

        #endregion

        #region API

        public void Trigger(T action)
        {
            // Check if machine reached a end state
            if (!states.ContainsKey(currentState)) return;
            var oldState = states[currentState];

            // Check if the triggered action can trigger a transition
            if (!oldState.transitions.ContainsKey(action)) return;
            var transition = oldState.transitions[action];

            // Trigger transition
            transition.Transitate(currentState);
            currentState = transition.targetState; // CHange current state

            // Invoke state events
            oldState.InvokeOnExit(action);
            states[currentState].InvokeOnEnter(action);
        }

        // Info API

        public S[] GetStates() => [.. states.Keys];

        #endregion

        #region Info

        public S currentState { get; protected set; }
        readonly protected Dictionary<S, State<S,T>> states = [];

        #endregion
    }
}
