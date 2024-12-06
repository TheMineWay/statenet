using StateNet.Info;
using StateNet.States;

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
            states.TryGetValue(CurrentState, out var oldState);

            // If current state is not defined, temporarily create a simulated state instance
            oldState ??= new(CurrentState);
            var oldStateName = CurrentState;

            // Get the first valid transition
            var transition = oldState.GetTransitionByAction(this, action);
            if (transition == null) return; // If none is valid, abort

            // Disable transitions to itself
            // TODO: this might be configurable in the future
            if (CurrentState.Equals(transition.targetState)) return;

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

        // From any state
        public AnonymousState<S, A, C> AnyState() => anyState;

        // Info API

        public S[] GetStates() => [.. states.Keys];

        #endregion

        #region Info

        public S CurrentState { get; protected set; }
        readonly protected Dictionary<S, State<S, A, C>> states = [];

        public C? Context { get; protected set; }

        protected readonly AnonymousState<S, A, C> anyState = new();

        #endregion
    }
}
