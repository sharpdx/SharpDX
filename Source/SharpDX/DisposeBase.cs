using System;

namespace SharpDX
{
    /// <summary>
    /// Base class for a <see cref="IDisposable"/> class.
    /// </summary>
    public abstract class DisposeBase : IDisposable
    {
        /// <summary>
        /// Occurs when this instance is starting to be disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposing;

        /// <summary>
        /// Occurs when this instance is fully disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CheckAndDispose(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        private void CheckAndDispose(bool disposing)
        {
            if (!IsDisposed)
            {
	            EventHandler<EventArgs> disposingHandlers = Disposing;
	            if (disposingHandlers != null) 
					disposingHandlers(this, EventArgs.Empty);

                Dispose(disposing);
                IsDisposed = true;

	            EventHandler<EventArgs> disposedHandlers = Disposed;
	            if (disposedHandlers != null) 
					disposedHandlers(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);
    }
}