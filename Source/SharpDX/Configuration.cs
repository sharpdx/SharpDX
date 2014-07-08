// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using SharpDX.Diagnostics;

namespace SharpDX
{
    /// <summary>
    /// Global configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Enables or disables object tracking. Default is disabled (false).
        /// </summary>
        /// <remarks>
        /// Object Tracking is used to track COM object lifecycle creation/dispose. When this option is enabled
        /// objects can be tracked using <see cref="ObjectTracker"/>. Using Object tracking has a significant
        /// impact on performance and should be used only while debugging.
        /// </remarks>
        public static bool EnableObjectTracking = false;

        /// <summary>
        /// Enables or disables release of <see cref="ComObject"/> on finalizer. Default is disabled (false).
        /// </summary>
        public static bool EnableReleaseOnFinalizer = false;

        /// <summary>
        /// Enables or disables writing a warning via <see cref="System.Diagnostics.Trace"/> if a <see cref="ComObject"/> was disposed in the finalizer. Default is enabled (true).
        /// </summary>
        public static bool EnableTrackingReleaseOnFinalizer = true;

        /// <summary>
        /// Throws a <see cref="CompilationException"/> when a shader or effect compilation error occurred. Default is enabled (true).
        /// </summary>
        public static bool ThrowOnShaderCompileError = true;

        /// <summary>
        /// By default all objects in the process are tracked.
        /// Use this property to track objects per thread.
        /// </summary>
        public static bool UseThreadStaticObjectTracking = false;
    }
}

