using StateNet.States;

namespace StateNet
{
    public class MutableStateMachine<S, A, C> : StateMachine<S, A, C> where S : notnull, IComparable where A : notnull, IComparable
    {
        #region Factory
        internal MutableStateMachine(S initialState, C? initialContext) : base(initialState, initialContext) {}

        public static new Func<MutableStateMachine<S, A, C>> Factory(Action<MutableStateMachine<S, A, C>> builder, S initialState, C? initialContext = default) => () =>
        {
            var machine = new MutableStateMachine<S, A, C>(initialState, initialContext);
            builder(machine);
            return machine;
        };

        #endregion

        #region API.Build
        public State<S, A, C> AddState(S stateName, Dictionary<A, List<Transition<S, A, C>>>? transitions = null)
        {
            var state = new State<S, A, C>(stateName, transitions);
            states[stateName] = state;
            return state;
        }

        public void RemoveState(S stateName) => states.Remove(stateName);

        #endregion
    }
}
