using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	public partial class Device
	{
		/// <summary>	
		/// Creates a new device object that can be used to create other Microsoft DirectComposition objects.
		/// </summary>	
		/// <param name="device">The DXGI device to use to create DirectComposition surface objects.</param>
		public Device(DXGI.Device device)
		{
			IntPtr temp;
			DComp.CreateDevice(device, Utilities.GetGuidFromType(typeof(Device)), out temp);
			NativePointer = temp;
		}
	}
}
