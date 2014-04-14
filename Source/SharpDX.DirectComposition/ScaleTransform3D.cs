using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class ScaleTransform3D
	{
		public ScaleTransform3D(Device device) : base(IntPtr.Zero)
		{
			device.CreateScaleTransform3D(this);
		}

		public ScaleTransform3D(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateScaleTransform3D(this);
		}
	}
}
