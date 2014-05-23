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

namespace SharpGen.Model
{
    public class CsTypeBase : CsBase
    {
        public Type Type { get; set; }

        public bool IsReference { get; set; }

        public bool IsPointer
        {
            get { return Type == typeof (IntPtr); }
        }

        public bool IsBlittable
        {
            get { return (this is CsStruct || this is CsEnum || Type.IsValueType); }
        }

        /// <summary>
        /// Calculates the natural alignment of a type. -1 if it is a pointer alignment (4 on x86, 8 on x64)
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int CalculateAlignment()
        {
            if (Type == typeof(long) || Type == typeof(ulong) || Type == typeof(double))
            {
                return 8;
            }

            if (Type == typeof(int) || Type == typeof(uint) ||
                Type == typeof(float))
            {
                return 4;
            }

            if (Type == typeof(short) || Type == typeof(ushort) || Type == typeof(char))
            {
                return 2;
            }

            if (Type == typeof(byte) || Type == typeof(sbyte))
            {
                return 1;
            }

            if (IsPointer)
            {
                return -1;
            }

            // throws an exception?
            return 4;
        }

        private CsAssembly _assembly;
        public CsAssembly Assembly
        {
            get
            {
                if (_assembly == null)
                    _assembly = GetParent<CsAssembly>();
                return _assembly;
            }
        }

    }
}