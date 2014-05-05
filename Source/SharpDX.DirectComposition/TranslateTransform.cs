using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class TranslateTransform
	{
		public TranslateTransform(Device device) : base(IntPtr.Zero)
		{
			device.CreateTranslateTransform(this);
		}

		public TranslateTransform(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateTranslateTransform(this);
		}
	}
}
