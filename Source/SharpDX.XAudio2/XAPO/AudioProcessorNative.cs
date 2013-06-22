using System;
using SharpDX.Multimedia;

namespace SharpDX.XAPO
{

    /// <summary>
    /// Native Accessor to an existing AudioProcessor instance
    /// </summary>
    public partial class AudioProcessorNative
    {
        /// <summary>	
        /// Returns the registration properties of an XAPO.	
        /// </summary>	
        /// <returns> a <see cref="SharpDX.XAPO.RegistrationProperties"/> structure containing the registration properties the XAPO was created with; use XAPOFree to free the structure.</returns>
        /// <unmanaged>HRESULT IXAPO::GetRegistrationProperties([Out] XAPO_REGISTRATION_PROPERTIES** ppRegistrationProperties)</unmanaged>
        public RegistrationProperties RegistrationProperties
        {
            get {
                RegistrationProperties temp;
                GetRegistrationProperties_(out temp);
                return temp;
            }
        }

        /// <summary>	
        /// Queries if a specific input format is supported for a given output format.	
        /// </summary>	
        /// <param name="outputFormat">Output format.</param>
        /// <param name="requestedInputFormat">Input format to check for being supported.</param>
        /// <param name="supportedInputFormat"> If not NULL, and the input format is not supported for the given output format, ppSupportedInputFormat returns a  pointer to the closest input format that is supported. Use {{XAPOFree}} to free the returned structure. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAPO::IsInputFormatSupported([None] const WAVEFORMATEX* pOutputFormat,[None] const WAVEFORMATEX* pRequestedInputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedInputFormat)</unmanaged>
        public bool IsInputFormatSupported(WaveFormat outputFormat, WaveFormat requestedInputFormat, out WaveFormat supportedInputFormat)
        {
            return IsInputFormatSupported_(ref outputFormat, ref requestedInputFormat, out supportedInputFormat).Success;
        }

        /// <summary>	
        /// Queries if a specific output format is supported for a given input format.	
        /// </summary>	
        /// <param name="inputFormat">[in]  Input format. </param>
        /// <param name="requestedOutputFormat">[in]  Output format to check for being supported. </param>
        /// <param name="supportedOutputFormat">[out]  If not NULL and the output format is not supported for the given input format, ppSupportedOutputFormat returns a pointer to the closest output format that is supported. Use {{XAPOFree}} to free the returned structure. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAPO::IsOutputFormatSupported([None] const WAVEFORMATEX* pInputFormat,[None] const WAVEFORMATEX* pRequestedOutputFormat,[Out, Optional] WAVEFORMATEX** ppSupportedOutputFormat)</unmanaged>
        public bool IsOutputFormatSupported(WaveFormat inputFormat, WaveFormat requestedOutputFormat, out WaveFormat supportedOutputFormat)
        {
            return IsOutputFormatSupported_(ref inputFormat, ref requestedOutputFormat, out supportedOutputFormat).Success;
        }

        /// <summary>	
        /// Performs any effect-specific initialization.	
        /// </summary>	
        /// <param name="stream"> Effect-specific initialization parameters, may be NULL if DataByteSize is 0. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAPO::Initialize([In, Buffer, Optional] const void* pData,[None] UINT32 DataByteSize)</unmanaged>
        public void Initialize(DataStream stream)
        {
            if (stream == null)
                Initialize_(IntPtr.Zero, 0);
            else
            Initialize_(stream.PositionPointer, (int)(stream.Length - stream.Position));
        }

        /// <summary>	
        /// Resets variables dependent on frame history. 	
        /// </summary>	
        /// <unmanaged>void IXAPO::Reset()</unmanaged>
        public void Reset()
        {
            Reset_();
        }

        /// <summary>	
        /// Called by XAudio2 to lock the input and output configurations of an XAPO allowing it to	
        /// do any final initialization before {{Process}} is called on the realtime thread.	
        /// </summary>	
        /// <param name="inputLockedParameters"> Array of input <see cref="SharpDX.XAPO.LockParameters"/> structures.pInputLockedParameters may be NULL if InputLockedParameterCount is 0, otherwise it must have InputLockedParameterCount elements.</param>
        /// <param name="outputLockedParameters"> Array of output <see cref="SharpDX.XAPO.LockParameters"/> structures.pOutputLockedParameters may be NULL if OutputLockedParameterCount is 0, otherwise it must have OutputLockedParameterCount elements.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAPO::LockForProcess([None] UINT32 InputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pInputLockedParameters,[None] UINT32 OutputLockedParameterCount,[In, Buffer, Optional] const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS* pOutputLockedParameters)</unmanaged>
        public void LockForProcess(LockParameters[] inputLockedParameters, LockParameters[] outputLockedParameters)
        {
            LockForProcess_(inputLockedParameters.Length, inputLockedParameters, outputLockedParameters.Length, outputLockedParameters);
        }

        /// <summary>	
        /// Deallocates variables that were allocated with the {{LockForProcess}} method.	
        /// </summary>	
        /// <unmanaged>void IXAPO::UnlockForProcess()</unmanaged>
        public void UnlockForProcess()
        {
            UnlockForProcess_();
        }

        /// <summary>	
        /// Runs the XAPO's digital signal processing (DSP) code on the given input and output buffers.	
        /// </summary>	
        /// <param name="inputProcessParameters">[in]          Input array of  <see cref="SharpDX.XAPO.BufferParameters"/> structures.         </param>
        /// <param name="outputProcessParameters">[in, out]          Output array of <see cref="SharpDX.XAPO.BufferParameters"/> structures.  On input, the value of <see cref="SharpDX.XAPO.BufferParameters"/>.ValidFrameCount indicates the number of frames  that the XAPO should write to the output buffer.  On output, the value of <see cref="SharpDX.XAPO.BufferParameters"/>.ValidFrameCount indicates the actual number of frames written.         </param>
        /// <param name="isEnabled"> TRUE to process normally; FALSE to process thru.  See Remarks for additional information.         </param>
        /// <unmanaged>void IXAPO::Process([None] UINT32 InputProcessParameterCount,[In, Buffer, Optional] const XAPO_PROCESS_BUFFER_PARAMETERS* pInputProcessParameters,[None] UINT32 OutputProcessParameterCount,[InOut, Buffer, Optional] XAPO_PROCESS_BUFFER_PARAMETERS* pOutputProcessParameters,[None] BOOL IsEnabled)</unmanaged>
        public void Process(BufferParameters[] inputProcessParameters, BufferParameters[] outputProcessParameters, bool isEnabled)
        {
            Process_(inputProcessParameters.Length, inputProcessParameters, outputProcessParameters.Length, outputProcessParameters, isEnabled);
        }

        /// <summary>	
        /// Returns the number of input frames required to generate the given number of output frames.	
        /// </summary>	
        /// <param name="outputFrameCount">The number of output frames desired.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>UINT32 IXAPO::CalcInputFrames([None] UINT32 OutputFrameCount)</unmanaged>
        public int CalcInputFrames(int outputFrameCount)
        {
            return CalcInputFrames(outputFrameCount);
        }

        /// <summary>	
        /// Returns the number of output frames that will be generated from a given number of input frames.	
        /// </summary>	
        /// <param name="inputFrameCount">The number of input frames.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>UINT32 IXAPO::CalcOutputFrames([None] UINT32 InputFrameCount)</unmanaged>
        public int CalcOutputFrames(int inputFrameCount)
        {
            return CalcOutputFrames(inputFrameCount);
        }
    }
}