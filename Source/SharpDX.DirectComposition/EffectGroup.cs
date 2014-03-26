using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class EffectGroup
	{
		public EffectGroup(Device device)
			: base(IntPtr.Zero)
		{
			device.CreateEffectGroup(this);
		}

		public EffectGroup(Device2 device) : base(IntPtr.Zero)
		{
			device.CreateEffectGroup(this);
		}
	}
}
