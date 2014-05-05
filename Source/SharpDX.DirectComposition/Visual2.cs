using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Visual2
	{
		public Visual2(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateVisual(this);
		}
	}
}
