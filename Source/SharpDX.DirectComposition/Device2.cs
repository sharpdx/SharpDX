using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Device2
	{
		/// <summary>
		/// Creates a new device object that can be used to create other Microsoft DirectComposition objects.
		/// </summary>
		/// <param name="renderingDevice">An optional reference to a DirectX device to be used to create DirectComposition surface 
		/// objects. Must be a reference to an object implementing the <strong><see cref="SharpDX.DXGI.Device"/></strong> or
		/// <strong><see cref="SharpDX.Direct2D1.Device"/></strong> interfaces.</param>
		public Device2(ComObject renderingDevice)
		{
			IntPtr temp;
			DComp.CreateDevice2(renderingDevice?.NativePointer ?? IntPtr.Zero, Utilities.GetGuidFromType(typeof(Device2)), out temp);
			NativePointer = temp;
		}
	}
}
