using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.DirectComposition
{
	partial class DesktopDevice
	{
		/// <summary>
		/// Creates a new device object that can be used to create other Microsoft DirectComposition objects.
		/// </summary>
		/// <param name="renderingDevice">An optional reference to a DirectX device to be used to create DirectComposition surface objects. Must be a reference to an object implementing the <strong><see cref="SharpDX.DXGI.Device"/></strong> or <strong><see cref="SharpDX.Direct2D1.Device"/></strong> interfaces.</param>
		public DesktopDevice(ComObject renderingDevice)
			: base(IntPtr.Zero)
		{
			IntPtr temp;
			DComp.CreateDevice2(renderingDevice, Utilities.GetGuidFromType(typeof(DesktopDevice)), out temp);
			NativePointer = temp;
		}

		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
			[DllImport("user32.dll")]
			public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int index);

			[DllImport("user32.dll")]
			public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
			[DllImport("user32.dll")]
			public static extern int GetWindowLong(IntPtr hwnd, int index);

			[DllImport("user32.dll")]
			public static extern int SetLayeredWindowAttributes(IntPtr hwnd, int color, byte alpha, int flags);

			[DllImport("dwmapi.dll")]
			public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttr, int attrSize);
			[DllImport("dwmapi.dll")]
			public static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttr, int attrSize);

			public const int GWL_EXSTYLE = -20;
			public const int WS_EX_LAYERED = 0x00080000;
			public const int LWA_ALPHA = 2;
			public const int DWMWA_CLOAK = 13;
		}

		/// <summary>
		/// Sets the DWM cloaking state for the given window.
		/// </summary>
		/// <param name="hwnd">A window handle.</param>
		/// <param name="cloaked">True to cloak the window, false to make the window visible again.</param>
		/// <remarks>
		/// When a window is used as a source in a DirectComposition tree, by default it will still be rendered in its original position and 
		/// layout. Cloaking the window causes it to disappear from its original position and render only through the DirectComposition tree.
		/// </remarks>
		public static void SetWindowIsCloaked(IntPtr hwnd, bool cloaked)
		{
			int value = 1;
			int result = NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWMWA_CLOAK, ref value, sizeof(int));
			if (result != 0)
				throw new Win32Exception(result);
		}

		/// <summary>
		/// Gets the cloaking state for a given window.
		/// </summary>
		/// <param name="hwnd">A window handle.</param>
		/// <returns>True if the window is currently being cloaked, false if it is visible in its regular position.</returns>
		public static bool GetWindowIsCloaked(IntPtr hwnd)
		{
			int value = 1;
			int result = NativeMethods.DwmGetWindowAttribute(hwnd, NativeMethods.DWMWA_CLOAK, ref value, sizeof(int));
			if (result != 0)
				throw new Win32Exception(result);
			return value != 0;
		}

		/// <summary>
		/// Sets or clears the WS_EX_LAYERED extended style for a window.
		/// </summary>
		/// <param name="hwnd">A window handle.</param>
		/// <param name="isLayered">True to apply the layered style to the window, false to clear the style.</param>
		/// <remarks>
		/// Only windows with the WS_EX_LAYERED style can be used as Visual content. Starting in Windows 8, child windows can also be layered windows.
		/// </remarks>
		public static void SetWindowIsLayered(IntPtr hwnd, bool isLayered)
		{
#if NET45
            if (Environment.Is64BitProcess)
#else
            if(IntPtr.Size == 8)
#endif
            {
				IntPtr exStyle = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_EXSTYLE);
				if (exStyle == IntPtr.Zero)
					throw new Win32Exception();
				exStyle = NativeMethods.SetWindowLongPtr(hwnd, NativeMethods.GWL_EXSTYLE, (IntPtr)(exStyle.ToInt64() | NativeMethods.WS_EX_LAYERED));
				if (exStyle == IntPtr.Zero)
					throw new Win32Exception();
			}
			else
			{
				int exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
				if (exStyle == 0)
					throw new Win32Exception();
				exStyle = NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, exStyle | NativeMethods.WS_EX_LAYERED);
				if (exStyle == 0)
					throw new Win32Exception();
			}
		}

		/// <summary>
		/// Gets whether a given window has the WS_EX_LAYERED extended style applied.
		/// </summary>
		/// <param name="hwnd">A window handle.</param>
		/// <returns>True if the window has the layered style applied, false otherwise.</returns>
		public static bool GetWindowIsLayered(IntPtr hwnd)
		{
#if NET45
            if (Environment.Is64BitProcess)
#else
            if (IntPtr.Size == 8)
#endif
            {
                IntPtr exStyle = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_EXSTYLE);
				if (exStyle == IntPtr.Zero)
					throw new Win32Exception();
				return (int)(exStyle.ToInt64() & NativeMethods.WS_EX_LAYERED) == NativeMethods.WS_EX_LAYERED;
			}
			else
			{
				int exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
				if (exStyle == 0)
					throw new Win32Exception();
				return (exStyle & NativeMethods.WS_EX_LAYERED) == NativeMethods.WS_EX_LAYERED;
			}
		}
	}
}
