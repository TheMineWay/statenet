namespace StateNet
{
    public class State<S, T> where S : notnull where T : notnull
    {
        public delegate void OnStateEvent(T fromAction);

        public readonly Dictionary<T, Transition<S>> transitions = [];
        internal State(Dictionary<T, Transition<S>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null) {
            if (transitions != null) this.transitions = transitions;

            // Register default events
            if (onEnter != null) this.onEnter += onEnter;
            if (onExit != null) this.onExit += onExit;
        }

        #region Events

        private OnStateEvent? onEnter;
        internal void InvokeOnEnter(T fromAction) => onEnter?.Invoke(fromAction);

        private OnStateEvent? onExit;
        internal void InvokeOnExit(T fromAction) => onExit?.Invoke(fromAction);

        #endregion

        #region API

        public State<S, T> AddTransition(T action, S targetStateName) => AddTransition(action, new Transition<S>(targetStateName));
        public State<S, T> AddTransition(T action, Transition<S> transition)
        {
            transitions[action] = transition;
            return this;
        }

        public State<S, T> RemoveTransition(T action)
        {
            transitions.Remove(action);
            return this;
        }

        // State API

        public State<S, T> OnState(OnStateEvent onState)
        {
            this.onEnter += onState;
            return this;
        }

        #endregion
    }
}
