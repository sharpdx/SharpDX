// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace SharpDX.XAudio2
{  
    /// <summary>
    /// Defines speaker positions.
    /// </summary>
    [Flags]
    public enum SpeakerPosition : uint
    { 
         FrontLeft              = 0x1,
         FrontRight             = 0x2,
         FrontCenter            = 0x4,
         LowFrequency           = 0x8,
         BackLeft               = 0x10,
         BackRight              = 0x20,
         FrontLeftOfCenter      = 0x40,
         FrontRightOfCenter     = 0x80,
         BackCenter             = 0x100,
         SideLeft               = 0x200,
         SideRight              = 0x400,
         TopCenter              = 0x800,
         TopFrontLeft           = 0x1000,
         TopFrontCenter         = 0x2000,
         TopFrontRight          = 0x4000,
         TopBackLeft            = 0x8000,
         TopBackCenter          = 0x10000,
         TopBackRight           = 0x20000, 
         /// <summary>
         /// Bit mask locations reserved for future use 
         /// </summary>
         Reserved               = 0x7FFC0000, 
         /// <summary>
         ///  Used to specify that any possible permutation of speaker configurations
         /// </summary>
         All                    = 0x80000000
    }
     
    /// <summary>
    /// Defines common speaker positions.
    /// </summary>
    public enum CommonSpeakerPosition : uint
    { 
         Directout = 0, 
         Mono = SpeakerPosition.FrontCenter,
         Stereo = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight,  
         Quad = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|SpeakerPosition.BackLeft|SpeakerPosition.BackRight,
         Surround = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|SpeakerPosition.FrontCenter|SpeakerPosition.BackCenter,
         S5Point1 = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|
             SpeakerPosition.FrontCenter|SpeakerPosition.LowFrequency|
             SpeakerPosition.BackLeft|SpeakerPosition.BackRight,
         S7Point1 = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|
             SpeakerPosition.FrontCenter|SpeakerPosition.LowFrequency|
             SpeakerPosition.BackLeft|SpeakerPosition.BackRight|
             SpeakerPosition.FrontLeftOfCenter|SpeakerPosition.FrontRightOfCenter,
         S5Point1Surround = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|
             SpeakerPosition.FrontCenter|SpeakerPosition.LowFrequency|
             SpeakerPosition.SideLeft|SpeakerPosition.SideRight,
         S7Point1Surround = SpeakerPosition.FrontLeft|SpeakerPosition.FrontRight|
             SpeakerPosition.FrontCenter|SpeakerPosition.LowFrequency|
             SpeakerPosition.BackLeft|SpeakerPosition.BackRight|
             SpeakerPosition.SideLeft|SpeakerPosition.SideRight,
         S5Point1Back = CommonSpeakerPosition.S5Point1,
         S7Point1Wide = CommonSpeakerPosition.S7Point1, 
         DVDFrontLeft = SpeakerPosition.FrontLeft,
         DVDFronCenter = SpeakerPosition.FrontCenter,
         DVDFrontRight = SpeakerPosition.FrontRight,
         DVDRearLeft = SpeakerPosition.BackLeft,
         DVDRearRight = SpeakerPosition.BackRight,
         DVDTopMiddle = SpeakerPosition.TopCenter,
         DVDSuperWoofer = SpeakerPosition.LowFrequency 
    }
}
