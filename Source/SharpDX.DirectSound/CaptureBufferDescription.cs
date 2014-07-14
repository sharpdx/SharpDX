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
using SharpDX.Multimedia;

namespace SharpDX.DirectSound
{
    public partial class CaptureBufferDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureBufferDescription"/> class.
        /// </summary>
        public CaptureBufferDescription()
        {
            unsafe
            {
                Size = sizeof (CaptureBufferDescription.__Native);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [wave mapped].
        /// </summary>
        /// <value><c>true</c> if [wave mapped]; otherwise, <c>false</c>.</value>
        public bool WaveMapped
        {
            get
            {
                return (Flags & CaptureBufferCapabilitiesFlags.WaveMapped) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [control effects].
        /// </summary>
        /// <value><c>true</c> if [control effects]; otherwise, <c>false</c>.</value>
        public bool ControlEffects
        {
            get
            {
                return (Flags & CaptureBufferCapabilitiesFlags.ControlEffects) != 0;
            }
        }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int Size;
            public SharpDX.DirectSound.CaptureBufferCapabilitiesFlags Flags;
            public int BufferBytes;
            public int Reserved;
            public IntPtr FormatPointer;
            public int EffectCount;
            public IntPtr EffectDescriptionPointer;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (FormatPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(FormatPointer);
                if (EffectCount > 0 && EffectDescriptionPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(EffectDescriptionPointer);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.Flags = @ref.Flags;
            this.BufferBytes = @ref.BufferBytes;
            this.Reserved = @ref.Reserved;
            this.Format = WaveFormat.MarshalFrom(@ref.FormatPointer);
            this.EffectCount = @ref.EffectCount;
            if (EffectCount > 0)
            {
                var nativeDescriptions = new CaptureEffectDescription.__Native[EffectCount];
                Utilities.Read(@ref.EffectDescriptionPointer, nativeDescriptions, 0, EffectCount);

                EffectDescriptions = new CaptureEffectDescription[EffectCount];
                for (int i = 0; i < EffectCount; i++)
                {
                    EffectDescriptions[i] = new CaptureEffectDescription();
                    EffectDescriptions[i].__MarshalFrom(ref nativeDescriptions[i]);
                }
            }
        }

        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.Flags = this.Flags;
            @ref.BufferBytes = this.BufferBytes;
            @ref.Reserved = this.Reserved;
            @ref.FormatPointer = WaveFormat.MarshalToPtr(this.Format);

            int effectCount = EffectDescriptions == null ? 0 : EffectDescriptions.Length;
            @ref.EffectCount = effectCount;
            if (effectCount > 0)
            {
                var nativeDescriptions = new CaptureEffectDescription.__Native[effectCount];
                for(int i = 0; i < effectCount; i++)
                {
                    nativeDescriptions[i] = CaptureEffectDescription.__NewNative();
                    EffectDescriptions[i].__MarshalTo(ref nativeDescriptions[i]);
                }

                @ref.EffectDescriptionPointer =  Marshal.AllocHGlobal(effectCount*Utilities.SizeOf<CaptureEffectDescription.__Native>());
                Utilities.Write(@ref.EffectDescriptionPointer, nativeDescriptions, 0, effectCount);
            }
        }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                return temp;
            }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public WaveFormat Format { get; set; }

        /// <summary>
        /// Describes effects supported by hardware for the buffer.
        /// </summary>
        public CaptureEffectDescription[] EffectDescriptions { get; set;}
    }
}