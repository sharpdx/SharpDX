using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Animation
	{
		public Animation(Device device) : base(IntPtr.Zero)
		{
			device.CreateAnimation(this);
		}

		public Animation(Device2 device) : base(IntPtr.Zero)
		{
			device.CreateAnimation(this);
		}
	}
}
