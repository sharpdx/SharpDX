using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class VirtualSurface
	{
		public VirtualSurface(Device device, int initialWidth, int initialHeight, DXGI.Format format, DXGI.AlphaMode alphaMode) : base(IntPtr.Zero)
		{
			device.CreateVirtualSurface(initialWidth, initialHeight, format, alphaMode, this);
		}

		public VirtualSurface(Device2 device, int initialWidth, int initialHeight, DXGI.Format format, DXGI.AlphaMode alphaMode)
			: base(IntPtr.Zero)
		{
			device.CreateVirtualSurface(initialWidth, initialHeight, format, alphaMode, this);
		}

		public VirtualSurface(SurfaceFactory factory, int initialWidth, int initialHeight, DXGI.Format format, DXGI.AlphaMode alphaMode)
			: base(IntPtr.Zero)
		{
			factory.CreateVirtualSurface(initialWidth, initialHeight, format, alphaMode, this);
		}
	}
}
