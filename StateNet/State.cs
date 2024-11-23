using StateNet.Info;

namespace StateNet
{
    public class State<S, T, C> where S : IComparable where T : IComparable
    {
        public delegate void OnStateEvent(TransitionInfo<S,T,C> transitionInfo);

        public readonly Dictionary<T, List<Transition<S, T, C>>> transitions = [];
        internal State(Dictionary<T, List<Transition<S, T, C>>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null) {
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
            if (!transitions.ContainsKey(action)) transitions[action] = [];
            transitions[action].Add(transition);
            return this;
        }

        public State<S, T, C> RemoveTransitions(T action)
        {
            transitions.Remove(action);
            return this;
        }

        internal List<Transition<S, T, C>> GetSortedActionTransitionsList(T action) => [.. transitions[action].OrderByDescending(item => item.IsConditional())];

        internal Transition<S, T, C>? GetTransitionByAction(StateMachine<S, T, C> machine, T action)
        {
            foreach (var transition in GetSortedActionTransitionsList(action))
            {
                var transitionInfo = new TransitionInfo<S, T, C> {
                    Via = action,
                    Machine = machine,
                    ToState = transition.targetState,
                    FromState = machine.CurrentState,
                };

                var evaluated = transition.Evaluate(transitionInfo);
                if (!transition.IsConditional()) return transition;
            }

            return null;
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
