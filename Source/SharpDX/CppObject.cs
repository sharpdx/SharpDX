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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SharpDX
{
    /// <summary>
    /// Root class for all Cpp interop object.
    /// </summary>
    public partial class CppObject
    {
        /// <summary>
        /// The native pointer
        /// </summary>
        protected unsafe void* _nativePointer;

//        /// <summary>
//        ///   Static initializer.
//        /// </summary>
//        static CppObject()
//        {
//            // Generate Interop Assembly
//            // interopAssembly = DynamicInterop.Generate(_interopCalliSignatures, false, null);
////            interopAssembly = DynamicInterop.Generate(_interopCalliSignatures, true, ".");
//            // Bind AppDomain AssemblyResolve to return Interop Assembly
//            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
//        }

        /// <summary>
        ///   Fake Init Method. Force CppObject to initialize
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]        
        internal static void Init()
        {
            // Debug.WriteLineIf(interopAssembly == null, "Warning, interop assembly was not dynamically generated");
        }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        /// <param name = "pointer">Pointer to Cpp Object</param>
        public CppObject(IntPtr pointer)
        {
            NativePointer = pointer;
            //unsafe
            //{
            //    _nativePointer = (void*) pointer;
            //}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CppObject"/> class.
        /// </summary>
        protected CppObject()
        {
        }

        /// <summary>
        ///   Get a pointer to the underlying Cpp Object
        /// </summary>
        public IntPtr NativePointer
        {
            get
            {
                unsafe
                {
                    return (IntPtr) _nativePointer;
                }
            }
            set
            {
                unsafe
                {
                    void* newNativePointer = (void*) value;
                    if (_nativePointer != newNativePointer)
                    {
                        _nativePointer = newNativePointer;
                        NativePointerUpdated();
                    }
                }
            }
        }

        /// <summary>
        ///   Method that could be modified by inherited class
        /// </summary>
        protected virtual void NativePointerUpdated()
        {
        }


        //private static Regex matchAssemblyName = new Regex(@"^([^,]+)");

        ///// <summary>
        /////   Assembly Resolve callback
        ///// </summary>
        ///// <param name = "sender"></param>
        ///// <param name = "args"></param>
        ///// <returns></returns>
        //private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    string assemblyName = matchAssemblyName.Match(args.Name).Groups[0].Value;

        //    if (assemblyName.StartsWith(typeof(CppObject).Namespace + ".") && assemblyName.EndsWith(".Interop"))
        //    {
        //        string rootNameSpace = assemblyName.Substring(0, assemblyName.Length - ".Interop".Length);
        //        string signatureType = rootNameSpace + ".LocalInteropSignatures";

        //        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        //        {
        //            Type type = assembly.GetType(signatureType);
        //            if (type != null)
        //            {
        //                DynamicInterop.CalliSignature[] signatures = (DynamicInterop.CalliSignature[]) type.GetField("Signatures").GetValue(null);

        //                return DynamicInterop.Generate(rootNameSpace, signatures, false, null);
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}