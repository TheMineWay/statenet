using StateNet.info;

namespace StateNet
{
    public class State<S, T, C> where S : notnull where T : notnull where C : notnull
    {
        public delegate void OnStateEvent(TransitionInfo<S,T,C> transitionInfo);

        public readonly Dictionary<T, Transition<S, T, C>> transitions = [];
        internal State(Dictionary<T, Transition<S, T, C>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null) {
            if (transitions != null) this.transitions = transitions;

            // Register default events
            if (onEnter != null) this.onEnter += onEnter;
            if (onExit != null) this.onExit += onExit;
        }

        #region Events

        private OnStateEvent? onEnter;
        internal void InvokeOnEnter(TransitionInfo<S, T, C> transitionInfo) => onEnter?.Invoke(transitionInfo);

        private OnStateEvent? onExit;
        internal void InvokeOnExit(TransitionInfo<S, T, C> transitionInfo) => onExit?.Invoke(transitionInfo);

        #endregion

        #region API

        public State<S, T, C> AddTransition(T action, S targetStateName) => AddTransition(action, new Transition<S, T, C>(targetStateName));
        public State<S, T, C> AddTransition(T action, Transition<S, T, C> transition)
        {
            transitions[action] = transition;
            return this;
        }

        public State<S, T, C> RemoveTransition(T action)
        {
            transitions.Remove(action);
            return this;
        }

        // State API

        public State<S, T, C> OnEnter(OnStateEvent onEnter)
        {
            this.onEnter += onEnter;
            return this;
        }

        public State<S, T, C> OnExit(OnStateEvent onExit)
        {
            this.onExit += onExit;
            return this;
        }

        #endregion
    }
}
