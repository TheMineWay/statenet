using StateNet.info;

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
            var oldStateName = CurrentState;

            // Check if the triggered action can trigger a transition
            if (!oldState.transitions.ContainsKey(action)) return;
            var transition = oldState.transitions[action];

            CurrentState = transition.targetState; // Change current state

            // Get transition info
            TransitionInfo<S, T, C> transitionInfo = new() { FromState = oldStateName, ToState = transition.targetState, Via = action, Machine = this };

            // Trigger transition with context info (action and machine)
            transition.Transitate(transitionInfo);

            // Invoke state events
            oldState.InvokeOnExit(transitionInfo);
            states[CurrentState].InvokeOnEnter(transitionInfo);
        }

        public void SetContext(C context) {
            Context = context;
        }

        public C MutateContext(Func<C, C> mutateFn)
        {
            SetContext(mutateFn(Context));
            return Context;
        }

        // Info API

        public S[] GetStates() => [.. states.Keys];

        #endregion

        #region Info

        public S CurrentState { get; protected set; }
        readonly protected Dictionary<S, State<S,T,C>> states = [];

        public C Context { get; protected set; }

        #endregion
    }
}
