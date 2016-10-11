using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class Target
	{
		/// <summary>
		/// Creates a composition target object that is bound to the window that is represented by the specified window handle.
		/// </summary>
		/// <param name="desktopDevice">A device the target is to be associated with.</param>
		/// <param name="hwnd">The window to which the composition object will be bound. This cannot be null.</param>
		/// <param name="topmost">TRUE if the visual tree should be displayed on top of the children of the window specified by the hwnd parameter; otherwise, the visual tree is displayed behind the children.</param>
		/// <returns></returns>
		public static Target FromHwnd(DesktopDevice desktopDevice, IntPtr hwnd, bool topmost)
		{
			return desktopDevice.CreateTargetForHwnd(hwnd, topmost);
		}
        
        /// <summary>
		/// Creates a composition target object that is bound to the window that is represented by the specified window handle.
		/// </summary>
		/// <param name="device">A device the target is to be associated with.</param>
		/// <param name="hwnd">The window to which the composition object will be bound. This cannot be null.</param>
		/// <param name="topmost">TRUE if the visual tree should be displayed on top of the children of the window specified by the hwnd parameter; otherwise, the visual tree is displayed behind the children.</param>
		/// <returns></returns>
      public static Target FromHwnd(Device device, IntPtr hwnd, bool topmost)
      {
         Target target;
         device.CreateTargetForHwnd(hwnd, topmost, out target);
         return target;
      }
	}
}
