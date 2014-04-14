using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Transform3D
	{
		public Transform3D(Device dev, params Transform3D[] effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransform3DGroup(effects, effects.Length, this);
		}

		public Transform3D(Device2 dev, params Transform3D[] effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransform3DGroup(effects, effects.Length, this);
		}

		public Transform3D(Device dev, ComArray<Transform3D> effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransform3DGroup(effects, effects.Length, this);
		}

		public Transform3D(Device2 dev, ComArray<Transform3D> effects)
			: this(IntPtr.Zero)
		{
			dev.CreateTransform3DGroup(effects, effects.Length, this);
		}
	}
}
