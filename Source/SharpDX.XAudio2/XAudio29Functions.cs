using System;
using System.Runtime.InteropServices;

namespace SharpDX.XAudio2
{
    /// <summary>	
    /// Functions	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='SharpDX.XAudio2.XAudio29Functions']/*"/>	
    static  partial class XAudio29Functions {   
        
        /// <summary>	
        /// <p>Creates a new <strong>XAudio2</strong> object and returns a reference to its <strong><see cref="SharpDX.XAudio2.XAudio2"/></strong> interface.</p>	
        /// </summary>	
        /// <param name="xAudio2Out">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <param name="xAudio2Processor">No documentation.</param>	
        /// <returns><p>Returns <see cref="SharpDX.Result.Ok"/> if successful, an error code otherwise. See <strong>XAudio2 Error Codes</strong> for descriptions of XAudio2 specific error codes. </p></returns>	
        /// <remarks>	
        /// <p>The DirectX SDK versions of XAUDIO2 supported a flag <strong><see cref="SharpDX.XAudio2.XAudio2Flags.DebugEngine"/></strong> to select between the release and 'checked' version. This flag is not supported or defined in the Windows 8 version of XAUDIO2. </p><p><strong>Note</strong>??No versions of the DirectX SDK contain the xaudio2.lib import library. DirectX SDK versions use COM to create a new <strong>XAudio2</strong> object.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='XAudio2Create']/*"/>	
        /// <msdn-id>microsoft.directx_sdk.xaudio2.xaudio2create</msdn-id>	
        /// <unmanaged>HRESULT XAudio2Create([Out, Fast] IXAudio2** ppXAudio2,[In] unsigned int Flags,[In] unsigned int XAudio2Processor)</unmanaged>	
        /// <unmanaged-short>XAudio2Create</unmanaged-short>	
        public static void XAudio2Create(SharpDX.XAudio2.XAudio2 xAudio2Out, int flags, int xAudio2Processor) {
            unsafe {
                IntPtr xAudio2Out_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__= 
                    XAudio2Create_(&xAudio2Out_, flags, xAudio2Processor);		
                ((SharpDX.XAudio2.XAudio2)xAudio2Out).NativePointer = xAudio2Out_;
                __result__.CheckError();
            }
        }
        [DllImport("xaudio2_9.dll", EntryPoint = "XAudio2Create", CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern int XAudio2Create_(void* arg0,int arg1,int arg2);
        
        /// <summary>	
        /// <p>Creates a new reverb audio processing object (APO), and returns a reference to it.</p>	
        /// </summary>	
        /// <param name="apoOut"><dd> <p>Contains a reference to the reverb APO that is created.</p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p><strong>XAudio2CreateReverb</strong> creates an effect performing Princeton Digital Reverb. The XAPO effect library (XAPOFX) includes an alternate reverb effect. Use <strong>CreateFX</strong> to create this alternate effect. </p><p>The reverb APO supports has the following restrictions: </p><ul> <li>Input audio data must be FLOAT32. </li> <li>Framerate must be within XAUDIO2FX_REVERB_MIN_FRAMERATE (20,000 Hz) and XAUDIO2FX_REVERB_MAX_FRAMERATE (48,000 Hz). </li> <li>The input and output channels must be one of the following combinations.<ul> <li>Mono input and mono output </li> <li>Mono input and 5.1 output </li> <li>Stereo input and stereo output </li> <li>Stereo input and 5.1 output</li> </ul> </li> </ul>The reverb APO maintains internal state information between processing samples. You can only use an instance of the APO with one source of audio data at a time. Multiple voices that require reverb effects would each need to create a separate reverb effect with<strong>XAudio2CreateReverb</strong>.<p>For information about creating new effects for use with XAudio2, see the XAPO Overview. </p><table> <tr><th>Windows</th></tr> <tr><td> <p>Because <strong>XAudio2CreateReverb</strong> calls <strong>CoCreateInstance</strong> on Windows, the application must have called the <strong>CoInitializeEx</strong> method before calling <strong>XAudio2CreateReverb</strong>. <strong><see cref="XAudio29Functions.XAudio2Create"/></strong> has the same requirement, which means <strong>CoInitializeEx</strong> typically will be called long before <strong>XAudio2CreateReverb</strong> is called. </p> <p>A typical calling pattern on Windows would be as follows: </p>  <pre>#ifndef _XBOX	
        /// CoInitializeEx(<c>null</c>, COINIT_MULTITHREADED);	
        /// #endif	
        /// <see cref="SharpDX.XAudio2.XAudio2"/>* pXAudio2 = <c>null</c>;	
        /// <see cref="SharpDX.Result"/> hr;	
        /// if ( FAILED(hr = <see cref="XAudio29Functions.XAudio2Create"/>( &amp;pXAudio2, 0, <see cref="SharpDX.XAudio2.ProcessorSpecifier.DefaultProcessor"/> ) ) ) return hr;	
        /// ...	
        /// <see cref="SharpDX.ComObject"/> * pReverbAPO;	
        /// XAudio2CreateReverb(&amp;pReverbAPO);	
        /// </pre>  </td></tr> </table><p>?</p><p>The xaudio2fx.h header defines the <strong>AudioReverb</strong> class <see cref="System.Guid"/> as   a cross-platform audio processing object (XAPO). </p><code> class __declspec(uuid("C2633B16-471B-4498-B8C5-4F0959E2EC09")) AudioReverb;	
        /// </code><p><strong>XAudio2CreateReverb</strong> returns this object as a reference to a reference to <strong><see cref="SharpDX.ComObject"/></strong> in the <em>ppApo</em> parameter. Although you can query the <strong><see cref="SharpDX.XAPO.AudioProcessor"/></strong> and <strong><see cref="SharpDX.XAPO.ParameterProvider"/></strong> interfaces from this <strong><see cref="SharpDX.ComObject"/></strong>, you typically never use these interfaces directly. Instead, you use them when you create a voice to add them as part of the effects chain. </p><p>The reverb uses the <strong><see cref="SharpDX.XAudio2.Fx.ReverbParameters"/></strong> parameter structure that you access via the <strong><see cref="SharpDX.XAudio2.Voice.SetEffectParameters"/></strong>. </p><p><strong>Note</strong>??<strong>XAudio2CreateReverb</strong> is an inline function in xaudio2fx.h that calls <strong>CreateAudioReverb</strong>: </p><code> XAUDIO2FX_STDAPI CreateAudioReverb(_Outptr_ <see cref="SharpDX.ComObject"/>** ppApo);	
        /// __inline <see cref="SharpDX.Result"/> XAudio2CreateReverb(_Outptr_ <see cref="SharpDX.ComObject"/>** ppApo, UINT32 /*Flags*/ DEFAULT(0))	
        /// { return CreateAudioReverb(ppApo);	
        /// }	
        /// </code>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='CreateAudioReverb']/*"/>	
        /// <msdn-id>microsoft.directx_sdk.xaudio2.xaudio2createreverb</msdn-id>	
        /// <unmanaged>HRESULT CreateAudioReverb([Out, Fast] IUnknown** ppApo)</unmanaged>	
        /// <unmanaged-short>CreateAudioReverb</unmanaged-short>	
        public static void CreateAudioReverb(SharpDX.ComObject apoOut) {
            unsafe {
                IntPtr apoOut_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__= 
                    CreateAudioReverb_(&apoOut_);		
                ((SharpDX.ComObject)apoOut).NativePointer = apoOut_;
                __result__.CheckError();
            }
        }
        [DllImport("xaudio2_9.dll", EntryPoint = "CreateAudioReverb", CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern int CreateAudioReverb_(void* arg0);
        
        /// <summary>	
        /// <p>Creates a new volume meter audio processing object (APO) and returns a reference to it.</p>	
        /// </summary>	
        /// <param name="apoOut"><dd> <p>Contains the created volume meter APO.</p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>For information on creating new effects for use with XAudio2, see the XAPO Overview.</p><table> <tr><th>Windows</th></tr> <tr><td> <p>Because <strong>XAudio2CreateVolumeMeter</strong> calls <strong>CoCreateInstance</strong> on Windows, the application must have called the <strong>CoInitializeEx</strong> method before calling <strong>XAudio2CreateVolumeMeter</strong>. <strong><see cref="XAudio29Functions.XAudio2Create"/></strong> has the same requirement, which means <strong>CoInitializeEx</strong> typically will be called long before <strong>XAudio2CreateVolumeMeter</strong> is called. </p> <p>A typical calling pattern on Windows would be as follows: </p>  <pre>#ifndef _XBOX	
        /// CoInitializeEx(<c>null</c>, COINIT_MULTITHREADED);	
        /// #endif	
        /// <see cref="SharpDX.XAudio2.XAudio2"/>* pXAudio2 = <c>null</c>;	
        /// <see cref="SharpDX.Result"/> hr;	
        /// if ( FAILED(hr = <see cref="XAudio29Functions.XAudio2Create"/>( &amp;pXAudio2, 0, <see cref="SharpDX.XAudio2.ProcessorSpecifier.DefaultProcessor"/> ) ) ) return hr;	
        /// ...	
        /// <see cref="SharpDX.ComObject"/> * pVolumeMeterAPO;	
        /// XAudio2CreateVolumeMeter(&amp;pVolumeMeterAPO);	
        /// </pre>  </td></tr> </table><p>?</p><p>The xaudio2fx.h header defines the <strong>AudioVolumeMeter</strong> class <see cref="System.Guid"/> as   a cross-platform audio processing object (XAPO). </p><code> class __declspec(uuid("4FC3B166-972A-40CF-BC37-7DB03DB2FBA3")) AudioVolumeMeter;	
        /// </code><p><strong>XAudio2CreateVolumeMeter</strong> returns this object as a reference to a reference to <strong><see cref="SharpDX.ComObject"/></strong> in the <em>ppApo</em> parameter. Although you can query the <strong><see cref="SharpDX.XAPO.AudioProcessor"/></strong> and <strong><see cref="SharpDX.XAPO.ParameterProvider"/></strong> interfaces from this <strong><see cref="SharpDX.ComObject"/></strong>, you typically never use these interfaces directly. Instead, you use them when you create a voice to add them as part of the effects chain. </p><p>The volume meter uses the <strong><see cref="SharpDX.XAudio2.Fx.VolumeMeterLevels"/></strong> parameter structure that you access via the <strong><see cref="SharpDX.XAudio2.Voice.GetEffectParameters"/></strong> method when the XAPO is bound to the audio graph.</p><p><strong>Note</strong>??<strong>XAudio2CreateVolumeMeter</strong> is an inline function in xaudio2fx.h that calls <strong>CreateAudioVolumeMeter</strong>: </p><code> XAUDIO2FX_STDAPI CreateAudioVolumeMeter(_Outptr_ <see cref="SharpDX.ComObject"/>** ppApo);	
        /// __inline <see cref="SharpDX.Result"/> XAudio2CreateVolumeMeter(_Outptr_ <see cref="SharpDX.ComObject"/>** ppApo, UINT32 /*Flags*/ DEFAULT(0))	
        /// { return CreateAudioVolumeMeter(ppApo);	
        /// }	
        /// </code>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='CreateAudioVolumeMeter']/*"/>	
        /// <msdn-id>microsoft.directx_sdk.xaudio2.xaudio2createvolumemeter</msdn-id>	
        /// <unmanaged>HRESULT CreateAudioVolumeMeter([Out, Fast] IUnknown** ppApo)</unmanaged>	
        /// <unmanaged-short>CreateAudioVolumeMeter</unmanaged-short>	
        public static void CreateAudioVolumeMeter(SharpDX.ComObject apoOut) {
            unsafe {
                IntPtr apoOut_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__= 
                    CreateAudioVolumeMeter_(&apoOut_);		
                ((SharpDX.ComObject)apoOut).NativePointer = apoOut_;
                __result__.CheckError();
            }
        }
        [DllImport("xaudio2_9.dll", EntryPoint = "CreateAudioVolumeMeter", CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern int CreateAudioVolumeMeter_(void* arg0);
    }
}