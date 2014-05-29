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

using SharpDX.Direct3D;
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Provides helper methods to send event marker to Graphics Debuggers
    /// (Visual Studio, NSight, IntelGPA, GPUPerfStudio... etc.)
    /// </summary>
    public class GraphicsPerformance
    {
        private readonly GraphicsDevice device;

        private bool allowProfiling;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPerformance"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal GraphicsPerformance(GraphicsDevice device)
        {
            // currently not used, but will be used by Direct3D11.1 annotation API.
            this.device = device;

            // By default, profiling is authorized.
            allowProfiling = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this program give permission to be profiled, by default true.
        /// </summary>
        /// <value><c>true</c> if this program give permission to be profiled; otherwise, <c>false</c>.</value>
        public bool AllowProfiling
        {
            get
            {
                return allowProfiling;
            }
            set
            {
#if !W8CORE
                PixHelper.AllowProfiling(value);
#endif
                allowProfiling = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GraphicsPerformance"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Begins a section of profiling.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Begin(string name)
        {
            if (!Enabled) return;
            Begin(Color.Green, name);
        }

        /// <summary>
        /// Begins a section of profiling.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="name">The name.</param>
        public void Begin(Color color, string name)
        {
            if (!Enabled) return;
#if !W8CORE
            PixHelper.BeginEvent(color, name);
#endif
            // TODO Add code for Direct3D11.1 annotation API
        }

        /// <summary>
        /// Begins a section of profiling.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="formatName">Name of the format.</param>
        /// <param name="parameters">The parameters.</param>
        public void Begin(Color color, string formatName, params object[] parameters)
        {
            if (!Enabled) return;
#if !W8CORE
            PixHelper.BeginEvent(color, formatName, parameters);
#endif
            // TODO Add code for Direct3D11.1 annotation API
        }

        /// <summary>
        /// Sets a profiling marker.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetMarker(string name)
        {
            if (!Enabled) return;
            SetMarker(Color.Black, name);
        }

        /// <summary>
        /// Sets a profiling marker.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="name">The name.</param>
        public void SetMarker(Color color, string name)
        {
            if (!Enabled) return;
#if !W8CORE
            PixHelper.SetMarker(color, name);
#endif
            // TODO Add code for Direct3D11.1 annotation API
        }

        /// <summary>
        /// Sets a profiling marker.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="formatName">Name of the format.</param>
        /// <param name="parameters">The parameters.</param>
        public void SetMarker(Color color, string formatName, params object[] parameters)
        {
            if (!Enabled) return;
#if !W8CORE
            PixHelper.SetMarker(color, formatName, parameters);
#endif
            // TODO Add code for Direct3D11.1 annotation API
        }

        /// <summary>
        /// Ends the current section of profiling (must match with a begin).
        /// </summary>
        public void End()
        {
            if (!Enabled) return;
#if !W8CORE
            PixHelper.EndEvent();
#endif
            // TODO Add code for Direct3D11.1 annotation API
        }
    }
}