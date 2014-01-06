using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class AutoScrollBehavior
	{
		public AutoScrollBehavior(Manager2 manager)
		{
			IntPtr temp;
			manager.CreateBehavior(Manager2.AutoScrollBehavior, Utilities.GetGuidFromType(typeof(AutoScrollBehavior)), out temp);
			NativePointer = temp;
		}
	}
}
