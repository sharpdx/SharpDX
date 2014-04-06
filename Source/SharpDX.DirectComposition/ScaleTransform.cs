using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class ScaleTransform
	{
		public ScaleTransform(Device device) : base(IntPtr.Zero)
		{
			device.CreateScaleTransform(this);
		}

		public ScaleTransform(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateScaleTransform(this);
		}
	}
}
