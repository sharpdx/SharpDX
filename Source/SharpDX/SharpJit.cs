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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpDX
{
    internal class SharpJit
    {

        private static byte[] clrCallToStdcallX86Prolog = new byte[]
                                                              {
//  00000	58		 pop	 eax
    0x58,
//  00001	89 01		 mov	 DWORD PTR [ecx], eax
    0x89, 0x01,
//  00003	64 a1 40 0e 00 00		 mov	 eax, DWORD PTR fs:3648
    0x64, 0xa1, 0x40, 0x0e, 0x00, 0x00,
//  00009	c6 40 08 00	 mov	 BYTE PTR [eax+8], 0
    0xc6, 0x40, 0x08, 0x00,
//  0000d	ff d2		 call	 edx
    0xff, 0xd2,
//  0000f	64 8b 0d 40 0e 00 00		 mov	 ecx, DWORD PTR fs:3648
    0x64, 0x8b, 0x0d, 0x40, 0x0e, 0x00, 0x00,
//  00016	c6 41 08 01	 mov	 BYTE PTR [ecx+8], 1
    0xc6, 0x41, 0x08, 0x01,
//  0001a	59		 pop	 ecx
    0x59,
// 0001b	ff 21		 jmp	 SHORT DWORD PTR [ecx]
    0xff, 0x21		 
                                                              };

        static SharpJit()
        {
            Install(typeof(SharpJit));
        }

        public static void Install(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
                Install(type);
        }

        public static void Install(Type typeToPatch)
        {
            unsafe
            {
                void* test;
                Console.WriteLine(new IntPtr(&test));
            }

            foreach (var method in typeToPatch.GetMethods())
            {
                var rawAttributes = method.GetCustomAttributes(typeof(ObfuscationAttribute), true) as ObfuscationAttribute[];
                if (rawAttributes.Length > 0)
                {
                    switch (rawAttributes[0].Feature)
                    {
                        case "SharpJit.ComMethod":
                            EmitTrampoline(method, clrCallToStdcallX86Prolog);
                            break;
                        case "SharpJit.Function":
                            EmitTrampoline(method, FunctionClrCallToStdcallX86Prolog);
                            break;
                    }
                }
            }
        }

        private static void EmitTrampoline(MethodInfo method, byte[] asmCode)
        {
            RuntimeHelpers.PrepareMethod(method.MethodHandle);
            var pointer = method.MethodHandle.GetFunctionPointer();
            Marshal.Copy(asmCode, 0, pointer, asmCode.Length);
        }

        private static byte[] FunctionClrCallToStdcallX86Prolog = new byte[]
                                                              {
                                                                  //00000	ff 21		 jmp ecx
                                                                  0xff, 0xe1,
                                                              };
    }
}

