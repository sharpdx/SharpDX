using System;
using System.Runtime.InteropServices;
using SharpDX.Multimedia;

namespace SharpDX.XAPO
{
    /// <summary>
    /// Internal AudioProcessorShadow
    /// </summary>
    /// IXAPO GUID
    [Guid("a90bc001-e897-e897-55e4-9e4700000000")]
    internal class AudioProcessorShadow : ComObjectShadow
    {
        private static readonly AudioProcessorVtbl Vtbl = new AudioProcessorVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(AudioProcessor callback)
        {
            return ToCallbackPtr<AudioProcessor>(callback);
        }

        public class AudioProcessorVtbl : ComObjectVtbl
        {
            public AudioProcessorVtbl() : base(10)
            {
                unsafe
                {
                    AddMethod(new GetRegistrationPropertiesDelegate(GetRegistrationPropertiesImpl));
                    AddMethod(new IsInputFormatSupportedDelegate(IsInputFormatSupportedImpl));
                    AddMethod(new IsOutputFormatSupportedDelegate(IsOutputFormatSupportedImpl));
                    AddMethod(new InitializeDelegate(InitializeImpl));
                    AddMethod(new ResetDelegate(ResetImpl));
                    AddMethod(new LockForProcessDelegate(LockForProcessImpl));
                    AddMethod(new UnlockForProcessDelegate(UnlockForProcessImpl));
                    AddMethod(new ProcessDelegate(ProcessImpl));
                    AddMethod(new CalcInputFramesDelegate(CalcInputFramesImpl));
                    AddMethod(new CalcOutputFramesDelegate(CalcOutputFramesImpl));
                }
            }

            /// <unmanaged>HRESULT IXAPO::GetRegistrationProperties([Out] XAPO_REGISTRATION_PROPERTIES** ppRegistrationProperties)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetRegistrationPropertiesDelegate(IntPtr thisObject, out IntPtr output);
            private static int GetRegistrationPropertiesImpl(IntPtr thisObject, out IntPtr output)
            {
                output = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                    var callback = (AudioProcessor)shadow.Callback;

                    int sizeOfNative = Utilities.SizeOf<RegistrationProperties.__Native>();
                    output = Marshal.AllocCoTaskMem(sizeOfNative);
                    RegistrationProperties.__Native temp = default(RegistrationProperties.__Native);
                    callback.RegistrationProperties.__MarshalTo(ref temp);
                    Utilities.Write(output, ref temp);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IXAPO::IsInputFormatSupported([None] const WAVEFORMATEX* pOutputFormat,[None] const WAVEFORMATEX* pRequestedInputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedInputFormat)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int IsInputFormatSupportedDelegate(IntPtr thisObject, IntPtr outputFormat, IntPtr requestedInputFormat, out IntPtr supportedInputFormat);
            private static int IsInputFormatSupportedImpl(IntPtr thisObject, IntPtr pOutputFormat, IntPtr pRequestedInputFormat, out IntPtr pSupportedInputFormat)
            {
                pSupportedInputFormat = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                    var callback = (AudioProcessor)shadow.Callback;

                    WaveFormat outputFormat = WaveFormat.MarshalFrom(pOutputFormat);
                    WaveFormat requestedInputFormat = WaveFormat.MarshalFrom(pRequestedInputFormat);

                    WaveFormat supportedInputFormat;
                    var result = callback.IsInputFormatSupported(outputFormat, requestedInputFormat, out supportedInputFormat);

                    int sizeOfWaveFormat = Marshal.SizeOf(supportedInputFormat);
                    pSupportedInputFormat = Marshal.AllocCoTaskMem(sizeOfWaveFormat);

                    Marshal.StructureToPtr(supportedInputFormat, pSupportedInputFormat, false);

                    // return XAPO_E_FORMAT_UNSUPPORTED if fail
                    return result ? Result.Ok.Code : unchecked((int)0x88970001);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
            }

            /// <unmanaged>HRESULT IXAPO::IsOutputFormatSupported([None] const WAVEFORMATEX* pInputFormat,[None] const WAVEFORMATEX* pRequestedOutputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedOutputFormat)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int IsOutputFormatSupportedDelegate(IntPtr thisObject, IntPtr outputFormat, IntPtr requestedInputFormat, out IntPtr supportedInputFormat);
            private static int IsOutputFormatSupportedImpl(IntPtr thisObject, IntPtr pInputFormat, IntPtr pRequestedOutputFormat, out IntPtr pSupportedOutputFormat)
            {
                pSupportedOutputFormat = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                    var callback = (AudioProcessor)shadow.Callback;

                    WaveFormat inputFormat = WaveFormat.MarshalFrom(pInputFormat);
                    WaveFormat requestedOutputFormat = WaveFormat.MarshalFrom(pRequestedOutputFormat);

                    WaveFormat supportedOutputFormat;
                    var result = callback.IsOutputFormatSupported(inputFormat, requestedOutputFormat, out supportedOutputFormat);

                    int sizeOfWaveFormat = Marshal.SizeOf(supportedOutputFormat);
                    pSupportedOutputFormat = Marshal.AllocCoTaskMem(sizeOfWaveFormat);

                    Marshal.StructureToPtr(supportedOutputFormat, pSupportedOutputFormat, false);

                    // return XAPO_E_FORMAT_UNSUPPORTED if fail
                    return result ? Result.Ok.Code : unchecked((int)0x88970001);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
            }

            /// <unmanaged>HRESULT IXAPO::Initialize([In, Buffer, Optional] const void* pData,[None] UINT32 DataByteSize)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int InitializeDelegate(IntPtr thisObject, IntPtr ptr, int dataSize);
            private static int InitializeImpl(IntPtr thisObject, IntPtr ptr, int dataSize)
            {
                try
                {
                    var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                    var callback = (AudioProcessor)shadow.Callback;
                    callback.Initialize(new DataStream(ptr, dataSize, true, true));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>void IXAPO::Reset()</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void ResetDelegate(IntPtr thisObject);
            private static void ResetImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                var callback = (AudioProcessor)shadow.Callback;
                callback.Reset();
            }

            /// <unmanaged>HRESULT IXAPO::LockForProcess([None] UINT32 InputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pInputLockedParameters,[None] UINT32 OutputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pOutputLockedParameters)</unmanaged>
            //void LockForProcess(SharpDX.XAPO.LockforprocessParameters[] inputLockedParameters, SharpDX.XAPO.LockforprocessParameters[] outputLockedParameters);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int LockForProcessDelegate(IntPtr thisObject,
                int inputLockedParameterCount, LockParameters.__Native* pInputLockedParameters, int outputLockedParameterCount, LockParameters.__Native* pOutputLockedParameters);
            private static unsafe int LockForProcessImpl(IntPtr thisObject,
                int inputLockedParameterCount, LockParameters.__Native* pInputLockedParameters, int outputLockedParameterCount, LockParameters.__Native* pOutputLockedParameters)
            {
                try
                {
                    var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                    var callback = (AudioProcessor)shadow.Callback;

                    var inputLockedParameters = new LockParameters[inputLockedParameterCount];
                    for (int i = 0; i < inputLockedParameters.Length; i++)
                    {
                        var param = new LockParameters();
                        param.__MarshalFrom(&pInputLockedParameters[i]);
                        inputLockedParameters[i] = param;
                    }

                    var ouputLockedParameters = new LockParameters[outputLockedParameterCount];
                    for (int i = 0; i < ouputLockedParameters.Length; i++)
                    {
                        var param = new LockParameters();
                        param.__MarshalFrom(&pOutputLockedParameters[i]);
                        ouputLockedParameters[i] = param;
                    }

                    callback.LockForProcess(inputLockedParameters, ouputLockedParameters);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return 0;
            }

            /// <summary>	
            /// Deallocates variables that were allocated with the {{LockForProcess}} method.	
            /// </summary>	
            /// <unmanaged>void IXAPO::UnlockForProcess()</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void UnlockForProcessDelegate(IntPtr thisObject);
            private static void UnlockForProcessImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                var callback = (AudioProcessor)shadow.Callback;
                callback.UnlockForProcess();
            }

            /// <unmanaged>void IXAPO::Process([None] UINT32 InputProcessParameterCount,[In, Buffer, Optional] const XAPO_PROCESS_BUFFER_PARAMETERS* pInputProcessParameters,[None] UINT32 OutputProcessParameterCount,[InOut, Buffer, Optional] XAPO_PROCESS_BUFFER_PARAMETERS* pOutputProcessParameters,[None] BOOL IsEnabled)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate void ProcessDelegate(IntPtr thisObject,
                int inputProcessParameterCount, BufferParameters* pInputProcessParameters, int outputProcessParameterCount, BufferParameters* inputProcessParameters,
                int isEnabled);
            private static unsafe void ProcessImpl(IntPtr thisObject,
                int inputProcessParameterCount, BufferParameters* pInputProcessParameters, int outputProcessParameterCount, BufferParameters* pOutputProcessParameters,
                int isEnabled)
            {
                var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                var callback = (AudioProcessor)shadow.Callback;

                var inputProcessParameters = new BufferParameters[inputProcessParameterCount];
                for (int i = 0; i < inputProcessParameters.Length; i++)
                    inputProcessParameters[i] = pInputProcessParameters[i];

                var outputProcessParameters = new BufferParameters[outputProcessParameterCount];
                for (int i = 0; i < outputProcessParameters.Length; i++)
                    outputProcessParameters[i] = pOutputProcessParameters[i];

                //// NOTE: because XAudio currently support only 1 input and 1 output buffer at a time, don't waste our time
                //BufferParameters outputParameter = *pOutputProcessParameters;

                callback.Process(inputProcessParameters, outputProcessParameters, isEnabled == 1);

                //Update BufferParameters output (see doc for IXAPO::Process
                //ValidFrameCount must be fill by the Process method. Most of the time ValidFrameCount in input == ValidFrameCount in output (effectively written)
                for (int i = 0; i < outputProcessParameters.Length; i++)
                    pOutputProcessParameters[i].ValidFrameCount = outputProcessParameters[i].ValidFrameCount;
            }

            /// <summary>	
            /// Returns the number of input frames required to generate the given number of output frames.	
            /// </summary>
            /// <param name="thisObject">This pointer</param>
            /// <param name="outputFrameCount">The number of output frames desired.</param>
            /// <returns>No documentation.</returns>
            /// <unmanaged>UINT32 IXAPO::CalcInputFrames([None] UINT32 OutputFrameCount)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CalcInputFramesDelegate(IntPtr thisObject, int outputFrameCount);
            private static int CalcInputFramesImpl(IntPtr thisObject, int outputFrameCount)
            {
                var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                var callback = (AudioProcessor)shadow.Callback;
                return callback.CalcInputFrames(outputFrameCount);
            }

            /// <summary>	
            /// Returns the number of output frames that will be generated from a given number of input frames.	
            /// </summary>	
            /// <param name="thisObject">This Pointer</param>
            /// <param name="inputFrameCount">The number of input frames.</param>
            /// <returns>No documentation.</returns>
            /// <unmanaged>UINT32 IXAPO::CalcOutputFrames([None] UINT32 InputFrameCount)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CalcOutputFramesDelegate(IntPtr thisObject, int inputFrameCount);
            private static int CalcOutputFramesImpl(IntPtr thisObject, int inputFrameCount)
            {
                var shadow = ToShadow<AudioProcessorShadow>(thisObject);
                var callback = (AudioProcessor) shadow.Callback;
                return callback.CalcOutputFrames(inputFrameCount);
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}