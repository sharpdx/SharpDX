using System;
using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;

namespace SharpDX.X3DAudio
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='X3DAUDIO_LISTENER']/*"/>	
    /// <unmanaged>X3DAUDIO_LISTENER</unmanaged>	
    public  partial class Listener
    {
        /// <summary>
        /// Reference to Cone data.
        /// </summary>
        /// <unmanaged>X3DAUDIO_CONE* pCone</unmanaged>	
        public Cone Cone;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal partial struct __Native
        {
            public RawVector3 OrientFront;
            public RawVector3 OrientTop;
            public RawVector3 Position;
            public RawVector3 Velocity;
            public System.IntPtr ConePointer;
            public Cone.__Native Cone;
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

        //// Method to marshal from native to managed struct
        /// Disabled as it is not used
        //internal unsafe void __MarshalFrom(ref __Native @ref)
        //{
        //    this.OrientFront = @ref.OrientFront;
        //    this.OrientTop = @ref.OrientTop;
        //    this.Position = @ref.Position;
        //    this.Velocity = @ref.Velocity;
        //    this.ConePointer = @ref.ConePointer;
        //}

        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.OrientFront = this.OrientFront;
            @ref.OrientTop = this.OrientTop;
            @ref.Position = this.Position;
            @ref.Velocity = this.Velocity;
            if (this.Cone == null)
            {
                @ref.ConePointer = IntPtr.Zero;
            }
            else
            {
                // We can marshal a pointer to inner Cone struct because Native is only allocated on the stack
                fixed (void* pCone = &@ref.Cone)
                    @ref.ConePointer = (IntPtr)pCone;
                this.Cone.__MarshalTo(ref @ref.Cone);
            }
        }
    }
}