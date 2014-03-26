using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.DirectManipulation
{
	class CompositorShadow : ComObjectShadow
	{
		private static readonly CompositorVtbl Vtbl = new CompositorVtbl(0);

		/// <summary>
		/// Return a pointer to the unmanaged version of this callback.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <returns>A pointer to a shadow c++ callback</returns>
		public static IntPtr ToIntPtr(Compositor callback)
		{
			return ToCallbackPtr<Compositor>(callback);
		}

		public class CompositorVtbl : ComObjectVtbl
		{
			public CompositorVtbl(int methods)
				: base(1 + methods)
			{
				AddMethod(new AddContentDelegate(AddContentImpl));
				AddMethod(new RemoveContentDelegate(RemoveContentImpl));
				AddMethod(new SetUpdateManagerDelegate(SetUpdateManagerImpl));
				AddMethod(new Flush(FlushImpl));
			}

			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			private delegate int AddContentDelegate(IntPtr thisPtr, IntPtr content, IntPtr device, IntPtr arentVisualRef, IntPtr childVisual);
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			private delegate int RemoveContentDelegate(IntPtr thisPtr, IntPtr content);
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			private delegate int SetUpdateManagerDelegate(IntPtr thisPtr, IntPtr updateManager);
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			private delegate int Flush(IntPtr thisPtr);

			private static int AddContentImpl(IntPtr thisPtr, IntPtr content, IntPtr device, IntPtr arentVisualRef, IntPtr childVisual)
			{
				try
				{
					var shadow = ToShadow<CompositorShadow>(thisPtr);
					var callback = (Compositor)shadow.Callback;
					callback.AddContent(new Content(content), new ComObject(device), new ComObject(arentVisualRef), new ComObject(childVisual));
				}
				catch (Exception ex)
				{
					return (int)SharpDX.Result.GetResultFromException(ex);
				}
				return Result.Ok.Code;
			}

			private static int RemoveContentImpl(IntPtr thisPtr, IntPtr content)
			{
				try
				{
					var shadow = ToShadow<CompositorShadow>(thisPtr);
					var callback = (Compositor)shadow.Callback;
					callback.RemoveContent(new Content(content));
				}
				catch (Exception ex)
				{
					return (int)SharpDX.Result.GetResultFromException(ex);
				}
				return Result.Ok.Code;
			}

			private static int SetUpdateManagerImpl(IntPtr thisPtr, IntPtr updateManager)
			{
				try
				{
					var shadow = ToShadow<CompositorShadow>(thisPtr);
					var callback = (Compositor)shadow.Callback;
					callback.SetUpdateManager(new UpdateManager(updateManager));
				}
				catch (Exception ex)
				{
					return (int)SharpDX.Result.GetResultFromException(ex);
				}
				return Result.Ok.Code;
			}

			private static int FlushImpl(IntPtr thisPtr)
			{
				try
				{
					var shadow = ToShadow<CompositorShadow>(thisPtr);
					var callback = (Compositor)shadow.Callback;
					callback.Flush();
				}
				catch (Exception ex)
				{
					return (int)SharpDX.Result.GetResultFromException(ex);
				}
				return Result.Ok.Code;
			}
		}

		protected override CppObjectVtbl GetVtbl
		{
			get { return Vtbl; }
		}
	}
}
