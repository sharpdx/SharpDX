// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#if NET35Plus

namespace SharpDX.Toolkit
{
    using System;
    using Graphics;

    /// <summary>
    /// Helper extensions to facilitate usage of various functionality.
    /// </summary>
    public static class HelperExtensions
    {
        /// <summary>
        /// Token used to mark the end of an graphics event during profiling sessions.
        /// </summary>
        public struct EventEndToken : IDisposable
        {
            private readonly GraphicsPerformance performance;

            /// <summary>
            /// Initializes a new instance of this <see cref="EventEndToken"/> structure.
            /// </summary>
            /// <param name="performance">The associated performance helper.</param>
            /// <param name="message">The event message.</param>
            /// <remarks>The start of event is called immediately.</remarks>
            /// <exception cref="ArgumentNullException">Is thrown when <see cref="performance"/> is null.</exception>
            public EventEndToken(GraphicsPerformance performance, string message)
            {
                if (performance == null) throw new ArgumentNullException("performance");

                this.performance = performance;

                this.performance.Begin(message);
            }

            /// <summary>
            /// Ends the associated performance event.
            /// </summary>
            public void Dispose()
            {
                performance.End();
            }
        }

        /// <summary>
        /// Creates a performance event message.
        /// </summary>
        /// <param name="performance">The associated performance helper.</param>
        /// <param name="message">The event message.</param>
        /// <returns>The event token which marks the event ending when disposed.</returns>
        public static EventEndToken CreateEvent(this GraphicsPerformance performance, string message)
        {
            return new EventEndToken(performance, message);
        }

        /// <summary>
        /// Gets a service of required type from an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The service type to retrieve.</typeparam>
        /// <param name="registry">The registry containing the needed service.</param>
        /// <returns>The service instance.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="registry"/> is null or doesn't contain the needed service.</exception>
        public static T GetService<T>(this IServiceProvider registry)
            where T : class
        {
            if (registry == null) throw new ArgumentNullException("registry");

            var service = (T)registry.GetService(typeof(T));
            if (service == null)
                throw new ArgumentNullException(string.Format("Service of type {0} is not registered.", typeof(T)));

            return service;
        }

        /// <summary>
        /// Gets a service of required type from a <see cref="GameSystem"/> using its <see cref="GameSystem.Services"/>.
        /// </summary>
        /// <typeparam name="T">The service type to retrieve.</typeparam>
        /// <param name="gameSystem">The game system which contains the needed service registry.</param>
        /// <returns>The service instance.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="gameSystem"/> is null or doesn't contain the needed service.</exception>
        public static T GetService<T>(this GameSystem gameSystem)
            where T : class
        {
            if(gameSystem == null) throw new ArgumentNullException("gameSystem");

            return GetService<T>(gameSystem.Services);
        }
    }
}

#endif