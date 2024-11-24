using StateNet.Info;

namespace StateNet
{
    public class StateMachine<S, A, C> where S : notnull, IComparable where A : notnull, IComparable
    {
        #region Factory
        internal StateMachine(S initialState, C? initialContext) {
            CurrentState = initialState;
            Context = initialContext;
        }

        public static Func<StateMachine<S, A, C>> Factory(Action<MutableStateMachine<S, A, C>> builder, S initialState, C? initialContext = default) => () =>
        {
            var machine = new MutableStateMachine<S, A, C>(initialState, initialContext);
            builder(machine);
            return machine;
        };

        #endregion

        #region API

        public void Trigger(A action)
        {
            // Check if machine reached a end state
            if (!states.ContainsKey(CurrentState)) return;
            var oldState = states[CurrentState];
            var oldStateName = CurrentState;

            // Check if the triggered action can trigger a transition
            if (!oldState.transitions.ContainsKey(action)) return;

            // Get the first valid transition
            var transition = oldState.GetTransitionByAction(this, action);
            if (transition == null) return; // If none is valid, abort

            CurrentState = transition.targetState; // Change current state

            // Get transition info
            TransitionInfo<S, A, C> transitionInfo = new() { FromState = oldStateName, ToState = transition.targetState, Via = action, Machine = this };

            // Trigger transition with context info (action and machine)
            transition.Transitate(transitionInfo);

            // Invoke state events
            oldState.InvokeOnExit(transitionInfo);
            states[CurrentState].InvokeOnEnter(transitionInfo);
        }

        public void SetContext(C context) {
            Context = context;
        }

        public void SetState(S state)
        {
            CurrentState = state;
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
        readonly protected Dictionary<S, State<S, A, C>> states = [];

        public C? Context { get; protected set; }

        #endregion
    }
}
