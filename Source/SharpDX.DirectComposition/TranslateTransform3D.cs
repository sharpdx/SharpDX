using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class TranslateTransform3D
	{
		public TranslateTransform3D(Device device) : base(IntPtr.Zero)
		{
			device.CreateTranslateTransform3D(this);
		}

		public TranslateTransform3D(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateTranslateTransform3D(this);
		}
	}
}
