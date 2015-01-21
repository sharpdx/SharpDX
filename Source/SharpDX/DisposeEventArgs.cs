using System;

namespace SharpDX
{
    /// <summary>
    /// Event args which can tell whether calling Dispose with dispoing flag or not.
    /// </summary>
    public class DisposeEventArgs : EventArgs
    {
        /// <summary>
        /// DisposeEventArgs with Disposing flag set to true.
        /// </summary>
        public static readonly DisposeEventArgs DisposingEventArgs = new DisposeEventArgs(true);

        /// <summary>
        /// DisposeEventArgs with Disposing flag set to false.
        /// </summary>
        public static readonly DisposeEventArgs NotDisposingEventArgs = new DisposeEventArgs(false);

        /// <summary>
        /// True when disposing, otherwise false.
        /// </summary>
        public readonly bool Disposing;

        /// <summary>
        /// Initializes a new instance of a DisposeEventArgs class.
        /// </summary>
        /// <param name="disposing">True when disposing, otherwise false.</param>
        private DisposeEventArgs(bool disposing)
        {
            Disposing = disposing;
        }

        /// <summary>
        /// Gets event args base on disposing parameter.
        /// </summary>
        /// <param name="disposing">True when disposing, otherwise false.</param>
        /// <returns>DisposeEventArgs object based on disposing parameter.</returns>
        public static DisposeEventArgs Get(bool disposing)
        {
            return disposing ? DisposingEventArgs : NotDisposingEventArgs;
        }
    }
}
