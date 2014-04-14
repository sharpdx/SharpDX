using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class RectangleClip
	{
		public RectangleClip(Device device) : base(IntPtr.Zero)
		{
			device.CreateRectangleClip(this);
		}

		public RectangleClip(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateRectangleClip(this);
		}
	}
}
