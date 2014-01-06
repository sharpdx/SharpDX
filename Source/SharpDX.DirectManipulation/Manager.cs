using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class Manager
	{
		public static readonly Guid ManagerClassId = new Guid("54E211B6-3650-4F75-8334-FA359598E1C5");

		public Manager() : base(IntPtr.Zero)
		{
			Utilities.CreateComInstance(ManagerClassId, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(Manager)), this);
		}

		public T CreateContent<T>(FrameInfoProvider frameInfo, Guid contentClassId) where T : ComObject
		{
			IntPtr temp;
			CreateContent(frameInfo, contentClassId, Utilities.GetGuidFromType(typeof(T)), out temp);
			return ComObject.FromPointer<T>(temp);
		}

		public T CreateViewport<T>(FrameInfoProvider frameInfo, IntPtr window) where T : ComObject
		{
			IntPtr temp;
			CreateViewport(frameInfo, window, Utilities.GetGuidFromType(typeof(T)), out temp);
			return ComObject.FromPointer<T>(temp);
		}
	}
}
