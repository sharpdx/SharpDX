using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Device2
	{
		public Device2(ComObject renderingDevice)
		{
			IntPtr temp;
			DComp.CreateDevice2(renderingDevice, Utilities.GetGuidFromType(typeof(Device2)), out temp);
			NativePointer = temp;
		}
	}
}
