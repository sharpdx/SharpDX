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

namespace SharpDX.XAudio2.Fx
{
    /// <summary>
    /// XAudio2FxContants Functions.
    /// </summary>
    internal static class XAudio2FxContants
    {
        /// <summary>Constant None.</summary>
        internal static Guid CLSID_AudioReverb = new Guid("6a93130e-1d53-41d1-a9cf-e758800bb179");
        /// <summary>Constant None.</summary>
        internal static Guid CLSID_AudioReverb_Debug = new Guid("c4f82dd4-cb4e-4ce1-8bdb-ee32d4198269");
        /// <summary>Constant None.</summary>
        internal static Guid CLSID_AudioVolumeMeter = new Guid("cac1105f-619b-4d04-831a-44e1cbf12d57");
        /// <summary>Constant None.</summary>
        internal static Guid CLSID_AudioVolumeMeter_Debug = new Guid("2d9a0f9c-e67b-4b24-ab44-92b3e770c020");

        internal static Guid CLSID_IAudioProcessor = new Guid("a90bc001-e897-e897-55e4-9e4700000000");
    }
}