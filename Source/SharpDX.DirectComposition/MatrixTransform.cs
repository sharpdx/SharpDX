using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class MatrixTransform
	{
		public MatrixTransform(Device device) : base(IntPtr.Zero)
		{
			device.CreateMatrixTransform(this);
		}

		public MatrixTransform(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateMatrixTransform(this);
		}
	}
}
