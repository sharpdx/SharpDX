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

using System;
using System.Collections.Generic;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Provides methods to retrieve and manipulate graphics adapters. This is the equivalent to <see cref="Adapter1"/>.
    /// </summary>
    /// <msdn-id>ff471329</msdn-id>	
    /// <unmanaged>IDXGIAdapter1</unmanaged>	
    /// <unmanaged-short>IDXGIAdapter1</unmanaged-short>	
    public class GraphicsAdapter : Component
    {
        private static DisposeCollector staticCollector;
        private readonly Adapter1 adapter;
        private readonly int adapterOrdinal;

        private readonly GraphicsOutput[] outputs1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsAdapter" /> class.
        /// </summary>
        /// <param name="adapterOrdinal">The adapter ordinal.</param>
        private GraphicsAdapter(int adapterOrdinal)
        {
            this.adapterOrdinal = adapterOrdinal;
            adapter = ToDispose(Factory.GetAdapter1(adapterOrdinal));
            Description = adapter.Description1;
            var outputs = adapter.Outputs;

            outputs1 = new GraphicsOutput[outputs.Length];
            for (var i = 0; i < outputs.Length; i++)
                outputs1[i] = new GraphicsOutput(i, outputs[i]);
        }

        /// <summary>
        /// Initializes static members of the <see cref="GraphicsAdapter" /> class.
        /// </summary>
        static GraphicsAdapter()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the GraphicsAdapter. On Desktop and WinRT, this is done statically.
        /// </summary>
        public static void Initialize()
        {
            if (Adapters == null)
            {
#if DIRECTX11_1
            using (var factory = new Factory1()) Initialize(factory.QueryInterface<Factory2>());
#else
                Initialize(new Factory1());
#endif
            }
        }

        /// <summary>
        /// Dispose all statically cached value by this instance.
        /// </summary>
        public new static void Dispose()
        {
            Utilities.Dispose(ref staticCollector);
            Adapters = null;
            Default = null;
        }

        /// <summary>
        /// Initializes all adapters with the specified factory.
        /// </summary>
        /// <param name="factory1">The factory1.</param>
        internal static void Initialize(Factory1 factory1)
        {
            if (staticCollector != null)
            {
                staticCollector.Dispose();
            }

            staticCollector = new DisposeCollector();
            Factory = factory1;
            staticCollector.Collect(Factory);

            int countAdapters = Factory.GetAdapterCount1();
            var adapters = new List<GraphicsAdapter>();
            for (int i = 0; i < countAdapters; i++)
            {
                var adapter = new GraphicsAdapter(i);
                staticCollector.Collect(adapter);
                adapters.Add(adapter);
            }

            Default = adapters[0];
            Adapters = adapters.ToArray();
        }

        /// <summary>
        /// Collection of available adapters on the system.
        /// </summary>
        public static GraphicsAdapter[] Adapters { get; private set; }

        /// <summary>
        /// Gets the default adapter.
        /// </summary>
        public static GraphicsAdapter Default { get; private set; }

        /// <summary>
        /// Gets the number of <see cref="GraphicsOutput"/> attached to this <see cref="GraphicsAdapter"/>.
        /// </summary>
        public int OutputsCount { get { return outputs1.Length; } }

        /// <summary>
        /// Gets the description for this adapter.
        /// </summary>
        public readonly AdapterDescription1 Description;

        /// <summary>
        /// Default PixelFormat used.
        /// </summary>
        public readonly PixelFormat DefaultFormat = PixelFormat.R8G8B8A8.UNorm;

        /// <summary>
        /// Gets the <see cref="Factory1"/> used by all GraphicsAdapter.
        /// </summary>
        public static Factory1 Factory { get; private set; }

        /// <summary>
        /// Determines if this instance of GraphicsAdapter is the default adapter.
        /// </summary>
        public bool IsDefaultAdapter { get { return adapterOrdinal == 0; } }

        /// <summary>
        /// Gets the current display mode.
        /// </summary>
        /// <value>The current display mode.</value>
        [Obsolete("Use 'GetOutputAt' method to get specific output and retrieve needed information from it.")]
        public DisplayMode CurrentDisplayMode { get { return outputs1[0].CurrentDisplayMode; } }

        /// <summary>
        /// Returns a collection of supported display modes for the current adapter.
        /// </summary>
        [Obsolete("Use 'GetOutputAt' method to get specific output and retrieve needed information from it.")]
        public DisplayMode[] SupportedDisplayModes { get { return outputs1[0].SupportedDisplayModes; } }

        /// <summary>
        /// Retrieves bounds of the desktop coordinates.
        /// </summary>
        /// <msdn-id>bb173068</msdn-id>	
        /// <unmanaged>RECT DesktopCoordinates</unmanaged>	
        /// <unmanaged-short>RECT DesktopCoordinates</unmanaged-short>	
        [Obsolete("Use 'GetOutputAt' method to get specific output and retrieve needed information from it.")]
        public Rectangle DesktopBounds { get { return outputs1[0].DesktopBounds; } }

        /// <summary>
        /// Retrieves the handle of the monitor associated with the Microsoft Direct3D object.
        /// </summary>
        /// <msdn-id>bb173068</msdn-id>	
        /// <unmanaged>HMONITOR Monitor</unmanaged>	
        /// <unmanaged-short>HMONITOR Monitor</unmanaged-short>	
        [Obsolete("Use 'GetOutputAt' method to get specific output and retrieve needed information from it.")]
        public IntPtr MonitorHandle { get { return outputs1[0].MonitorHandle; } }

        /// <summary>
        /// <see cref="Adapter1"/> casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Adapter1(GraphicsAdapter from)
        {
            return from.adapter;
        }

        /// <summary>
        /// <see cref="Adapter1"/> casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Factory1(GraphicsAdapter from)
        {
            return Factory;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsOutput"/> attached to this adapter at the specified index.
        /// </summary>
        /// <param name="index">The index of the output to get.</param>
        /// <returns>The <see cref="GraphicsOutput"/> at the specified <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="index"/> is less than zero or greater or equal to <see cref="OutputsCount"/>.</exception>
        public GraphicsOutput GetOutputAt(int index)
        {
            if (index < 0 || index >= OutputsCount)
                throw new ArgumentOutOfRangeException("index");

            return outputs1[index];
        }

        /// <summary>
        /// Tests to see if the adapter supports the requested profile.
        /// </summary>
        /// <param name="featureLevel">The graphics profile.</param>
        /// <returns>true if the profile is supported</returns>
        public bool IsProfileSupported(FeatureLevel featureLevel)
        {
            return Direct3D11.Device.IsSupportedFeatureLevel(this, featureLevel);
        }

        /// <summary>
        /// Return the description of this adapter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description.Description;
        }
    }
}