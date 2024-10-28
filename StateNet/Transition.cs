namespace StateNet {
    public class Transition<S> where S : notnull
    {
        public delegate void OnTransitionEvent(S fromState, S toState);

        public readonly S targetState;

        public Transition(S targetState, OnTransitionEvent? onTransition = null) {
            this.targetState = targetState;

            // Register default events
            if (onTransition != null) this.onTransition += onTransition;
        }

        #region Events

        private OnTransitionEvent? onTransition;
        internal void InvokeOnTransition(S fromState) => onTransition?.Invoke(fromState, targetState);

        #endregion

        #region API

        public void Transitate(S fromState)
        {
            InvokeOnTransition(fromState);
        }

        #endregion
    }
}
