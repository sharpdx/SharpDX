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

		/// <summary>
		/// The factory method that is used to create an instance of secondary content (such as a panning indicator) inside a viewport.
		/// </summary>
		/// <typeparam name="T">The type of the COM object to create.</typeparam>
		/// <param name="frameInfo">The frame info provider for the secondary content. This should match the frame info provider used to create the viewport.</param>
		/// <param name="contentClassId">Class identifier (CLSID) of the secondary content. This ID specifies the content type.</param>
		/// <returns></returns>
		public T CreateContent<T>(FrameInfoProvider frameInfo, Guid contentClassId) where T : ComObject
		{
			IntPtr temp;
			CreateContent(frameInfo, contentClassId, Utilities.GetGuidFromType(typeof(T)), out temp);
			return ComObject.FromPointer<T>(temp);
		}
	}
}
