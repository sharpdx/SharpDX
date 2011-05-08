using System;
using System.Runtime.InteropServices;
using SharpDX.Multimedia;

namespace SharpDX.XAPO
{
    /// <summary>
    /// Internal AudioProcessorCallback
    /// </summary>
    /// IXAPO GUID
    [Guid("a90bc001-e897-e897-55e4-9e4700000000")]
    internal class AudioProcessorCallback : ComObjectCallback
    {
        private readonly AudioProcessor Callback;
        private readonly ParameterProviderCallback _parameterProviderCallback;

        internal AudioProcessorCallback(AudioProcessor callback) : base(callback, 10)        
        {
            unsafe
            {
                Callback = callback;
                if (Callback is ParameterProvider)
                    _parameterProviderCallback = new ParameterProviderCallback(Callback as ParameterProvider);
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

        protected unsafe override int QueryInterfaceImpl(IntPtr thisPointer, Guid* guid, out IntPtr output)
        {
            if (*guid == typeof (ParameterProvider).GUID)
            {
                if (_parameterProviderCallback != null)
                {
                    output = _parameterProviderCallback.NativePointer;
                    return Result.Ok.Code;
                }
                output = IntPtr.Zero;
                return Result.NoInterface.Code;
            }
            return base.QueryInterfaceImpl(thisPointer, guid, out output);
        }

        /// <unmanaged>HRESULT IXAPO::GetRegistrationProperties([Out] XAPO_REGISTRATION_PROPERTIES** ppRegistrationProperties)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetRegistrationPropertiesDelegate(IntPtr thisObject, out IntPtr output);
        private int GetRegistrationPropertiesImpl(IntPtr thisObject, out IntPtr output)
        {
            output = IntPtr.Zero;
            try
            {
                int sizeOfNative = Utilities.SizeOf<RegistrationProperties.__Native>();
                output = Marshal.AllocCoTaskMem(sizeOfNative);
                RegistrationProperties.__Native temp = default(RegistrationProperties.__Native);
                Callback.RegistrationProperties.__MarshalTo(ref temp);
                Utilities.Write(output, ref temp);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>HRESULT IXAPO::IsInputFormatSupported([None] const WAVEFORMATEX* pOutputFormat,[None] const WAVEFORMATEX* pRequestedInputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedInputFormat)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int IsInputFormatSupportedDelegate(IntPtr thisObject, IntPtr outputFormat, IntPtr requestedInputFormat, out IntPtr supportedInputFormat);
        private int IsInputFormatSupportedImpl(IntPtr thisObject, IntPtr pOutputFormat, IntPtr pRequestedInputFormat, out IntPtr pSupportedInputFormat)
        {
            pSupportedInputFormat = IntPtr.Zero;
            try
            {
                WaveFormat outputFormat = WaveFormat.MarshalFromPtr(pOutputFormat);
                WaveFormat requestedInputFormat = WaveFormat.MarshalFromPtr(pRequestedInputFormat);

                WaveFormat supportedInputFormat;
                var result = Callback.IsInputFormatSupported(outputFormat, requestedInputFormat, out supportedInputFormat);

                int sizeOfWaveFormat = Marshal.SizeOf(supportedInputFormat);
                pSupportedInputFormat = Marshal.AllocCoTaskMem(sizeOfWaveFormat);
                
                Marshal.StructureToPtr(supportedInputFormat, pSupportedInputFormat, false);

                // return XAPO_E_FORMAT_UNSUPPORTED if fail
                return result ? Result.Ok.Code : unchecked((int)0x88970001);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
        }

        /// <unmanaged>HRESULT IXAPO::IsOutputFormatSupported([None] const WAVEFORMATEX* pInputFormat,[None] const WAVEFORMATEX* pRequestedOutputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedOutputFormat)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int IsOutputFormatSupportedDelegate(IntPtr thisObject, IntPtr outputFormat, IntPtr requestedInputFormat, out IntPtr supportedInputFormat);
        private int IsOutputFormatSupportedImpl(IntPtr thisObject, IntPtr pInputFormat, IntPtr pRequestedOutputFormat, out IntPtr pSupportedOutputFormat)
        {
            pSupportedOutputFormat = IntPtr.Zero;
            try
            {
                WaveFormat inputFormat = WaveFormat.MarshalFromPtr(pInputFormat);
                WaveFormat requestedOutputFormat = WaveFormat.MarshalFromPtr(pRequestedOutputFormat);

                WaveFormat supportedOutputFormat;
                var result = Callback.IsOutputFormatSupported(inputFormat, requestedOutputFormat, out supportedOutputFormat);

                int sizeOfWaveFormat = Marshal.SizeOf(supportedOutputFormat);
                pSupportedOutputFormat = Marshal.AllocCoTaskMem(sizeOfWaveFormat);

                Marshal.StructureToPtr(supportedOutputFormat, pSupportedOutputFormat, false);

                // return XAPO_E_FORMAT_UNSUPPORTED if fail
                return result ? Result.Ok.Code : unchecked((int)0x88970001);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
        }

        /// <unmanaged>HRESULT IXAPO::Initialize([In, Buffer, Optional] const void* pData,[None] UINT32 DataByteSize)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int InitializeDelegate(IntPtr thisObject, IntPtr ptr, int dataSize);
        private int InitializeImpl(IntPtr thisObject, IntPtr ptr, int dataSize)
        {
            try
            {
                Callback.Initialize(new DataStream(ptr, dataSize, true, true));
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>void IXAPO::Reset()</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ResetDelegate();
        private void ResetImpl()
        {
            Callback.Reset();
        }

        /// <unmanaged>HRESULT IXAPO::LockForProcess([None] UINT32 InputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pInputLockedParameters,[None] UINT32 OutputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pOutputLockedParameters)</unmanaged>
        //void LockForProcess(SharpDX.XAPO.LockforprocessParameters[] inputLockedParameters, SharpDX.XAPO.LockforprocessParameters[] outputLockedParameters);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int LockForProcessDelegate(IntPtr thisObject, 
            int inputLockedParameterCount, LockParameters.__Native* pInputLockedParameters, int outputLockedParameterCount, LockParameters.__Native* pOutputLockedParameters);
        private unsafe int LockForProcessImpl(IntPtr thisObject, 
            int inputLockedParameterCount, LockParameters.__Native* pInputLockedParameters, int outputLockedParameterCount, LockParameters.__Native* pOutputLockedParameters)
        {
            try
            {
                LockParameters[] inputLockedParameters = new LockParameters[inputLockedParameterCount];
                for (int i = 0; i < inputLockedParameters.Length; i++)
                {
                    var param = new LockParameters();                    
                    param.__MarshalFrom(&pInputLockedParameters[i]);
                    inputLockedParameters[i] = param;
                }

                LockParameters[] ouputLockedParameters = new LockParameters[outputLockedParameterCount];
                for (int i = 0; i < ouputLockedParameters.Length; i++)
                {
                    var param = new LockParameters();
                    param.__MarshalFrom(&pOutputLockedParameters[i]);
                    ouputLockedParameters[i] = param;
                }

                return Callback.LockForProcess(inputLockedParameters, ouputLockedParameters).Code;
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
        }

        /// <summary>	
        /// Deallocates variables that were allocated with the {{LockForProcess}} method.	
        /// </summary>	
        /// <unmanaged>void IXAPO::UnlockForProcess()</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void UnlockForProcessDelegate(IntPtr thisObject);
        private void UnlockForProcessImpl(IntPtr thisObject)
        {
            Callback.UnlockForProcess();
        }

        /// <unmanaged>void IXAPO::Process([None] UINT32 InputProcessParameterCount,[In, Buffer, Optional] const XAPO_PROCESS_BUFFER_PARAMETERS* pInputProcessParameters,[None] UINT32 OutputProcessParameterCount,[InOut, Buffer, Optional] XAPO_PROCESS_BUFFER_PARAMETERS* pOutputProcessParameters,[None] BOOL IsEnabled)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate void ProcessDelegate(IntPtr thisObject,
            int inputProcessParameterCount, BufferParameters* pInputProcessParameters, int outputProcessParameterCount, BufferParameters* inputProcessParameters,
            int isEnabled);
        private unsafe void ProcessImpl(IntPtr thisObject,
            int inputProcessParameterCount, BufferParameters* pInputProcessParameters, int outputProcessParameterCount, BufferParameters* pOutputProcessParameters,
            int isEnabled)
        {
            BufferParameters[] inputProcessParameters = new BufferParameters[inputProcessParameterCount];
            for (int i = 0; i < inputProcessParameters.Length; i++)
                inputProcessParameters[i] = pInputProcessParameters[i];

            BufferParameters[] outputProcessParameters = new BufferParameters[outputProcessParameterCount];
            for (int i = 0; i < outputProcessParameters.Length; i++)
                outputProcessParameters[i] = pOutputProcessParameters[i];

            //// NOTE: because XAudio currently support only 1 input and 1 output buffer at a time, don't waste our time
            //BufferParameters outputParameter = *pOutputProcessParameters;

            Callback.Process(inputProcessParameters, outputProcessParameters, isEnabled == 1);

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
        private int CalcInputFramesImpl(IntPtr thisObject, int outputFrameCount)
        {
            return Callback.CalcInputFrames(outputFrameCount);
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
        private int CalcOutputFramesImpl(IntPtr thisObject, int inputFrameCount)
        {
            return Callback.CalcOutputFrames(inputFrameCount);
        }
    }
}