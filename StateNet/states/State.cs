namespace StateNet.States
{
    public class State<S, A, C> : AnonymousState<S, A, C> where S : notnull, IComparable where A : notnull, IComparable
    {
        public readonly S name; // <- State code (id)
        internal State(S name, Dictionary<A, List<Transition<S, A, C>>>? transitions = null, OnStateEvent? onEnter = null, OnStateEvent? onExit = null) : base(transitions, onEnter, onExit)
        {
            // Store state code
            this.name = name;
        }

        public new State<S, A, C> RemoveTransitions(A action)
        {
            base.RemoveTransitions(action);
            return this;
        }

        // State API

        public new State<S, A, C> OnEnter(OnStateEvent onEnter)
        {
            base.OnEnter(onEnter);
            return this;
        }

        public new State<S, A, C> OnExit(OnStateEvent onExit)
        {
            base.OnExit(onExit);
            return this;
        }
    }
}
