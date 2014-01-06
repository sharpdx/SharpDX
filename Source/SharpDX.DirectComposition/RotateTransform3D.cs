using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class RotateTransform3D
	{
		public RotateTransform3D(Device device) : base(IntPtr.Zero)
		{
			device.CreateRotateTransform3D(this);
		}

		public RotateTransform3D(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateRotateTransform3D(this);
		}
	}
}
