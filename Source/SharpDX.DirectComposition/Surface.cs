using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectComposition
{
	partial class Surface
	{
		public Surface(Device device, int width, int height, DXGI.Format format, DXGI.AlphaMode alphaMode) : base(IntPtr.Zero)
		{
			device.CreateSurface(width, height, format, alphaMode, this);
		}

		public Surface(Device2 device, int width, int height, DXGI.Format format, DXGI.AlphaMode alphaMode)
			: base(IntPtr.Zero)
		{
			device.CreateSurface(width, height, format, alphaMode, this);
		}

		public Surface(SurfaceFactory factory, int initialWidth, int initialHeight, DXGI.Format format, DXGI.AlphaMode alphaMode)
			: base(IntPtr.Zero)
		{
			factory.CreateSurface(initialWidth, initialHeight, format, alphaMode, this);
		}

		public T BeginDraw<T>(RawRectangle? updateRect, out RawPoint updateOffset) where T : ComObject
		{
			IntPtr temp;
			BeginDraw(updateRect, Utilities.GetGuidFromType(typeof(T)), out temp, out updateOffset);
			return ComObject.FromPointer<T>(temp);
		}
	}
}
