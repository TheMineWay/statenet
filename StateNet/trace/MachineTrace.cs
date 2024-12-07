namespace StateNet.Trace
{
    public class MachineTrace<S, C> where S : notnull, IComparable
    {
        internal MachineTrace() { }

        // Info
        private readonly List<Trace<S, C>> traces = new();

        #region API

        public Trace<S, C>[] GetTraces() => traces.ToArray();
        public S[] GetStates() => GetTraces().Select((trace) => trace.state).ToArray();
        public C[] GetContexts() => GetTraces().Select((trace) => trace.context).ToArray();

        #endregion

        #region Internal API

        internal void AddTrace(Trace<S, C> trace)
        {
            traces.Add(trace);
        }

        #endregion
    }

    public struct Trace<S, C> where S : notnull, IComparable
    {
        public readonly S state;
        public readonly C context;

        public Trace(S state, C context)
        {
            this.state = state;
            this.context = context;
        }
    }
}
