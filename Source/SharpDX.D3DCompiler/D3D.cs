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
#if !DIRECTX11_1
using System;
using System.Runtime.InteropServices;

namespace SharpDX.D3DCompiler
{
    /// <summary>
    /// Warming, the following code is manually copied from generated code from Direct3D10 compiler.
    /// We need to access this method in order to compile Direct3D10 Effects with plain old D3D10CompileEffectFromMemory function.
    /// </summary>
    static partial class D3D  
    {
        private static IntPtr D3D10Handle_;

        private static IntPtr D3D10Handle
        {
            get
            {
                if (D3D10Handle_ == IntPtr.Zero)
                {
                    D3D10Handle_ = SharpDX.Utilities.LoadLibrary("d3d10.dll");
                }
                return D3D10Handle_;
            }
        }

        /// <summary>
        /// Compiles the effect10 from memory.
        /// </summary>
        /// <param name="dataRef">The data ref.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <param name="srcFileNameRef">The SRC file name ref.</param>
        /// <param name="definesRef">The defines ref.</param>
        /// <param name="includeRef">The include ref.</param>
        /// <param name="hlslFlags">The h LSL flags.</param>
        /// <param name="fxFlags">The f X flags.</param>
        /// <param name="compiledEffectOut">The compiled effect out.</param>
        /// <param name="errorsOut">The errors out.</param>
        /// <returns>Result code.</returns>
        /// <unmanaged>HRESULT D3D10CompileEffectFromMemory([In] void* pData,[In] SIZE_T DataLength,[In] const char* pSrcFileName,[In, Buffer, Optional] const D3D_SHADER_MACRO* pDefines,[In] ID3DInclude* pInclude,[In] D3DCOMPILE_SHADER_FLAGS HLSLFlags,[In] D3DCOMPILE_EFFECT_FLAGS FXFlags,[In] ID3D10Blob** ppCompiledEffect,[In] ID3D10Blob** ppErrors)</unmanaged>
        public static SharpDX.Result CompileEffect10FromMemory(
            System.IntPtr dataRef,
            SharpDX.PointerSize dataLength,
            string srcFileNameRef,
            SharpDX.Direct3D.ShaderMacro[] definesRef,
            System.IntPtr includeRef,
            SharpDX.D3DCompiler.ShaderFlags hlslFlags,
            SharpDX.D3DCompiler.EffectFlags fxFlags,
            out SharpDX.Direct3D.Blob compiledEffectOut,
            out SharpDX.Direct3D.Blob errorsOut)
        {
            unsafe
            {
                IntPtr srcFileNameRef_ = Marshal.StringToHGlobalAnsi(srcFileNameRef);
                SharpDX.Direct3D.ShaderMacro.__Native[] definesRef__ = (definesRef == null)
                                                                           ? null
                                                                           : new SharpDX.Direct3D.ShaderMacro.__Native[definesRef.Length];
                if (definesRef != null)
                {
                    for (int i = 0; i < definesRef.Length; i++)
                    {
                        definesRef[i].__MarshalTo(ref definesRef__[i]);
                    }
                }
                IntPtr compiledEffectOut_ = IntPtr.Zero;
                IntPtr errorsOut_ = IntPtr.Zero;
                SharpDX.Result __result__;
                fixed (void* definesRef_ = definesRef__)
                {
                    __result__ = SharpDX.D3DCompiler.LocalInterop.CalliFuncint(
                        (void*)dataRef,
                        (void*)dataLength,
                        (void*)srcFileNameRef_,
                        definesRef_,
                        (void*)includeRef,
                        unchecked((int)hlslFlags),
                        unchecked((int)fxFlags),
                        &compiledEffectOut_,
                        &errorsOut_,
                        (void*)D3D10CompileEffectFromMemory_);
                }
                Marshal.FreeHGlobal(srcFileNameRef_);
                if (definesRef != null)
                {
                    for (int i = 0; i < definesRef.Length; i++)
                    {
                        definesRef[i].__MarshalFree(ref definesRef__[i]);
                    }
                }
                compiledEffectOut = (compiledEffectOut_ == IntPtr.Zero) ? null : new SharpDX.Direct3D.Blob(compiledEffectOut_);
                errorsOut = (errorsOut_ == IntPtr.Zero) ? null : new SharpDX.Direct3D.Blob(errorsOut_);
                __result__.CheckError();
                return __result__;
            }
        }

        private static IntPtr D3D10CompileEffectFromMemoryField_;

        private static IntPtr D3D10CompileEffectFromMemory_
        {
            get
            {
                if (D3D10CompileEffectFromMemoryField_ == IntPtr.Zero)
                {
                    D3D10CompileEffectFromMemoryField_ = SharpDX.Utilities.GetProcAddress(D3D10Handle, "D3D10CompileEffectFromMemory");
                }
                return D3D10CompileEffectFromMemoryField_;
            }
        }
    }
}
#endif