// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.XAudio2
{
    /// <summary>	
    /// Device role, only valid for XAudio27.	
    /// </summary>	
    /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_ROLE']/*" />	
    /// <unmanaged>XAUDIO2_DEVICE_ROLE</unmanaged>	
    /// <unmanaged-short>XAUDIO2_DEVICE_ROLE</unmanaged-short>	
    public enum DeviceRole
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='NotDefaultDevice']/*" />	
        /// <unmanaged>NotDefaultDevice</unmanaged>	
        /// <unmanaged-short>NotDefaultDevice</unmanaged-short>	
        NotDefaultDevice,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='DefaultConsoleDevice']/*" />	
        /// <unmanaged>DefaultConsoleDevice</unmanaged>	
        /// <unmanaged-short>DefaultConsoleDevice</unmanaged-short>	
        DefaultConsoleDevice,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='DefaultMultimediaDevice']/*" />	
        /// <unmanaged>DefaultMultimediaDevice</unmanaged>	
        /// <unmanaged-short>DefaultMultimediaDevice</unmanaged-short>	
        DefaultMultimediaDevice,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='DefaultCommunicationsDevice']/*" />	
        /// <unmanaged>DefaultCommunicationsDevice</unmanaged>	
        /// <unmanaged-short>DefaultCommunicationsDevice</unmanaged-short>	
        DefaultCommunicationsDevice = 4,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='DefaultGameDevice']/*" />	
        /// <unmanaged>DefaultGameDevice</unmanaged>	
        /// <unmanaged-short>DefaultGameDevice</unmanaged-short>	
        DefaultGameDevice = 8,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='GlobalDefaultDevice']/*" />	
        /// <unmanaged>GlobalDefaultDevice</unmanaged>	
        /// <unmanaged-short>GlobalDefaultDevice</unmanaged-short>	
        GlobalDefaultDevice = 15,
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='InvalidDeviceRole']/*" />	
        /// <unmanaged>InvalidDeviceRole</unmanaged>	
        /// <unmanaged-short>InvalidDeviceRole</unmanaged-short>	
        InvalidDeviceRole = -16
    }

    /// <summary>	
    /// Details of the device, only valid for XAudio27.	
    /// </summary>	
    /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_DETAILS']/*" />	
    /// <unmanaged>XAUDIO2_DEVICE_DETAILS</unmanaged>	
    /// <unmanaged-short>XAUDIO2_DEVICE_DETAILS</unmanaged-short>	
    public struct DeviceDetails
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct __Native
        {
            public char DeviceID;
            private char __DeviceID1;
            private char __DeviceID2;
            private char __DeviceID3;
            private char __DeviceID4;
            private char __DeviceID5;
            private char __DeviceID6;
            private char __DeviceID7;
            private char __DeviceID8;
            private char __DeviceID9;
            private char __DeviceID10;
            private char __DeviceID11;
            private char __DeviceID12;
            private char __DeviceID13;
            private char __DeviceID14;
            private char __DeviceID15;
            private char __DeviceID16;
            private char __DeviceID17;
            private char __DeviceID18;
            private char __DeviceID19;
            private char __DeviceID20;
            private char __DeviceID21;
            private char __DeviceID22;
            private char __DeviceID23;
            private char __DeviceID24;
            private char __DeviceID25;
            private char __DeviceID26;
            private char __DeviceID27;
            private char __DeviceID28;
            private char __DeviceID29;
            private char __DeviceID30;
            private char __DeviceID31;
            private char __DeviceID32;
            private char __DeviceID33;
            private char __DeviceID34;
            private char __DeviceID35;
            private char __DeviceID36;
            private char __DeviceID37;
            private char __DeviceID38;
            private char __DeviceID39;
            private char __DeviceID40;
            private char __DeviceID41;
            private char __DeviceID42;
            private char __DeviceID43;
            private char __DeviceID44;
            private char __DeviceID45;
            private char __DeviceID46;
            private char __DeviceID47;
            private char __DeviceID48;
            private char __DeviceID49;
            private char __DeviceID50;
            private char __DeviceID51;
            private char __DeviceID52;
            private char __DeviceID53;
            private char __DeviceID54;
            private char __DeviceID55;
            private char __DeviceID56;
            private char __DeviceID57;
            private char __DeviceID58;
            private char __DeviceID59;
            private char __DeviceID60;
            private char __DeviceID61;
            private char __DeviceID62;
            private char __DeviceID63;
            private char __DeviceID64;
            private char __DeviceID65;
            private char __DeviceID66;
            private char __DeviceID67;
            private char __DeviceID68;
            private char __DeviceID69;
            private char __DeviceID70;
            private char __DeviceID71;
            private char __DeviceID72;
            private char __DeviceID73;
            private char __DeviceID74;
            private char __DeviceID75;
            private char __DeviceID76;
            private char __DeviceID77;
            private char __DeviceID78;
            private char __DeviceID79;
            private char __DeviceID80;
            private char __DeviceID81;
            private char __DeviceID82;
            private char __DeviceID83;
            private char __DeviceID84;
            private char __DeviceID85;
            private char __DeviceID86;
            private char __DeviceID87;
            private char __DeviceID88;
            private char __DeviceID89;
            private char __DeviceID90;
            private char __DeviceID91;
            private char __DeviceID92;
            private char __DeviceID93;
            private char __DeviceID94;
            private char __DeviceID95;
            private char __DeviceID96;
            private char __DeviceID97;
            private char __DeviceID98;
            private char __DeviceID99;
            private char __DeviceID100;
            private char __DeviceID101;
            private char __DeviceID102;
            private char __DeviceID103;
            private char __DeviceID104;
            private char __DeviceID105;
            private char __DeviceID106;
            private char __DeviceID107;
            private char __DeviceID108;
            private char __DeviceID109;
            private char __DeviceID110;
            private char __DeviceID111;
            private char __DeviceID112;
            private char __DeviceID113;
            private char __DeviceID114;
            private char __DeviceID115;
            private char __DeviceID116;
            private char __DeviceID117;
            private char __DeviceID118;
            private char __DeviceID119;
            private char __DeviceID120;
            private char __DeviceID121;
            private char __DeviceID122;
            private char __DeviceID123;
            private char __DeviceID124;
            private char __DeviceID125;
            private char __DeviceID126;
            private char __DeviceID127;
            private char __DeviceID128;
            private char __DeviceID129;
            private char __DeviceID130;
            private char __DeviceID131;
            private char __DeviceID132;
            private char __DeviceID133;
            private char __DeviceID134;
            private char __DeviceID135;
            private char __DeviceID136;
            private char __DeviceID137;
            private char __DeviceID138;
            private char __DeviceID139;
            private char __DeviceID140;
            private char __DeviceID141;
            private char __DeviceID142;
            private char __DeviceID143;
            private char __DeviceID144;
            private char __DeviceID145;
            private char __DeviceID146;
            private char __DeviceID147;
            private char __DeviceID148;
            private char __DeviceID149;
            private char __DeviceID150;
            private char __DeviceID151;
            private char __DeviceID152;
            private char __DeviceID153;
            private char __DeviceID154;
            private char __DeviceID155;
            private char __DeviceID156;
            private char __DeviceID157;
            private char __DeviceID158;
            private char __DeviceID159;
            private char __DeviceID160;
            private char __DeviceID161;
            private char __DeviceID162;
            private char __DeviceID163;
            private char __DeviceID164;
            private char __DeviceID165;
            private char __DeviceID166;
            private char __DeviceID167;
            private char __DeviceID168;
            private char __DeviceID169;
            private char __DeviceID170;
            private char __DeviceID171;
            private char __DeviceID172;
            private char __DeviceID173;
            private char __DeviceID174;
            private char __DeviceID175;
            private char __DeviceID176;
            private char __DeviceID177;
            private char __DeviceID178;
            private char __DeviceID179;
            private char __DeviceID180;
            private char __DeviceID181;
            private char __DeviceID182;
            private char __DeviceID183;
            private char __DeviceID184;
            private char __DeviceID185;
            private char __DeviceID186;
            private char __DeviceID187;
            private char __DeviceID188;
            private char __DeviceID189;
            private char __DeviceID190;
            private char __DeviceID191;
            private char __DeviceID192;
            private char __DeviceID193;
            private char __DeviceID194;
            private char __DeviceID195;
            private char __DeviceID196;
            private char __DeviceID197;
            private char __DeviceID198;
            private char __DeviceID199;
            private char __DeviceID200;
            private char __DeviceID201;
            private char __DeviceID202;
            private char __DeviceID203;
            private char __DeviceID204;
            private char __DeviceID205;
            private char __DeviceID206;
            private char __DeviceID207;
            private char __DeviceID208;
            private char __DeviceID209;
            private char __DeviceID210;
            private char __DeviceID211;
            private char __DeviceID212;
            private char __DeviceID213;
            private char __DeviceID214;
            private char __DeviceID215;
            private char __DeviceID216;
            private char __DeviceID217;
            private char __DeviceID218;
            private char __DeviceID219;
            private char __DeviceID220;
            private char __DeviceID221;
            private char __DeviceID222;
            private char __DeviceID223;
            private char __DeviceID224;
            private char __DeviceID225;
            private char __DeviceID226;
            private char __DeviceID227;
            private char __DeviceID228;
            private char __DeviceID229;
            private char __DeviceID230;
            private char __DeviceID231;
            private char __DeviceID232;
            private char __DeviceID233;
            private char __DeviceID234;
            private char __DeviceID235;
            private char __DeviceID236;
            private char __DeviceID237;
            private char __DeviceID238;
            private char __DeviceID239;
            private char __DeviceID240;
            private char __DeviceID241;
            private char __DeviceID242;
            private char __DeviceID243;
            private char __DeviceID244;
            private char __DeviceID245;
            private char __DeviceID246;
            private char __DeviceID247;
            private char __DeviceID248;
            private char __DeviceID249;
            private char __DeviceID250;
            private char __DeviceID251;
            private char __DeviceID252;
            private char __DeviceID253;
            private char __DeviceID254;
            private char __DeviceID255;
            public char DisplayName;
            private char __DisplayName1;
            private char __DisplayName2;
            private char __DisplayName3;
            private char __DisplayName4;
            private char __DisplayName5;
            private char __DisplayName6;
            private char __DisplayName7;
            private char __DisplayName8;
            private char __DisplayName9;
            private char __DisplayName10;
            private char __DisplayName11;
            private char __DisplayName12;
            private char __DisplayName13;
            private char __DisplayName14;
            private char __DisplayName15;
            private char __DisplayName16;
            private char __DisplayName17;
            private char __DisplayName18;
            private char __DisplayName19;
            private char __DisplayName20;
            private char __DisplayName21;
            private char __DisplayName22;
            private char __DisplayName23;
            private char __DisplayName24;
            private char __DisplayName25;
            private char __DisplayName26;
            private char __DisplayName27;
            private char __DisplayName28;
            private char __DisplayName29;
            private char __DisplayName30;
            private char __DisplayName31;
            private char __DisplayName32;
            private char __DisplayName33;
            private char __DisplayName34;
            private char __DisplayName35;
            private char __DisplayName36;
            private char __DisplayName37;
            private char __DisplayName38;
            private char __DisplayName39;
            private char __DisplayName40;
            private char __DisplayName41;
            private char __DisplayName42;
            private char __DisplayName43;
            private char __DisplayName44;
            private char __DisplayName45;
            private char __DisplayName46;
            private char __DisplayName47;
            private char __DisplayName48;
            private char __DisplayName49;
            private char __DisplayName50;
            private char __DisplayName51;
            private char __DisplayName52;
            private char __DisplayName53;
            private char __DisplayName54;
            private char __DisplayName55;
            private char __DisplayName56;
            private char __DisplayName57;
            private char __DisplayName58;
            private char __DisplayName59;
            private char __DisplayName60;
            private char __DisplayName61;
            private char __DisplayName62;
            private char __DisplayName63;
            private char __DisplayName64;
            private char __DisplayName65;
            private char __DisplayName66;
            private char __DisplayName67;
            private char __DisplayName68;
            private char __DisplayName69;
            private char __DisplayName70;
            private char __DisplayName71;
            private char __DisplayName72;
            private char __DisplayName73;
            private char __DisplayName74;
            private char __DisplayName75;
            private char __DisplayName76;
            private char __DisplayName77;
            private char __DisplayName78;
            private char __DisplayName79;
            private char __DisplayName80;
            private char __DisplayName81;
            private char __DisplayName82;
            private char __DisplayName83;
            private char __DisplayName84;
            private char __DisplayName85;
            private char __DisplayName86;
            private char __DisplayName87;
            private char __DisplayName88;
            private char __DisplayName89;
            private char __DisplayName90;
            private char __DisplayName91;
            private char __DisplayName92;
            private char __DisplayName93;
            private char __DisplayName94;
            private char __DisplayName95;
            private char __DisplayName96;
            private char __DisplayName97;
            private char __DisplayName98;
            private char __DisplayName99;
            private char __DisplayName100;
            private char __DisplayName101;
            private char __DisplayName102;
            private char __DisplayName103;
            private char __DisplayName104;
            private char __DisplayName105;
            private char __DisplayName106;
            private char __DisplayName107;
            private char __DisplayName108;
            private char __DisplayName109;
            private char __DisplayName110;
            private char __DisplayName111;
            private char __DisplayName112;
            private char __DisplayName113;
            private char __DisplayName114;
            private char __DisplayName115;
            private char __DisplayName116;
            private char __DisplayName117;
            private char __DisplayName118;
            private char __DisplayName119;
            private char __DisplayName120;
            private char __DisplayName121;
            private char __DisplayName122;
            private char __DisplayName123;
            private char __DisplayName124;
            private char __DisplayName125;
            private char __DisplayName126;
            private char __DisplayName127;
            private char __DisplayName128;
            private char __DisplayName129;
            private char __DisplayName130;
            private char __DisplayName131;
            private char __DisplayName132;
            private char __DisplayName133;
            private char __DisplayName134;
            private char __DisplayName135;
            private char __DisplayName136;
            private char __DisplayName137;
            private char __DisplayName138;
            private char __DisplayName139;
            private char __DisplayName140;
            private char __DisplayName141;
            private char __DisplayName142;
            private char __DisplayName143;
            private char __DisplayName144;
            private char __DisplayName145;
            private char __DisplayName146;
            private char __DisplayName147;
            private char __DisplayName148;
            private char __DisplayName149;
            private char __DisplayName150;
            private char __DisplayName151;
            private char __DisplayName152;
            private char __DisplayName153;
            private char __DisplayName154;
            private char __DisplayName155;
            private char __DisplayName156;
            private char __DisplayName157;
            private char __DisplayName158;
            private char __DisplayName159;
            private char __DisplayName160;
            private char __DisplayName161;
            private char __DisplayName162;
            private char __DisplayName163;
            private char __DisplayName164;
            private char __DisplayName165;
            private char __DisplayName166;
            private char __DisplayName167;
            private char __DisplayName168;
            private char __DisplayName169;
            private char __DisplayName170;
            private char __DisplayName171;
            private char __DisplayName172;
            private char __DisplayName173;
            private char __DisplayName174;
            private char __DisplayName175;
            private char __DisplayName176;
            private char __DisplayName177;
            private char __DisplayName178;
            private char __DisplayName179;
            private char __DisplayName180;
            private char __DisplayName181;
            private char __DisplayName182;
            private char __DisplayName183;
            private char __DisplayName184;
            private char __DisplayName185;
            private char __DisplayName186;
            private char __DisplayName187;
            private char __DisplayName188;
            private char __DisplayName189;
            private char __DisplayName190;
            private char __DisplayName191;
            private char __DisplayName192;
            private char __DisplayName193;
            private char __DisplayName194;
            private char __DisplayName195;
            private char __DisplayName196;
            private char __DisplayName197;
            private char __DisplayName198;
            private char __DisplayName199;
            private char __DisplayName200;
            private char __DisplayName201;
            private char __DisplayName202;
            private char __DisplayName203;
            private char __DisplayName204;
            private char __DisplayName205;
            private char __DisplayName206;
            private char __DisplayName207;
            private char __DisplayName208;
            private char __DisplayName209;
            private char __DisplayName210;
            private char __DisplayName211;
            private char __DisplayName212;
            private char __DisplayName213;
            private char __DisplayName214;
            private char __DisplayName215;
            private char __DisplayName216;
            private char __DisplayName217;
            private char __DisplayName218;
            private char __DisplayName219;
            private char __DisplayName220;
            private char __DisplayName221;
            private char __DisplayName222;
            private char __DisplayName223;
            private char __DisplayName224;
            private char __DisplayName225;
            private char __DisplayName226;
            private char __DisplayName227;
            private char __DisplayName228;
            private char __DisplayName229;
            private char __DisplayName230;
            private char __DisplayName231;
            private char __DisplayName232;
            private char __DisplayName233;
            private char __DisplayName234;
            private char __DisplayName235;
            private char __DisplayName236;
            private char __DisplayName237;
            private char __DisplayName238;
            private char __DisplayName239;
            private char __DisplayName240;
            private char __DisplayName241;
            private char __DisplayName242;
            private char __DisplayName243;
            private char __DisplayName244;
            private char __DisplayName245;
            private char __DisplayName246;
            private char __DisplayName247;
            private char __DisplayName248;
            private char __DisplayName249;
            private char __DisplayName250;
            private char __DisplayName251;
            private char __DisplayName252;
            private char __DisplayName253;
            private char __DisplayName254;
            private char __DisplayName255;
            public DeviceRole Role;
            public WaveFormatExtensible.__Native OutputFormat;
            internal void __MarshalFree()
            {
                this.OutputFormat.__MarshalFree();
            }
        }
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_DETAILS::DeviceID']/*" />	
        /// <unmanaged>wchar_t DeviceID[256]</unmanaged>	
        /// <unmanaged-short>wchar_t DeviceID</unmanaged-short>	
        public string DeviceID;
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_DETAILS::DisplayName']/*" />	
        /// <unmanaged>wchar_t DisplayName[256]</unmanaged>	
        /// <unmanaged-short>wchar_t DisplayName</unmanaged-short>	
        public string DisplayName;
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_DETAILS::Role']/*" />	
        /// <unmanaged>XAUDIO2_DEVICE_ROLE Role</unmanaged>	
        /// <unmanaged-short>XAUDIO2_DEVICE_ROLE Role</unmanaged-short>	
        public DeviceRole Role;
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='XAUDIO2_DEVICE_DETAILS::OutputFormat']/*" />	
        /// <unmanaged>WAVEFORMATEXTENSIBLE OutputFormat</unmanaged>	
        /// <unmanaged-short>WAVEFORMATEXTENSIBLE OutputFormat</unmanaged-short>	
        public WaveFormatExtensible OutputFormat;
        internal void __MarshalFree(ref DeviceDetails.__Native @ref)
        {
            @ref.__MarshalFree();
        }
        internal unsafe void __MarshalFrom(ref DeviceDetails.__Native @ref)
        {
            fixed (char* ptr = &@ref.DeviceID)
            {
                this.DeviceID = Utilities.PtrToStringUni((IntPtr)((void*)ptr), 256);
            }
            fixed (char* ptr2 = &@ref.DisplayName)
            {
                this.DisplayName = Utilities.PtrToStringUni((IntPtr)((void*)ptr2), 256);
            }
            this.Role = @ref.Role;
            this.OutputFormat = new WaveFormatExtensible();
            this.OutputFormat.__MarshalFrom(ref @ref.OutputFormat);
        }
        internal unsafe void __MarshalTo(ref DeviceDetails.__Native @ref)
        {
            fixed (char* deviceID = this.DeviceID)
            {
                fixed (char* ptr = &@ref.DeviceID)
                {
                    Utilities.CopyMemory((IntPtr)((void*)ptr), (IntPtr)((void*)deviceID), this.DeviceID.Length * 2);
                }
            }
            fixed (char* displayName = this.DisplayName)
            {
                fixed (char* ptr2 = &@ref.DisplayName)
                {
                    Utilities.CopyMemory((IntPtr)((void*)ptr2), (IntPtr)((void*)displayName), this.DisplayName.Length * 2);
                }
            }
            @ref.Role = this.Role;
            @ref.OutputFormat = WaveFormatExtensible.__NewNative();
            this.OutputFormat.__MarshalTo(ref @ref.OutputFormat);
        }
    }
}