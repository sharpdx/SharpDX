// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

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
        private static readonly DisposableCollection StaticCollector;
        private readonly Adapter1 adapter;
        private readonly int adapterOrdinal;
        //private AdapterDescription1 adapterDescription;
        private Output[] outputs;
        private OutputDescription outputDescription;

        /// <summary>
        /// Default PixelFormat used.
        /// </summary>
        public PixelFormat DefaultFormat = PixelFormat.R8G8B8A8.UNorm;

        /// <summary>
        /// Static initializing of GraphicsAdapter.
        /// </summary>
        static GraphicsAdapter()
        {
            StaticCollector = new DisposableCollection();

            // DXGI Factory
            Factory = new Factory1();
            StaticCollector.Add(Factory);

            int countAdapters = Factory.GetAdapterCount1();
            var adapters = new List<GraphicsAdapter>();
            for (int i = 0; i < countAdapters; i++)
            {
                var adapter = new GraphicsAdapter(i);
                StaticCollector.Add(adapter);
                adapters.Add(adapter);
            }

            Default = adapters[0];
            Adapters = adapters.ToArray();            
        }

        /// <summary>
        /// Constructor from adapter ordinal
        /// </summary>
        /// <param name="adapterOrdinal"></param>
        private GraphicsAdapter(int adapterOrdinal)
        {
            this.adapterOrdinal = adapterOrdinal;
            adapter = ToDispose(Factory.GetAdapter1(adapterOrdinal));
            Description = adapter.Description1;
            outputs = adapter.Outputs;
            SupportedDisplayModes = new DisplayMode[0];
            Initialize();
        }

        private void Initialize()
        {
            if (outputs != null && outputs.Length > 0)
            {
                outputDescription = outputs[0].Description;
                SupportedDisplayModes = GetSupportedDisplayModes();
                foreach (var output in outputs)
                    ToDispose(output);
            }
        }

        /// <summary>
        /// Gets the <see cref="Factory1"/> used by all GraphicsAdapter.
        /// </summary>
        public static readonly SharpDX.DXGI.Factory1 Factory;

        /// <summary>
        /// Return the description of this adapter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description.Description;
        }

        /// <summary>
        /// Tests to see if the adapter supports the requested profile.
        /// </summary>
        /// <param name="featureLevel">The graphics profile.</param>
        /// <returns>true if the profile is supported</returns>
        public bool IsProfileSupported(FeatureLevel featureLevel)
        {
            // Only way is to instantiate a Direct3D11 device and check the selected
            // feature level
            bool isProfileSupported = false;

            Direct3D11.Device device = null;
            try
            {
                device = new SharpDX.Direct3D11.Device(adapter, DeviceCreationFlags.None, featureLevel);
                isProfileSupported = featureLevel == device.FeatureLevel;
            }
            catch (SharpDXException) { }
            finally
            {
                if (device != null)
                    device.Dispose();
            }
            return isProfileSupported;
        }

        /// <summary>
        /// Collection of available adapters on the system.
        /// </summary>
        public static readonly GraphicsAdapter[] Adapters;

        /// <summary>
        /// Gets the default adapter.
        /// </summary>
        public static readonly GraphicsAdapter Default;

        /// <summary>
        /// Returns a collection of supported display modes for the current adapter.
        /// </summary>
        public DisplayMode[] SupportedDisplayModes { get; private set; }

        /// <summary>
        /// Gets the description for this adapter.
        /// </summary>
        public readonly AdapterDescription1 Description;

        /// <summary>
        /// Retrieves bounds of the desktop coordinates.
        /// </summary>
        /// <msdn-id>bb173068</msdn-id>	
        /// <unmanaged>RECT DesktopCoordinates</unmanaged>	
        /// <unmanaged-short>RECT DesktopCoordinates</unmanaged-short>	
        public Rectangle DesktopBounds { get { return outputDescription.DesktopBounds; } }

        /// <summary>
        /// Determines if this instance of GraphicsAdapter is the default adapter.
        /// </summary>
        public bool IsDefaultAdapter { get { return adapterOrdinal == 0; } }

        /// <summary>
        /// Retrieves the handle of the monitor associated with the Microsoft Direct3D object.
        /// </summary>
        /// <msdn-id>bb173068</msdn-id>	
        /// <unmanaged>HMONITOR Monitor</unmanaged>	
        /// <unmanaged-short>HMONITOR Monitor</unmanaged-short>	
        public IntPtr MonitorHandle { get { return outputDescription.MonitorHandle; } }

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
        /// Disposes of all objects
        /// </summary>
        internal static void DisposeStatic()
        {
            ((IDisposable)StaticCollector).Dispose();
        }

        /// <summary>
        /// Returns a collection of supported display modes for a particular Format.
        /// </summary>
        /// <returns>a read-only collection of display modes</returns>
        private DisplayMode[] GetSupportedDisplayModes()
        {
            var output = outputs[0];
            var modeAvailable = new List<DisplayMode>();
            var modeMap = new Dictionary<string, DisplayMode>();

            foreach (var format in Enum.GetValues(typeof(DXGI.Format)))
            {
                var modes = output.GetDisplayModeList((Format)format,
                                                      DisplayModeEnumerationFlags.Interlaced |
                                                      DisplayModeEnumerationFlags.Scaling);

                foreach (var mode in modes)
                {
                    if (mode.Scaling == DisplayModeScaling.Unspecified)
                    {
                        string key = format + ";" + mode.Width + ";" + mode.Height + ";" + mode.RefreshRate.Numerator + ";" + mode.RefreshRate.Denominator;

                        DisplayMode oldMode;
                        if (!modeMap.TryGetValue(key, out oldMode))
                        {
                            var displayMode = new DisplayMode(mode);

                            modeMap.Add(key, displayMode);
                            modeAvailable.Add(displayMode);
                        }
                    }
                }
            }
            return modeAvailable.ToArray();
        }
    }
}