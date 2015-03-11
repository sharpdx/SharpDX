using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class RotateTransform
	{
		public RotateTransform(Device device) : base(IntPtr.Zero)
		{
			device.CreateRotateTransform(this);
		}

		public RotateTransform(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateRotateTransform(this);
		}
	}
}
