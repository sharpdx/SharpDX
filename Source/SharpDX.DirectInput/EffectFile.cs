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
    public partial class EffectFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFile"/> class.
        /// </summary>
        public EffectFile()
        {
            unsafe
            {
                Size = sizeof(__Native);
            }
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public EffectParameters Parameters { get; set; }
      
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
            public System.Guid Guid;
            public System.IntPtr EffectParametersPointer;
            public byte Name;
            public byte _Name2;
            public byte _Name3;
            public byte _Name4;
            public byte _Name5;
            public byte _Name6;
            public byte _Name7;
            public byte _Name8;
            public byte _Name9;
            public byte _Name10;
            public byte _Name11;
            public byte _Name12;
            public byte _Name13;
            public byte _Name14;
            public byte _Name15;
            public byte _Name16;
            public byte _Name17;
            public byte _Name18;
            public byte _Name19;
            public byte _Name20;
            public byte _Name21;
            public byte _Name22;
            public byte _Name23;
            public byte _Name24;
            public byte _Name25;
            public byte _Name26;
            public byte _Name27;
            public byte _Name28;
            public byte _Name29;
            public byte _Name30;
            public byte _Name31;
            public byte _Name32;
            public byte _Name33;
            public byte _Name34;
            public byte _Name35;
            public byte _Name36;
            public byte _Name37;
            public byte _Name38;
            public byte _Name39;
            public byte _Name40;
            public byte _Name41;
            public byte _Name42;
            public byte _Name43;
            public byte _Name44;
            public byte _Name45;
            public byte _Name46;
            public byte _Name47;
            public byte _Name48;
            public byte _Name49;
            public byte _Name50;
            public byte _Name51;
            public byte _Name52;
            public byte _Name53;
            public byte _Name54;
            public byte _Name55;
            public byte _Name56;
            public byte _Name57;
            public byte _Name58;
            public byte _Name59;
            public byte _Name60;
            public byte _Name61;
            public byte _Name62;
            public byte _Name63;
            public byte _Name64;
            public byte _Name65;
            public byte _Name66;
            public byte _Name67;
            public byte _Name68;
            public byte _Name69;
            public byte _Name70;
            public byte _Name71;
            public byte _Name72;
            public byte _Name73;
            public byte _Name74;
            public byte _Name75;
            public byte _Name76;
            public byte _Name77;
            public byte _Name78;
            public byte _Name79;
            public byte _Name80;
            public byte _Name81;
            public byte _Name82;
            public byte _Name83;
            public byte _Name84;
            public byte _Name85;
            public byte _Name86;
            public byte _Name87;
            public byte _Name88;
            public byte _Name89;
            public byte _Name90;
            public byte _Name91;
            public byte _Name92;
            public byte _Name93;
            public byte _Name94;
            public byte _Name95;
            public byte _Name96;
            public byte _Name97;
            public byte _Name98;
            public byte _Name99;
            public byte _Name100;
            public byte _Name101;
            public byte _Name102;
            public byte _Name103;
            public byte _Name104;
            public byte _Name105;
            public byte _Name106;
            public byte _Name107;
            public byte _Name108;
            public byte _Name109;
            public byte _Name110;
            public byte _Name111;
            public byte _Name112;
            public byte _Name113;
            public byte _Name114;
            public byte _Name115;
            public byte _Name116;
            public byte _Name117;
            public byte _Name118;
            public byte _Name119;
            public byte _Name120;
            public byte _Name121;
            public byte _Name122;
            public byte _Name123;
            public byte _Name124;
            public byte _Name125;
            public byte _Name126;
            public byte _Name127;
            public byte _Name128;
            public byte _Name129;
            public byte _Name130;
            public byte _Name131;
            public byte _Name132;
            public byte _Name133;
            public byte _Name134;
            public byte _Name135;
            public byte _Name136;
            public byte _Name137;
            public byte _Name138;
            public byte _Name139;
            public byte _Name140;
            public byte _Name141;
            public byte _Name142;
            public byte _Name143;
            public byte _Name144;
            public byte _Name145;
            public byte _Name146;
            public byte _Name147;
            public byte _Name148;
            public byte _Name149;
            public byte _Name150;
            public byte _Name151;
            public byte _Name152;
            public byte _Name153;
            public byte _Name154;
            public byte _Name155;
            public byte _Name156;
            public byte _Name157;
            public byte _Name158;
            public byte _Name159;
            public byte _Name160;
            public byte _Name161;
            public byte _Name162;
            public byte _Name163;
            public byte _Name164;
            public byte _Name165;
            public byte _Name166;
            public byte _Name167;
            public byte _Name168;
            public byte _Name169;
            public byte _Name170;
            public byte _Name171;
            public byte _Name172;
            public byte _Name173;
            public byte _Name174;
            public byte _Name175;
            public byte _Name176;
            public byte _Name177;
            public byte _Name178;
            public byte _Name179;
            public byte _Name180;
            public byte _Name181;
            public byte _Name182;
            public byte _Name183;
            public byte _Name184;
            public byte _Name185;
            public byte _Name186;
            public byte _Name187;
            public byte _Name188;
            public byte _Name189;
            public byte _Name190;
            public byte _Name191;
            public byte _Name192;
            public byte _Name193;
            public byte _Name194;
            public byte _Name195;
            public byte _Name196;
            public byte _Name197;
            public byte _Name198;
            public byte _Name199;
            public byte _Name200;
            public byte _Name201;
            public byte _Name202;
            public byte _Name203;
            public byte _Name204;
            public byte _Name205;
            public byte _Name206;
            public byte _Name207;
            public byte _Name208;
            public byte _Name209;
            public byte _Name210;
            public byte _Name211;
            public byte _Name212;
            public byte _Name213;
            public byte _Name214;
            public byte _Name215;
            public byte _Name216;
            public byte _Name217;
            public byte _Name218;
            public byte _Name219;
            public byte _Name220;
            public byte _Name221;
            public byte _Name222;
            public byte _Name223;
            public byte _Name224;
            public byte _Name225;
            public byte _Name226;
            public byte _Name227;
            public byte _Name228;
            public byte _Name229;
            public byte _Name230;
            public byte _Name231;
            public byte _Name232;
            public byte _Name233;
            public byte _Name234;
            public byte _Name235;
            public byte _Name236;
            public byte _Name237;
            public byte _Name238;
            public byte _Name239;
            public byte _Name240;
            public byte _Name241;
            public byte _Name242;
            public byte _Name243;
            public byte _Name244;
            public byte _Name245;
            public byte _Name246;
            public byte _Name247;
            public byte _Name248;
            public byte _Name249;
            public byte _Name250;
            public byte _Name251;
            public byte _Name252;
            public byte _Name253;
            public byte _Name254;
            public byte _Name255;
            public byte _Name256;
            public byte _Name257;
            public byte _Name258;
            public byte _Name259;
            public byte _Name260;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (EffectParametersPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(EffectParametersPointer);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            // Free Parameters
            if (Parameters != null && @ref.EffectParametersPointer != IntPtr.Zero)
                Parameters.__MarshalFree(ref *((EffectParameters.__Native*)@ref.EffectParametersPointer));

            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.Guid = @ref.Guid;
            this.EffectParametersPointer = @ref.EffectParametersPointer;
            fixed (void* __ptr = &@ref.Name) this.Name = Utilities.PtrToStringAnsi((IntPtr)__ptr, 260);

            if (this.EffectParametersPointer != IntPtr.Zero)
            {
                Parameters = new EffectParameters();
                Parameters.__MarshalFrom(ref *(EffectParameters.__Native*)EffectParametersPointer);
                EffectParametersPointer = IntPtr.Zero;
            }
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.Guid = this.Guid;
            IntPtr effectParameters = IntPtr.Zero;
            if ( Parameters != null)
            {
                effectParameters = Marshal.AllocHGlobal(sizeof (EffectParameters.__Native));
                var nativeParameters = default(EffectParameters.__Native);
                Parameters.__MarshalTo(ref nativeParameters);
                *((EffectParameters.__Native*) effectParameters) = nativeParameters;
            }

            @ref.EffectParametersPointer = effectParameters;
            IntPtr Name_ = Marshal.StringToHGlobalAnsi(this.Name);
            fixed (void* __ptr = &@ref.Name) Utilities.CopyMemory((IntPtr)__ptr, Name_, this.Name.Length);
            Marshal.FreeHGlobal(Name_);
        }
    }
}