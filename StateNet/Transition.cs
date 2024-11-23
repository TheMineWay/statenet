using StateNet.Info;

namespace StateNet {
    public class Transition<S, A, C> where S : IComparable where A : IComparable
    {
        public delegate void OnTransitionEvent(TransitionInfo<S, A, C> transitionInfo);

        public readonly S targetState;

        public Transition( S targetState, OnTransitionEvent? onTransition = null) {
            this.targetState = targetState;

            // Register default events
            if (onTransition != null) this.onTransition += onTransition;
        }

        #region Events

        private OnTransitionEvent? onTransition;
        private void InvokeOnTransition(TransitionInfo<S,A,C> transitionInfo) => onTransition?.Invoke(transitionInfo);

        #endregion

        #region API

        internal void Transitate(TransitionInfo<S, A, C> transitionInfo)
        {
            InvokeOnTransition(transitionInfo);
        }

        public bool IsConditional() => false;

        #endregion
    }
}
