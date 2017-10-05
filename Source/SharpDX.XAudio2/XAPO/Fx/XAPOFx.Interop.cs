using System;
using System.Runtime.InteropServices;

namespace SharpDX.XAPO.Fx
{
    /// <summary>	
    /// Functions	
    /// </summary>	
    /// <include file='..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='SharpDX.XAPO.Fx.XAPOFx']/*"/>	
    static partial class XAPOFx {
#if WINDOWS_UWP
        /// <summary>	
        /// <p>Creates an instance of the requested XAPOFX effect.</p>	
        /// </summary>	
        /// <param name="clsid">No documentation.</param>	
        /// <param name="effectRef">No documentation.</param>	
        /// <param name="initDataRef">No documentation.</param>	
        /// <param name="initDataByteSize">No documentation.</param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>The created XAPO will have a reference count of 1. Client code must call <strong>IUnknown::Release</strong> after passing the XAPO to XAudio2 to allow XAudio2 to dispose of the XAPO when it is no longer needed. Use <strong> <see cref="SharpDX.XAudio2.XAudio2.CreateSourceVoice_"/></strong> or <strong><see cref="SharpDX.XAudio2.Voice.SetEffectChain"/></strong> to pass an XAPO to XAudio2. </p><p><strong>Note</strong>??The DirectX SDK version of this function doesn't have the <em>pInitData</em> or <em>InitDataByteSize</em> parameters as it only takes the first 2 parameters. To set initial parameters for the XAPOFX effect that is  created with the DirectX SDK version of this function, you must bind that effect to a voice and use <strong><see cref="SharpDX.XAudio2.Voice.SetEffectParameters"/></strong>.	
        /// For info about how to do this, see How to: Use XAPOFX in XAudio2.</p>	
        /// </remarks>	
        /// <include file='..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='CreateFX']/*"/>	
        /// <msdn-id>hh405044</msdn-id>	
        /// <unmanaged>HRESULT CreateFX([In] const GUID&amp; clsid,[Out, Fast] IUnknown** pEffect,[In, Buffer, Optional] const void* pInitData,[In] unsigned int InitDataByteSize)</unmanaged>	
        /// <unmanaged-short>CreateFX</unmanaged-short>	
        public static void CreateFX29(System.Guid clsid, SharpDX.ComObject effectRef, System.IntPtr initDataRef, int initDataByteSize) {
            unsafe {
                IntPtr effectRef_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__=
                    CreateFX_29(&clsid, &effectRef_, (void*)initDataRef, initDataByteSize);		
                ((SharpDX.ComObject)effectRef).NativePointer = effectRef_;
                __result__.CheckError();
            }
        }
        [DllImport("xaudio2_9.dll", EntryPoint = "CreateFX", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int CreateFX_29(void* arg0,void* arg1,void* arg2,int arg3);
#endif

        public static void CreateFX28(System.Guid clsid, SharpDX.ComObject effectRef, System.IntPtr initDataRef, int initDataByteSize)
        {
            unsafe
            {
                IntPtr effectRef_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__ =
                    CreateFX_28(&clsid, &effectRef_, (void*)initDataRef, initDataByteSize);
                ((SharpDX.ComObject)effectRef).NativePointer = effectRef_;
                __result__.CheckError();
            }
        }
        [DllImport("xaudio2_8.dll", EntryPoint = "CreateFX", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int CreateFX_28(void* arg0, void* arg1, void* arg2, int arg3);
    }
}