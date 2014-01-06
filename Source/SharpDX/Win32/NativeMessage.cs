using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.Win32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct NativeMessage
	{
		public IntPtr handle;
		public uint msg;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public Point p;
	}
}
