using System;
using System.Runtime.InteropServices;

namespace SharpDX.X3DAudio
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='X3DAUDIO_DSP_SETTINGS']/*"/>	
    /// <unmanaged>X3DAUDIO_DSP_SETTINGS</unmanaged>	
    public  partial class DspSettings
    {
        public float[] MatrixCoefficients;

        public float[] DelayTimes;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal partial struct __Native
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
                if (MatrixCoefficientsPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(MatrixCoefficientsPointer);
                if (DelayTimesPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(DelayTimesPointer);
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
            //this.MatrixCoefficientsPointer = @ref.MatrixCoefficientsPointer;
            //this.DelayTimesPointer = @ref.DelayTimesPointer;

            MatrixCoefficients = new float[@ref.SrcChannelCount * @ref.DstChannelCount];
            if (MatrixCoefficients.Length > 0)
                Utilities.Read(@ref.MatrixCoefficientsPointer, MatrixCoefficients, 0, MatrixCoefficients.Length);

            DelayTimes = new float[@ref.DstChannelCount];
            if (DelayTimes.Length > 0)
                Utilities.Read(@ref.DelayTimesPointer, DelayTimes, 0, DelayTimes.Length);

            this.SourceChannelCount = @ref.SrcChannelCount;
            this.DestinationChannelCount = @ref.DstChannelCount;
            this.LpfDirectCoefficient = @ref.LPFDirectCoefficient;
            this.LpfReverbCoefficient = @ref.LPFReverbCoefficient;
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
            @ref.MatrixCoefficientsPointer = Marshal.AllocHGlobal(SourceChannelCount * DestinationChannelCount * sizeof(float));
            @ref.DelayTimesPointer = Marshal.AllocHGlobal(DestinationChannelCount * sizeof(float));
            @ref.SrcChannelCount = this.SourceChannelCount;
            @ref.DstChannelCount = this.DestinationChannelCount;
            @ref.LPFDirectCoefficient = this.LpfDirectCoefficient;
            @ref.LPFReverbCoefficient = this.LpfReverbCoefficient;
            @ref.ReverbLevel = this.ReverbLevel;
            @ref.DopplerFactor = this.DopplerFactor;
            @ref.EmitterToListenerAngle = this.EmitterToListenerAngle;
            @ref.EmitterToListenerDistance = this.EmitterToListenerDistance;
            @ref.EmitterVelocityComponent = this.EmitterVelocityComponent;
            @ref.ListenerVelocityComponent = this.ListenerVelocityComponent;

        }
    }
}