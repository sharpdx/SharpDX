using System;

namespace SharpDX
{
    /// <summary>
    /// Base class for a <see cref="IDisposable"/> class.
    /// </summary>
    public abstract class DisposeBase : IDisposable
    {
        protected bool IsDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposeBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposeBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // TODO Should we throw an exception if this method is called more than once?
            if (!IsDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);
    }
}