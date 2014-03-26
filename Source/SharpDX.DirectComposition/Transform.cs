using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Transform
	{
		public Transform(Device dev, params Transform[] effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransformGroup(effects, effects.Length, this);
		}

		public Transform(Device2 dev, params Transform[] effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransformGroup(effects, effects.Length, this);
		}

		public Transform(Device dev, ComArray<Transform> effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransformGroup(effects, effects.Length, this);
		}
		
		public Transform(Device2 dev, ComArray<Transform> effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransformGroup(effects, effects.Length, this);
		}
	}
}
