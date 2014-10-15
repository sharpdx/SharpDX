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
using SharpDX.Direct3D;

namespace SharpDX.Direct3D12
{
    public partial class RootSignatureDescription
    {
        //public  partial struct RootSignatureDescription {	
        //    /// <unmanaged-short>unsigned int NumParameters</unmanaged-short>	
        //    public int ParameterCount;
        //    /// <unmanaged-short>D3D12_ROOT_PARAMETER pParameters</unmanaged-short>	
        //    public System.IntPtr ParametersPointer;
        //    /// <unmanaged-short>unsigned int Flags</unmanaged-short>	
        //    public int Flags;
        //}

        public RootSignatureDescription(RootParameter[] parameters)
        {
            Parameters = new RootParameterArray(parameters);
        }

        private RootSignatureDescription(int count, IntPtr nativeParameters, RootSignatureFlags flags)
        {
            Parameters = new RootParameterArray(count, nativeParameters);
            Flags = flags;
        }

        /// <summary>
        /// The parameters
        /// </summary>
        public RootParameterArray Parameters;

        /// <summary>
        /// The flags
        /// </summary>
        public RootSignatureFlags Flags;

        public struct RootParameterArray
        {
            internal readonly IntPtr nativeParameters;
            internal readonly RootParameter[] managedParameters;
            internal readonly int count;

            internal RootParameterArray(RootParameter[] parameters)
            {
                count = 0;
                nativeParameters = IntPtr.Zero;
                managedParameters = parameters;
                if (parameters != null)
                {
                    count = parameters.Length;
                }
            }

            internal RootParameterArray(int count, IntPtr nativeParameters)
                : this()
            {
                this.count = count;
                this.nativeParameters = nativeParameters;
            }

            public int Count
            {
                get { return count; }
            }

            public unsafe RootParameter this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    if (managedParameters != null)
                    {
                        return managedParameters[index];
                    }
                    if (nativeParameters != IntPtr.Zero)
                    {
                        return ((RootParameter*)nativeParameters)[index];
                    }

                    return new RootParameter();
                }
                set
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    if (managedParameters != null)
                    {
                        managedParameters[index] = value;
                    }

                    if (nativeParameters != IntPtr.Zero)
                    {
                        ((RootParameter*)nativeParameters)[index] = value;
                    }
                }
            }

            public static implicit operator RootParameterArray(RootParameter[] fromArray)
            {
                return new RootParameterArray(fromArray);
            }
        }

        /// <summary>
        /// Serializes this description to a blob.
        /// </summary>
        /// <returns>A serialized version of this description.</returns>
        /// <exception cref="SharpDXException">If an error occured while serializing the description</exception>
        public Blob Serialize()
        {
            string errorText;
            Blob blob;
            var result = Serialize(out blob, out errorText);
            if(result.Failure)
            {
                throw new SharpDXException(result, errorText);
            }
            return blob;
        }

        /// <summary>
        /// Serializes this description to a blob.
        /// </summary>
        /// <returns>A serialized version of this description.</returns>
        private unsafe Result Serialize(out Blob result, out string errorText)
        {
            Blob error;
            errorText = null;
            fixed(void* pParameters = Parameters.managedParameters)
            {
                __Native native;
                native.ParameterCount = Parameters.Count;
                native.ParametersPointer = Parameters.nativeParameters;
                if(Parameters.managedParameters != null)
                {
                    native.ParametersPointer = (IntPtr)pParameters;
                }
                var hresult = D3D12.SerializeRootSignature(new IntPtr(&native), RootSignatureVersion.V1, out result, out error);
                // TODO: check hresult or just rely on error?
                if(error != null)
                {
                    errorText = Utilities.BlobToString(error);
                }
                return hresult;
            }
        }

        internal static RootSignatureDescription FromPointer(IntPtr nativePtr)
        {
            unsafe
            {
                var pNative = (__Native*)nativePtr;
                return new RootSignatureDescription(pNative->ParameterCount, pNative->ParametersPointer, pNative->Flags);
            }
        }

        internal partial struct __Native
        {
            /// <unmanaged-short>unsigned int NumParameters</unmanaged-short>	
            public int ParameterCount;

            /// <unmanaged-short>D3D12_ROOT_PARAMETER pParameters</unmanaged-short>	
            public System.IntPtr ParametersPointer;

            /// <unmanaged-short>unsigned int Flags</unmanaged-short>	
            public RootSignatureFlags Flags;
        }
    }
}