namespace SharpDX.Toolkit.Input
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains collection of <see cref="PointerPoint"/>
    /// </summary>
    public sealed class PointerState
    {
        // this class can be reused several times to avoid garbage generation
        // it represents a future-proof wrapper around a simple list of pointer points

        /// <summary>
        /// Initializes a new instance of <see cref="PointerState"/> class
        /// </summary>
        public PointerState()
        {
            Points = new List<PointerPoint>();
        }

        // we expose a concrete List as it's Enumerator is a struct and iteration will not generate garbage,
        // however, the collection is mutable and it is responsibility of user-developer to avoid its change

        /// <summary>
        /// The collection of <see cref="PointerPoint"/> that were raised at some specific time
        /// </summary>
        public List<PointerPoint> Points { get; private set; } 
    }
}