using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class Manager2
	{
		public Manager2() : base(IntPtr.Zero)
		{
			Utilities.CreateComInstance(ManagerClassId, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(Manager2)), this);
		}

		/// <summary>
		/// Factory method to create a behavior.
		/// </summary>
		/// <typeparam name="T">The type of the COM object to create.</typeparam>
		/// <param name="classId">CLSID of the behavior. The CLSID specifies the type of behavior.</param>
		/// <returns></returns>
		public T CreateBehavior<T>(Guid classId) where T : ComObject
		{
			IntPtr temp;
			CreateBehavior(classId, Utilities.GetGuidFromType(typeof(T)), out temp);
			return ComObject.FromPointer<T>(temp);
		}
	}
}
