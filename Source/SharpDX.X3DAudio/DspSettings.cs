using System.Runtime.InteropServices;

namespace SharpDX.X3DAudio
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='X3DAUDIO_DSP_SETTINGS']/*"/>	
    /// <unmanaged>X3DAUDIO_DSP_SETTINGS</unmanaged>	
    public  partial class DspSettings {

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal unsafe partial struct __Native
        {
            public System.IntPtr MatrixCoefficientsPointer;
            public System.IntPtr DelayTimesPointer;
            public int SrcChannelCount;
            public int DstChannelCount;
            public float LPFDirectCoefficient;
            public float LPFReverbCoefficient;
            public float ReverbLevel;
            public float DopplerFactor;
            public float EmitterToListenerAngle;
            public float EmitterToListenerDistance;
            public float EmitterVelocityComponent;
            public float ListenerVelocityComponent;
            // Method to free unmanaged allocation
            internal unsafe void __MarshalFree()
            {
            }
        }

        // Method to free unmanaged allocation
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.MatrixCoefficientsPointer = @ref.MatrixCoefficientsPointer;
            this.DelayTimesPointer = @ref.DelayTimesPointer;
            this.SrcChannelCount = @ref.SrcChannelCount;
            this.DstChannelCount = @ref.DstChannelCount;
            this.LPFDirectCoefficient = @ref.LPFDirectCoefficient;
            this.LPFReverbCoefficient = @ref.LPFReverbCoefficient;
            this.ReverbLevel = @ref.ReverbLevel;
            this.DopplerFactor = @ref.DopplerFactor;
            this.EmitterToListenerAngle = @ref.EmitterToListenerAngle;
            this.EmitterToListenerDistance = @ref.EmitterToListenerDistance;
            this.EmitterVelocityComponent = @ref.EmitterVelocityComponent;
            this.ListenerVelocityComponent = @ref.ListenerVelocityComponent;
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.MatrixCoefficientsPointer = this.MatrixCoefficientsPointer;
            @ref.DelayTimesPointer = this.DelayTimesPointer;
            @ref.SrcChannelCount = this.SrcChannelCount;
            @ref.DstChannelCount = this.DstChannelCount;
            @ref.LPFDirectCoefficient = this.LPFDirectCoefficient;
            @ref.LPFReverbCoefficient = this.LPFReverbCoefficient;
            @ref.ReverbLevel = this.ReverbLevel;
            @ref.DopplerFactor = this.DopplerFactor;
            @ref.EmitterToListenerAngle = this.EmitterToListenerAngle;
            @ref.EmitterToListenerDistance = this.EmitterToListenerDistance;
            @ref.EmitterVelocityComponent = this.EmitterVelocityComponent;
            @ref.ListenerVelocityComponent = this.ListenerVelocityComponent;

        }
    }
}