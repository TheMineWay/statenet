using StateNet.Info;

namespace StateNet.States
{
    public class AnonymousState<S, A, C> where S : notnull, IComparable where A : notnull, IComparable
    {

        public delegate void OnStateEvent(TransitionInfo<S, A, C> transitionInfo);

        public readonly Dictionary<A, List<Transition<S, A, C>>> transitions = [];
        internal AnonymousState(Dictionary<A, List<Transition<S, A, C>>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null)
        {
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

        public Transition<S, A, C> AddTransition(A action, State<S, A, C> targetState) => AddTransition(action, new Transition<S, A, C>(targetState.name));
        public Transition<S, A, C> AddTransition(A action, S targetStateName) => AddTransition(action, new Transition<S, A, C>(targetStateName));
        public Transition<S, A, C> AddTransition(A action, Transition<S, A, C> transition)
        {
            if (!transitions.ContainsKey(action)) transitions[action] = [];
            transitions[action].Add(transition);
            return transition;
        }

        public AnonymousState<S, A, C> RemoveTransitions(A action)
        {
            transitions.Remove(action);
            return this;
        }

        // State API

        public AnonymousState<S, A, C> OnEnter(OnStateEvent onEnter)
        {
            this.onEnter += onEnter;
            return this;
        }

        public AnonymousState<S, A, C> OnExit(OnStateEvent onExit)
        {
            this.onExit += onExit;
            return this;
        }

        public List<Transition<S, A, C>>? GetTransitions(A action) {
            transitions.TryGetValue(action, out List<Transition<S, A, C>>? value);
            return value;
        }

        #endregion

        #region Internal API

        /*
         * ORDER
         * #1: transitions from any state.
         * #2: transitions between defined states.
         * 
         * Each transitions group is sorted. First conditional transitions, then conditionless ones.
         */
        internal List<Transition<S, A, C>> GetSortedActionTransitionsList(StateMachine<S, A, C> machine, A action) {
            List<Transition<S, A, C>> tr = [];

            transitions.TryGetValue(action, out var stateTransitions);
            if (stateTransitions != null)
            {
                tr.AddRange(stateTransitions.OrderByDescending(item => item.IsConditional()));
            }

            if (machine.AnyState().GetTransitions(action)?.Count > 0)
            {
                tr = [.. machine.AnyState().GetTransitions(action)?.OrderByDescending(item => item.IsConditional()), ..tr];
            }

            return tr;
        }

        internal Transition<S, A, C>? GetTransitionByAction(StateMachine<S, A, C> machine, A action)
        {
            foreach (var transition in GetSortedActionTransitionsList(machine, action))
            {
                if (!transition.IsConditional()) return transition;

                var transitionInfo = new TransitionInfo<S, A, C>
                {
                    Via = action,
                    Machine = machine,
                    ToState = transition.targetState,
                    FromState = machine.CurrentState,
                };
                if (transition.Evaluate(transitionInfo)) return transition;
            }

            return null;
        }

        #endregion
    }
}
