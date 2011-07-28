// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using Microsoft.Win32;

namespace SharpDX.XACT3
{
    public partial class Engine
    {
        private const string DebugEngineRegistryKey = "Software\\Microsoft\\XACT";
        private const string DebugEngineRegistryValue = "DebugEngine";

        public Engine() : this(CreationFlags.None)
        {
        }

        public Engine(CreationFlags creationFlags)
        {
            bool debug = (creationFlags == CreationFlags.DebugMode);
            bool audition = (creationFlags == CreationFlags.AuditionMode);

            var debugRegistryKey = Registry.LocalMachine.OpenSubKey(DebugEngineRegistryKey);

            // If neither the debug nor audition flags are set, see if the debug registry key is set
            if (!debug && !audition && debugRegistryKey != null)
            {
                var value = debugRegistryKey.GetValue(DebugEngineRegistryValue);

                if (value is Int32 && ((int) value) != 0)
                    debug = true;

                debugRegistryKey.Close();
            }            
            
            var selectedEngineCLSID = (debug) ? DebugEngineGuid : (audition) ? AuditionEngineGuid : EngineGuid;

            IntPtr temp;
            var result = Utilities.CoCreateInstance(selectedEngineCLSID, IntPtr.Zero, Utilities.CLSCTX.ClsctxInprocServer, typeof(Engine).GUID, out temp);
            result.CheckError();
            NativePointer = temp;            
        }
    }
}

