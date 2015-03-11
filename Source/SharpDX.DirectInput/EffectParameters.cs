// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    public partial class EffectParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameters"/> class.
        /// </summary>
        public EffectParameters()
        {
            unsafe
            {
                Size = sizeof(__Native);
            }
        }

        /// <summary>
        /// Optional Envelope structure that describes the envelope to be used by this effect. Not all effect types use envelope. If no envelope is to be applied, the member should be set to null. 
        /// </summary>
        public Envelope Envelope { get; set; }

        /// <summary>
        /// Gets or sets an array containing identifiers or offsets identifying the axes to which the effect is to be applied. 
        /// The flags <see cref="EffectFlags.ObjectIds"/> and <see cref="EffectFlags.ObjectOffsets"/> determine the semantics of the values in the array.
        /// The list of axes associated with an effect cannot be changed once it has been set.
        /// No more than 32 axes can be associated with a single effect. 
        /// </summary>
        /// <value>The axes.</value>
        public int[] Axes { get; set; }

        /// <summary>
        /// Gets or sets an array containing either Cartesian coordinates, polar coordinates, or spherical coordinates. 
        /// The flags <see cref="EffectFlags.Cartesian"/>, <see cref="EffectFlags.Polar"/>, and <see cref="EffectFlags.Spherical"/> determine the semantics of the values in the array.
        /// If Cartesian, each value in Directions is associated with the corresponding axis in Axes.
        /// If polar, the angle is measured in hundredths of degrees from the (0, - 1) direction, rotated in the direction of (1, 0). This usually means that north is away from the user, and east is to the user's right. The last element is not used.
        /// If spherical, the first angle is measured in hundredths of a degree from the (1, 0) direction, rotated in the direction of (0, 1). The second angle (if the number of axes is three or more) is measured in hundredths of a degree toward (0, 0, 1). The third angle (if the number of axes is four or more) is measured in hundredths of a degree toward (0, 0, 0, 1), and so on. The last element is not used. 
        /// </summary>
        /// <value>The directions.</value>
        public int[] Directions { get; set; }

        /// <summary>
        /// Gets the axes. See <see cref="Axes"/> and <see cref="Directions"/>.
        /// </summary>
        /// <param name="axes">The axes.</param>
        /// <param name="directions">The directions.</param>
        public void GetAxes(out int[] axes, out int[] directions)
        {
            axes = Axes;
            directions = Directions;
        }

        /// <summary>
        /// Sets the axes. See <see cref="Axes"/> and <see cref="Directions"/>.
        /// </summary>
        /// <param name="axes">The axes.</param>
        /// <param name="directions">The directions.</param>
        public void SetAxes(int[] axes, int[] directions)
        {
            Axes = axes;
            Directions = directions;
        }

        /// <summary>
        /// Gets or sets the type specific parameter.
        /// Reference to a type-specific parameters, or null  if there are no type-specific parameters.
        /// If the effect is of type <see cref="EffectType.Condition"/>, this member contains an indirect reference to a ConditionSet structures that define the parameters for the condition. A single structure may be used, in which case the condition is applied in the direction specified in the Directions array. Otherwise, there must be one structure for each axis, in the same order as the axes in rgdwAxes array. If a structure is supplied for each axis, the effect should not be rotated; you should use the following values in the Directions array: <see cref="EffectFlags.Spherical"/> : 0, 0, ... / <see cref="EffectFlags.Polar"/>: 9000, 0, ... / <see cref="EffectFlags.Cartesian"/>: 1, 0, ...
        /// If the effect is of type <see cref="EffectType.CustomForce"/>, this member contains an indirect reference to a <see cref="CustomForce"/> that defines the parameters for the custom force.
        /// If the effect is of type <see cref="EffectType.Periodic"/>, this member contains a pointer to a <see cref="PeriodicForce"/> that defines the parameters for the effect.
        /// If the effect is of type <see cref="EffectType.ConstantForce"/>, this member contains a pointer to a <see cref="ConstantForce"/> that defines the parameters for the constant force.
        /// If the effect is of type <see cref="EffectType.RampForce"/>, this member contains a pointer to a <see cref="RampForce"/> that defines the parameters for the ramp force. 
        /// To gain access to the underlying structure, you need to call the method <see cref="TypeSpecificParameters.As{T}"/>.
        /// </summary>
        /// <value>The type specific parameter.</value>
        public TypeSpecificParameters Parameters { get; set; }


        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                return temp;
            }
        }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int Size;
            public SharpDX.DirectInput.EffectFlags Flags;
            public int Duration;
            public int SamplePeriod;
            public int Gain;
            public int TriggerButton;
            public int TriggerRepeatInterval;
            public int AxeCount;
            public System.IntPtr AxePointer;
            public System.IntPtr DirectionPointer;
            public System.IntPtr EnvelopePointer;
            public int TypeSpecificParamCount;
            public System.IntPtr TypeSpecificParamPointer;
            public int StartDelay;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (AxePointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(AxePointer);
                if (DirectionPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(DirectionPointer);
                if (EnvelopePointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(EnvelopePointer);               
                //if (TypeSpecificParamPointer != IntPtr.Zero)
                //    Marshal.FreeHGlobal(TypeSpecificParamPointer);
            }
        }

        // Method to free native struct
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            if (Parameters != null && @ref.TypeSpecificParamPointer != IntPtr.Zero)
                Parameters.MarshalFree(@ref.TypeSpecificParamPointer);

            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.Flags = @ref.Flags;
            this.Duration = @ref.Duration;
            this.SamplePeriod = @ref.SamplePeriod;
            this.Gain = @ref.Gain;
            this.TriggerButton = @ref.TriggerButton;
            this.TriggerRepeatInterval = @ref.TriggerRepeatInterval;
            this.AxeCount = @ref.AxeCount;
            this.StartDelay = @ref.StartDelay;

            // Marshal Axes and Directions
            if (AxeCount > 0)
            {
                if (@ref.AxePointer != IntPtr.Zero)
                {
                    Axes = new int[AxeCount];
                    Marshal.Copy(@ref.AxePointer, Axes, 0, AxeCount);
                }

                if (@ref.DirectionPointer != IntPtr.Zero)
                {
                    Directions = new int[AxeCount];
                    Marshal.Copy(@ref.DirectionPointer, Directions, 0, AxeCount);
                }
            }

            // Marshal Envelope
            if (@ref.EnvelopePointer != IntPtr.Zero)
            {
                var envelopeNative = *((Envelope.__Native*) @ref.EnvelopePointer);
                Envelope = new Envelope();
                Envelope.__MarshalFrom(ref envelopeNative);
            }

            // Marshal TypeSpecificParameters
            if (@ref.TypeSpecificParamCount > 0 && @ref.TypeSpecificParamPointer != IntPtr.Zero)
                Parameters = new TypeSpecificParameters(@ref.TypeSpecificParamCount, @ref.TypeSpecificParamPointer);

        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.Flags = this.Flags;
            @ref.Duration = this.Duration;
            @ref.SamplePeriod = this.SamplePeriod;
            @ref.Gain = this.Gain;
            @ref.TriggerButton = this.TriggerButton;
            @ref.TriggerRepeatInterval = this.TriggerRepeatInterval;
            @ref.StartDelay = this.StartDelay;

            // Marshal Axes and Directions
            @ref.AxeCount = 0; 
            @ref.AxePointer = IntPtr.Zero;
            @ref.DirectionPointer = IntPtr.Zero;
            if (Axes != null && Axes.Length > 0)
            {
                @ref.AxeCount = Axes.Length;

                @ref.AxePointer = Marshal.AllocHGlobal(Axes.Length * sizeof(int));
                Utilities.Write(@ref.AxePointer, Axes, 0, Axes.Length);

                if (Directions != null && Directions.Length == Axes.Length)
                {
                    @ref.DirectionPointer = Marshal.AllocHGlobal(Directions.Length * sizeof(int));
                    Utilities.Write(@ref.DirectionPointer, Directions, 0, Directions.Length);
                }
            }

            // Marshal Envelope
            @ref.EnvelopePointer = IntPtr.Zero;
            if (Envelope != null)
            {
                @ref.EnvelopePointer = Marshal.AllocHGlobal(sizeof(Envelope.__Native));
                var envelopeNative = SharpDX.DirectInput.Envelope.__NewNative();
                Envelope.__MarshalTo(ref envelopeNative);
                Utilities.Write(@ref.EnvelopePointer, ref envelopeNative);
            }

            // Marshal TypeSpecificParameters
            @ref.TypeSpecificParamCount = 0;
            @ref.TypeSpecificParamPointer = IntPtr.Zero;
            if (Parameters != null)
            {
                @ref.TypeSpecificParamCount = Parameters.Size;
                @ref.TypeSpecificParamPointer = Parameters.MarshalTo();
            }
        }
    }
}