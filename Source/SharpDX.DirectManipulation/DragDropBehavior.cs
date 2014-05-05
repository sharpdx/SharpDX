using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class DragDropBehavior
	{
		public DragDropBehavior(Manager2 manager)
		{
			IntPtr temp;
			manager.CreateBehavior(Manager2.DragDropConfigurationBehavior, Utilities.GetGuidFromType(typeof(DragDropBehavior)), out temp);
			NativePointer = temp;
		}
	}
}
