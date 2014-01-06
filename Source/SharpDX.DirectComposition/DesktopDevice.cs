using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class DesktopDevice
	{
		public DesktopDevice(ComObject renderingDevice) : base(IntPtr.Zero)
		{
			IntPtr temp;
			DComp.CreateDevice2(renderingDevice, Utilities.GetGuidFromType(typeof(DesktopDevice)), out temp);
			NativePointer = temp;
		}
	}
}
