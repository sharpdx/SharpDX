using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class SurfaceFactory
	{
		public SurfaceFactory(Device2 device, ComObject renderingDevice) : base(IntPtr.Zero)
		{
			device.CreateSurfaceFactory(renderingDevice, this);
		}
	}
}
