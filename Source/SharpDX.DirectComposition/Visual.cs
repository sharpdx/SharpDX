using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Visual
	{
		public Visual(Device device)
			: base(IntPtr.Zero)
		{
			device.CreateVisual(this);
		}
	}
}
