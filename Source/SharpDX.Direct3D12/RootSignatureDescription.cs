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
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RootSignatureDescription"/> class.
        /// </summary>
        public RootSignatureDescription() : this(RootSignatureFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootSignatureDescription"/> class.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="samplers">The samplers.</param>
        public RootSignatureDescription(RootSignatureFlags flags, RootParameter[] parameters = null, StaticSamplerDescription[] samplers = null)
        {
            Parameters = parameters;
            StaticSamplers = samplers;
            Flags = flags;
        }

        public RootSignatureDescription(IntPtr pNativePtr)
        {
            Deserialize(pNativePtr);
        }

        /// <summary>
        /// The parameters
        /// </summary>
        public RootParameter[] Parameters { get; set; }

        /// <summary>
        /// The static samplers.
        /// </summary>
        public StaticSamplerDescription[] StaticSamplers { get; set; }

        /// <summary>
        /// The flags
        /// </summary>
        public RootSignatureFlags Flags { get; set; }

        /// <summary>
        /// Serializes this description to a blob.
        /// </summary>
        /// <returns>A serialized version of this description.</returns>
        /// <exception cref="SharpDXException">If an error occured while serializing the description</exception>
        /// <msdn-id>dn859363</msdn-id>
        /// <unmanaged>HRESULT D3D12SerializeRootSignature([In] const void* pRootSignature,[In] D3D_ROOT_SIGNATURE_VERSION Version,[Out] ID3D10Blob** ppBlob,[Out, Optional] ID3D10Blob** ppErrorBlob)</unmanaged>	
        /// <unmanaged-short>D3D12SerializeRootSignature</unmanaged-short>	
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
        /// <msdn-id>dn859363</msdn-id>
        /// <unmanaged>HRESULT D3D12SerializeRootSignature([In] const void* pRootSignature,[In] D3D_ROOT_SIGNATURE_VERSION Version,[Out] ID3D10Blob** ppBlob,[Out, Optional] ID3D10Blob** ppErrorBlob)</unmanaged>	
        /// <unmanaged-short>D3D12SerializeRootSignature</unmanaged-short>	
        private unsafe Result Serialize(out Blob result, out string errorText)
        {
            Blob error;
            errorText = null;

            int length = 0;
            var rootParameters = (RootParameter.__Native* )0;
            var pinnedTables = new List<GCHandle>();
            try
            {
                if(Parameters != null)
                {
                    length = Parameters.Length;
                    rootParameters = (RootParameter.__Native*)Utilities.AllocateMemory(length * Utilities.SizeOf<RootParameter.__Native>());
                    for (int i = 0; i < length; i++)
                    {
                        rootParameters[i] = Parameters[i].native;
                        if(rootParameters[i].ParameterType == RootParameterType.DescriptorTable && Parameters[i].DescriptorTable != null)
                        {
                            var handle = GCHandle.Alloc(Parameters[i].DescriptorTable, GCHandleType.Pinned);
                            pinnedTables.Add(handle);
                            rootParameters[i].Union.DescriptorTable.DescriptorRangeCount = Parameters[i].DescriptorTable.Length;
                            rootParameters[i].Union.DescriptorTable.DescriptorRangesPointer = handle.AddrOfPinnedObject();
                        }
                    }
                }

                fixed (void* pSamplers = StaticSamplers)
                {
                    __Native native;
                    native.ParameterCount = length;
                    native.ParametersPointer = new IntPtr(rootParameters);
                    if(StaticSamplers != null)
                    {
                        native.StaticSamplerCount = StaticSamplers.Length;
                        native.StaticSamplerPointer = new IntPtr(pSamplers);
                    }
                    native.Flags = Flags;

                    var hresult = D3D12.SerializeRootSignature(new IntPtr(&native), RootSignatureVersion.Version1, out result, out error);
                    // TODO: check hresult or just rely on error?
                    if(error != null)
                    {
                        errorText = Utilities.BlobToString(error);
                    }
                    return hresult;
                }
            }
            finally
            {
                if(new IntPtr(rootParameters) != IntPtr.Zero)
                {
                    Utilities.FreeMemory(new IntPtr(rootParameters));
                }
                foreach(var handle in pinnedTables)
                {
                    handle.Free();
                }
            }
        }

        private unsafe void Deserialize(IntPtr pNativePtr)
        {
            if(pNativePtr == IntPtr.Zero)
            {
                return;
            }

            __Native* pNative = (__Native*)pNativePtr;

            if (pNative->ParameterCount > 0)
            {
                Parameters = new RootParameter[pNative->ParameterCount];
                RootParameter.__Native* rpn = (RootParameter.__Native * ) pNative->ParametersPointer;
                for (int i = 0; i < Parameters.Length; ++i)
                {
                    Parameters[i] = new RootParameter();
                    if (rpn[i].ParameterType == RootParameterType.DescriptorTable)
                    {
                        // Marshal descriptor table
                        DescriptorRange[] ranges = null;

                        int rangeCount = rpn[i].Union.DescriptorTable.DescriptorRangeCount;
                        if (rangeCount > 0)
                        {
                            ranges = new DescriptorRange[rangeCount];
                            Utilities.Read(rpn[i].Union.DescriptorTable.DescriptorRangesPointer, ranges, 0, ranges.Length);
                        }

                        Parameters[i] = new RootParameter(rpn[i].ShaderVisibility, ranges);
                    }
                    else
                    {
                        // No need to marshal them when RootParameter don't contain DescriptorTable - simple copy as-is
                        Parameters[i] = new RootParameter {native = *rpn};
                    }
                }
            }
             
            if (pNative->StaticSamplerCount > 0)
            {
                StaticSamplers = new StaticSamplerDescription[pNative->StaticSamplerCount];
                Utilities.Read(pNative->StaticSamplerPointer, StaticSamplers, 0, StaticSamplers.Length);
            }
        }

        internal partial struct __Native
        {
            /// <unmanaged-short>unsigned int NumParameters</unmanaged-short>	
            public int ParameterCount;

            /// <unmanaged-short>D3D12_ROOT_PARAMETER pParameters</unmanaged-short>	
            public System.IntPtr ParametersPointer;

            public int StaticSamplerCount;

            public System.IntPtr StaticSamplerPointer;

            /// <unmanaged-short>unsigned int Flags</unmanaged-short>	
            public RootSignatureFlags Flags;
        }
    } 
}