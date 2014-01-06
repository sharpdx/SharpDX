using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Device
	{
		public Device(DXGI.Device device)
		{
			IntPtr temp;
			DComp.CreateDevice(device, Utilities.GetGuidFromType(typeof(Device)), out temp);
			NativePointer = temp;
		}
	}
}
