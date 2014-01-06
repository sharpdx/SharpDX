using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class Viewport
	{
		public Viewport(Manager manager, FrameInfoProvider frameInfo, IntPtr window) : base(IntPtr.Zero)
		{
			IntPtr temp;
			manager.CreateViewport(frameInfo, window, Utilities.GetGuidFromType(typeof(Viewport)), out temp);
			NativePointer = temp;
		}
	}
}
