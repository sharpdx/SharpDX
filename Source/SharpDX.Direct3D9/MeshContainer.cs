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
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D9
{
    public partial class MeshContainer : DisposeBase
    {
        /// <summary>
        /// Gets or sets the materials.
        /// </summary>
        /// <value>
        /// The materials.
        /// </value>
        public unsafe ExtendedMaterial[] Materials
        {
            get
            {
                var materials = new ExtendedMaterial[MaterialCount];
                var materialsNative = new ExtendedMaterial.__Native[MaterialCount];
                if (MaterialCount > 0)
                {
                    Utilities.Read(MaterialPointer, materialsNative, 0, MaterialCount);
                    for (int i = 0; i < materialsNative.Length; i++)
                    {
                        var materialNative = materialsNative[i];
                        var material = new ExtendedMaterial();
                        material.__MarshalFrom(ref materialNative);
                        materials[i] = material;
                    }
                }
                return materials;
            }

            set
            {
                DisposeMaterials();

                MaterialCount = value.Length;
                MaterialPointer = Marshal.AllocHGlobal(Utilities.SizeOf<ExtendedMaterial.__Native>() * MaterialCount);

                for (int i = 0; i < value.Length; i++)
                {
                    value[i].__MarshalTo(ref ((ExtendedMaterial.__Native*)MaterialPointer)[i]);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            DisposeMaterials();
        }

        private unsafe void DisposeMaterials()
        {
            if (MaterialPointer != IntPtr.Zero)
            {
                for (int i = 0; i < MaterialCount; i++)
                {
                    ((ExtendedMaterial.__Native*)MaterialPointer)[i].__MarshalFree();
                }
                Marshal.FreeHGlobal(MaterialPointer);
                MaterialPointer = IntPtr.Zero;
                MaterialCount = 0;
            }
        }
    }
}