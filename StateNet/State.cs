using StateNet.Info;

namespace StateNet
{
    public class State<S, A, C> where S : notnull, IComparable where A : notnull, IComparable
    {
        public delegate void OnStateEvent(TransitionInfo<S,A,C> transitionInfo);

        public readonly Dictionary<A, List<Transition<S, A, C>>> transitions = [];
        internal State(Dictionary<A, List<Transition<S, A, C>>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null) {
            if (transitions != null) this.transitions = transitions;

            // Register default events
            if (onEnter != null) this.onEnter += onEnter;
            if (onExit != null) this.onExit += onExit;
        }

        #region Events

        private OnStateEvent? onEnter;
        internal void InvokeOnEnter(TransitionInfo<S, A, C> transitionInfo) => onEnter?.Invoke(transitionInfo);

        private OnStateEvent? onExit;
        internal void InvokeOnExit(TransitionInfo<S, A, C> transitionInfo) => onExit?.Invoke(transitionInfo);

        #endregion

        #region API

        public Transition<S, A, C> AddTransition(A action, S targetStateName) => AddTransition(action, new Transition<S, A, C>(targetStateName));
        public Transition<S, A, C> AddTransition(A action, Transition<S, A, C> transition)
        {
            if (!transitions.ContainsKey(action)) transitions[action] = [];
            transitions[action].Add(transition);
            return transition;
        }

        public State<S, A, C> RemoveTransitions(A action)
        {
            transitions.Remove(action);
            return this;
        }

        internal List<Transition<S, A, C>> GetSortedActionTransitionsList(A action) => [.. transitions[action].OrderByDescending(item => item.IsConditional())];

        internal Transition<S, A, C>? GetTransitionByAction(StateMachine<S, A, C> machine, A action)
        {
            foreach (var transition in GetSortedActionTransitionsList(action))
            {
                if (!transition.IsConditional()) return transition;

                var transitionInfo = new TransitionInfo<S, A, C>
                {
                    Via = action,
                    Machine = machine,
                    ToState = transition.targetState,
                    FromState = machine.CurrentState,
                };
                if(transition.Evaluate(transitionInfo)) return transition;
            }

            return null;
        }

        // State API

        public State<S, A, C> OnEnter(OnStateEvent onEnter)
        {
            this.onEnter += onEnter;
            return this;
        }

        public State<S, A, C> OnExit(OnStateEvent onExit)
        {
            this.onExit += onExit;
            return this;
        }

        #endregion
    }
}
