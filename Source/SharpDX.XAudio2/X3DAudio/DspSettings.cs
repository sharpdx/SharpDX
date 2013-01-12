using System;
using System.Runtime.InteropServices;

namespace SharpDX.X3DAudio
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='X3DAUDIO_DSP_SETTINGS']/*"/>	
    /// <unmanaged>X3DAUDIO_DSP_SETTINGS</unmanaged>	
    public partial class DspSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspSettings" /> class.
        /// </summary>
        /// <param name="sourceChannelCount">The source channel count.</param>
        /// <param name="destinationChannelCount">The destination channel count.</param>
        public DspSettings(int sourceChannelCount, int destinationChannelCount)
        {
            SourceChannelCount = sourceChannelCount;
            DestinationChannelCount = destinationChannelCount;

            MatrixCoefficients = new float[sourceChannelCount * destinationChannelCount];
            DelayTimes = new float[destinationChannelCount];
        }

        /// <summary>
        /// The matrix coefficients
        /// </summary>
        public readonly float[] MatrixCoefficients;

        /// <summary>
        /// The delay times
        /// </summary>
        public readonly float[] DelayTimes;

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
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.LpfDirectCoefficient = @ref.LPFDirectCoefficient;
            this.LpfReverbCoefficient = @ref.LPFReverbCoefficient;
            this.ReverbLevel = @ref.ReverbLevel;
            this.DopplerFactor = @ref.DopplerFactor;
            this.EmitterToListenerAngle = @ref.EmitterToListenerAngle;
            this.EmitterToListenerDistance = @ref.EmitterToListenerDistance;
            this.EmitterVelocityComponent = @ref.EmitterVelocityComponent;
            this.ListenerVelocityComponent = @ref.ListenerVelocityComponent;
        }
    }
}