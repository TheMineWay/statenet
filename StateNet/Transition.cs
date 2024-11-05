using StateNet.info;

namespace StateNet {
    public class Transition<S, A, C> where S : notnull where A : notnull where C : notnull
    {
        public delegate void OnTransitionEvent(TransitionInfo<S, A, C> transitionInfo);

        public readonly S targetState;

        public Transition(S targetState, OnTransitionEvent? onTransition = null) {
            this.targetState = targetState;

            // Register default events
            if (onTransition != null) this.onTransition += onTransition;
        }

        #region Events

        private OnTransitionEvent? onTransition;
        internal void InvokeOnTransition(S fromState, A via, StateMachine<S, A, C> machine) => onTransition?.Invoke(new() { FromState = fromState, ToState = targetState, Via = via, Machine = machine});

        #endregion

        #region API

        internal void Transitate(S oldState, A action, StateMachine<S, A, C> machine)
        {
            InvokeOnTransition(fromState: oldState, via: action, machine: machine);
        }

        #endregion
    }
}
