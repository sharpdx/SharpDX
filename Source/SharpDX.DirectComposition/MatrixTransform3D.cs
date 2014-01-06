using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class MatrixTransform3D
	{
		public MatrixTransform3D(Device device) : base(IntPtr.Zero)
		{
			device.CreateMatrixTransform3D(this);
		}

		public MatrixTransform3D(Device2 device)
			: base(IntPtr.Zero)
		{
			device.CreateMatrixTransform3D(this);
		}
	}
}
