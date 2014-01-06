using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Target
	{
		public static Target FromHwnd(DesktopDevice desktopDevice, IntPtr hwnd, bool topmost)
		{
			return desktopDevice.CreateTargetForHwnd(hwnd, topmost);
		}
	}
}
