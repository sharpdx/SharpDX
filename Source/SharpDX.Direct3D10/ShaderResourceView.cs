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
using System.IO;

namespace SharpDX.Direct3D10
{
    public partial class ShaderResourceView
    {
        /// <summary>
        ///   Creates a <see cref = "T:SharpDX.Direct3D10.ShaderResourceView" /> for accessing resource data.
        /// </summary>
        /// <param name = "device">The device to use when creating this <see cref = "T:SharpDX.Direct3D10.ShaderResourceView" />.</param>
        /// <param name = "resource">The resource that represents the render-target surface. This surface must have been created with the <see cref = "T:SharpDX.Direct3D10.BindFlags">ShaderResource</see> flag.</param>
        /// <unmanaged>ID3D10Device::CreateShaderResourceView</unmanaged>
        public ShaderResourceView(Device device, Resource resource)
            : base(IntPtr.Zero)
        {
            device.CreateShaderResourceView(resource, null, this);
        }

        /// <summary>
        ///   Creates a <see cref = "T:SharpDX.Direct3D10.ShaderResourceView" /> for accessing resource data.
        /// </summary>
        /// <param name = "device">The device to use when creating this <see cref = "T:SharpDX.Direct3D10.ShaderResourceView" />.</param>
        /// <param name = "resource">The resource that represents the render-target surface. This surface must have been created with the <see cref = "T:SharpDX.Direct3D10.BindFlags">ShaderResource</see> flag.</param>
        /// <param name = "description">A structure describing the <see cref = "T:SharpDX.Direct3D10.ShaderResourceView" /> to be created.</param>
        /// <unmanaged>ID3D10Device::CreateShaderResourceView</unmanaged>
        public ShaderResourceView(Device device, Resource resource, ShaderResourceViewDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateShaderResourceView(resource, description, this);
        }
        /// <summary>	
        /// Create a shader-resource view from a file. Read the characteristics of a texture when the texture is loaded.
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="fileName">Name of the file that contains the shader-resource view.</param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromFileW([None] ID3D10Device* pDevice,[None] const wchar_t* pSrcFile,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromFile(Device device, string fileName)
        {
            ShaderResourceView temp;
            Result hResult;
            D3DX10.CreateShaderResourceViewFromFile(device, fileName, null, IntPtr.Zero, out temp, out hResult);
            return temp;
        }

        /// <summary>	
        /// Create a shader-resource view from a file.	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="fileName">Name of the file that contains the shader-resource view.</param>
        /// <param name="loadInformation">Identifies the characteristics of a texture (see <see cref="SharpDX.Direct3D10.ImageLoadInformation"/>) when the data processor is created. </param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromFileW([None] ID3D10Device* pDevice,[None] const wchar_t* pSrcFile,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromFile(Device device, string fileName, ImageLoadInformation loadInformation)
        {
            ShaderResourceView temp;
            Result hResult;
            D3DX10.CreateShaderResourceViewFromFile(device, fileName, loadInformation, IntPtr.Zero, out temp, out hResult);
            return temp;
        }

        /// <summary>	
        /// Create a shader-resource view from a file in memory.	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="memory">Pointer to a memory location that contains the shader-resource view. </param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromMemory([None] ID3D10Device* pDevice,[None] const void* pSrcData,[None] SIZE_T SrcDataSize,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromMemory(Device device, byte[] memory)
        {
            unsafe
            {
                ShaderResourceView temp;
                Result hResult;
                fixed (void *pMemory = &memory[0])
                    D3DX10.CreateShaderResourceViewFromMemory(device, new IntPtr(pMemory), memory.Length, null, IntPtr.Zero, out temp, out hResult);
                // TODO test hResult?
                return temp;
            }
        }

        /// <summary>	
        /// Create a shader-resource view from a file in memory.	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="memory">Pointer to a memory location that contains the shader-resource view. </param>
        /// <param name="loadInformation">Identifies the characteristics of a texture (see <see cref="SharpDX.Direct3D10.ImageLoadInformation"/>) when the data processor is created. </param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromMemory([None] ID3D10Device* pDevice,[None] const void* pSrcData,[None] SIZE_T SrcDataSize,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromMemory(Device device, byte[] memory, ImageLoadInformation loadInformation)
        {
            unsafe
            {
                ShaderResourceView temp;
                Result hResult;
                fixed (void* pMemory = &memory[0])
                    D3DX10.CreateShaderResourceViewFromMemory(device, new IntPtr(pMemory), memory.Length, loadInformation, IntPtr.Zero, out temp, out hResult);
                return temp;
            }
        }

        /// <summary>	
        /// Create a shader-resource view from a file in a stream..	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="stream">Pointer to the file in memory that contains the shader-resource view. </param>
        /// <param name="sizeInBytes">Size of the file to read from the stream</param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromMemory([None] ID3D10Device* pDevice,[None] const void* pSrcData,[None] SIZE_T SrcDataSize,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromStream(Device device, Stream stream, int sizeInBytes)
        {
            byte[] memory = Utilities.ReadStream(stream, ref sizeInBytes);
            return FromMemory(device, memory);
        }

        /// <summary>	
        /// Create a shader-resource view from a file in a stream..	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will use the resource. </param>
        /// <param name="stream">Pointer to the file in memory that contains the shader-resource view. </param>
        /// <param name="sizeInBytes">Size of the file to read from the stream</param>
        /// <param name="loadInformation">Identifies the characteristics of a texture (see <see cref="SharpDX.Direct3D10.ImageLoadInformation"/>) when the data processor is created. </param>
        /// <returns>Returns a reference to the shader-resource view (see <see cref="SharpDX.Direct3D10.ShaderResourceView"/>). </returns>
        /// <unmanaged>HRESULT D3DX10CreateShaderResourceViewFromMemory([None] ID3D10Device* pDevice,[None] const void* pSrcData,[None] SIZE_T SrcDataSize,[In, Optional] D3DX10_IMAGE_LOAD_INFO* pLoadInfo,[None] ID3DX10ThreadPump* pPump,[None] ID3D10ShaderResourceView** ppShaderResourceView,[None] HRESULT* pHResult)</unmanaged>
        public static ShaderResourceView FromStream(Device device, Stream stream, int sizeInBytes, ImageLoadInformation loadInformation)
        {
            byte[] memory = Utilities.ReadStream(stream, ref sizeInBytes);
            return FromMemory(device, memory, loadInformation);
        }
   }
}