using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class SkewTransform
	{
		public SkewTransform(Device device) : base(IntPtr.Zero)
		{
			device.CreateSkewTransform(this);
		}

		public SkewTransform(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateSkewTransform(this);
		}
	}
}
