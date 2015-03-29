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
using SharpDX.Direct3D;

namespace SharpDX.Direct3D10
{
    public partial class Mesh
    {
        /// <summary>	
        /// Creates a mesh object using a declarator.	
        /// </summary>	
        /// <param name="device">Pointer to an <see cref="SharpDX.Direct3D10.Device"/>, the device object to be associated with the mesh. </param>
        /// <param name="elements">Array of <see cref="SharpDX.Direct3D10.InputElement"/> elements, describing the vertex format for the returned mesh. This parameter must map directly to a flexible vertex format (FVF). </param>
        /// <param name="positionElement">Semantic that identifies which part of the vertex declaration contains position information. </param>
        /// <param name="vertexCount">Number of vertices for the mesh. This parameter must be greater than 0. </param>
        /// <param name="faceCount">Number of faces for the mesh. The valid range for this number is greater than 0, and one less than the maximum DWORD (typically 65534), because the last index is reserved. </param>
        /// <param name="flags">Combination of one or more flags from the <see cref="MeshFlags"/>, specifying options for the mesh.  </param>
        /// <unmanaged>HRESULT D3DX10CreateMesh([None] ID3D10Device* pDevice,[In, Buffer] const D3D10_INPUT_ELEMENT_DESC* pDeclaration,[None] int DeclCount,[None] const char* pPositionSemantic,[None] int VertexCount,[None] int FaceCount,[None] int Options,[None] ID3DX10Mesh** ppMesh)</unmanaged>
        public Mesh(Device device, InputElement[] elements, string positionElement, int vertexCount, int faceCount, MeshFlags flags)
        {
            D3DX10.CreateMesh(device, elements, elements.Length, positionElement, vertexCount, faceCount, (int) flags, this);
        }

        /// <summary>	
        /// Creates a new mesh and fills it with the data of a previously loaded mesh.	
        /// </summary>	
        /// <param name="positionElement">The semantic name for the position data. </param>
        /// <param name="elements">Array of <see cref="InputElement"/> structures, describing the vertex format for the returned mesh. See <see cref="SharpDX.Direct3D10.InputElement"/>. </param>
        /// <param name="flags">Creation flags to be applied to the new mesh. See <see cref="MeshFlags"/>.</param>
        /// <returns>returns the Mesh cloned. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::CloneMesh([None] int Flags,[None] const char* pPosSemantic,[In, Buffer] const D3D10_INPUT_ELEMENT_DESC* pDesc,[None] int DeclCount,[None] ID3DX10Mesh** ppCloneMesh)</unmanaged>
        public Mesh Clone(InputElement[] elements, string positionElement, MeshFlags flags)
        {
            Mesh temp;
            CloneMesh((int) flags, positionElement, elements, elements.Length, out temp);
            return temp;
        }

        /// <summary>	
        /// Access the mesh's creation flags.	
        /// </summary>	
        /// <returns>The creation flags passed into the options parameter of <see cref="SharpDX.Direct3D10.D3DX10.CreateMesh"/> when the mesh was created. See {{D3DX10_MESH}}. </returns>
        /// <unmanaged>int ID3DX10Mesh::GetFlags()</unmanaged>
        public MeshFlags Flags
        {
            get
            {
                return (MeshFlags)GetFlags();
            }
        }

        /// <summary>	
        /// Access the mesh's attribute buffer.	
        /// </summary>	
        /// <returns>Returns the attribute buffer. See <see cref="SharpDX.Direct3D10.MeshBuffer"/>. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::GetAttributeBuffer([None] ID3DX10MeshBuffer** ppAttribute)</unmanaged>
        public MeshBuffer GetAttributeBuffer()
        {
            MeshBuffer temp;
            GetAttributeBuffer(out temp);
            return temp;
        }


        /// <summary>	
        /// Retrieves either an attribute table for a mesh, or the number of entries stored in an attribute table for a mesh.	
        /// </summary>	
        /// <remarks>	
        /// An attribute table is used to identify areas of the mesh that need to be drawn with different textures, render states, materials, and so on. In addition, the application can use the attribute table to hide portions of a mesh by not drawing a given attribute identifier when drawing the frame. 	
        /// </remarks>	
        /// <returns>Returns an array of <see cref="MeshAttributeRange"/> structures, representing the entries in the mesh's attribute table. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::GetAttributeTable([Out, Buffer, Optional] D3DX10_ATTRIBUTE_RANGE* pAttribTable,[None] int* pAttribTableSize)</unmanaged>
        public MeshAttributeRange[] GetAttributeTable()
        {            
            int sizeAttributeTable = 0;
            GetAttributeTable(null, ref sizeAttributeTable);
            var temp = new MeshAttributeRange[sizeAttributeTable];
            GetAttributeTable(temp, ref sizeAttributeTable);
            return temp;
        }

        /// <summary>	
        /// Get the mesh's point rep buffer.	
        /// </summary>	
        /// <returns>Returns a mesh buffer containing the mesh's point rep data. See <see cref="SharpDX.Direct3D10.MeshBuffer"/>. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::GetPointRepBuffer([None] ID3DX10MeshBuffer** ppPointReps)</unmanaged>
        public MeshBuffer GetPointRepresentationBuffer()
        {
            MeshBuffer temp;
            GetPointRepBuffer(out temp);
            return temp;
        }

        /// <summary>
        /// Optimizes the mesh data.
        /// </summary>
        /// <param name="flags">Flags indicating which optimizations to perform.</param>
        /// <returns>A result code.</returns>
        public Result Optimize(MeshOptimizeFlags flags)
        {
            Optimize((int) flags, null, IntPtr.Zero); 
            return Result.Ok;
        }

        /// <summary>	
        /// Generates a new mesh with reordered faces and vertices to optimize drawing performance.	
        /// </summary>	
        /// <remarks>	
        /// This method generates a new mesh. Before running Optimize, an application must generate an adjacency buffer by calling <see cref="SharpDX.Direct3D10.Mesh.GenerateAdjacencyAndPointRepresentation"/>. The adjacency buffer contains adjacency data, such as a list of edges and the faces that are adjacent to each other. This method is very similar to the <see cref="SharpDX.Direct3D10.Mesh.CloneMesh"/> method, except that it can perform optimization while generating the new clone of the mesh. The output mesh inherits all of the creation parameters of the input mesh. 	
        /// </remarks>	
        /// <param name="flags">Specifies the type of optimization to perform. This parameter can be set to a combination of one or more flags from D3DXMESHOPT and D3DXMESH (except D3DXMESH_32BIT, D3DXMESH_IB_WRITEONLY, and D3DXMESH_WRITEONLY). </param>
        /// <param name="faceRemap">An array of UINTs, one per face, that identifies the original mesh face that corresponds to each face in the optimized mesh.</param>
        /// <param name="vertexRemap">An array of index for each vertex that specifies how the new vertices map to the old vertices. This remap is useful if you need to alter external data based on the new vertex mapping. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::Optimize([None] int Flags,[Out, Buffer, Optional] int* pFaceRemap,[In] LPD3D10BLOB* ppVertexRemap)</unmanaged>
        public void Optimize(MeshOptimizeFlags flags, out int[] faceRemap, out int[] vertexRemap)
        {

            IntPtr blobPtr;
            DataStream dataStream = null;
            unsafe
            {
                try
                {
                    faceRemap = new int[FaceCount];
                    Optimize((int)flags, faceRemap, new IntPtr(&blobPtr));
                    dataStream = new DataStream(new Blob(blobPtr));
                    vertexRemap = dataStream.ReadRange<int>(VertexCount);
                    dataStream.Dispose();
                }
                catch (Exception)
                {
                    faceRemap = null;
                    vertexRemap = null;
                    if (dataStream!=null)
                        dataStream.Dispose();
                    throw;
                }
            }
        }

        /// <summary>	
        /// Set the mesh's adjacency data.	
        /// </summary>	
        /// <param name="data">The adjacency data to set </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetAdjacencyData([In] const int* pAdjacency)</unmanaged>
        public void SetAdjacencyData(DataStream data)
        {
            SetAdjacencyData(data.PositionPointer);
        }

        /// <summary>	
        /// Set the mesh's attribute data.	
        /// </summary>	
        /// <param name="data">The attribute data to set. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetAttributeData([In] const int* pData)</unmanaged>
        public void SetAttributeData(DataStream data)
        {
            SetAttributeData(data.PositionPointer);
        }

        /// <summary>
        /// Sets the attribute table for a mesh and the number of entries stored in the table.	
        /// </summary>	
        /// <remarks>	
        /// If an application keeps track of the information in an attribute table, and rearranges the table as a result of changes to attributes or faces, this method allows the application to update the attribute tables instead of calling ID3DX10Mesh::Optimize again. 	
        /// </remarks>	
        /// <param name="data">an array of <see cref="MeshAttributeRange"/> structures, representing the entries in the mesh attribute table. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetAttributeTable([In, Buffer] const D3DX10_ATTRIBUTE_RANGE* pAttribTable,[None] int cAttribTableSize)</unmanaged>
        public void SetAttributeTable(MeshAttributeRange[] data)
        {
            SetAttributeTable(data, data.Length);
        }

        /// <summary>	
        /// Set the mesh's index data.	
        /// </summary>	
        /// <param name="data">The index data. </param>
        /// <param name="count">The number of indices in pData. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetIndexData([None] const void* pData,[None] int cIndices)</unmanaged>
        public void SetIndexData(DataStream data, int count)
        {
            SetIndexData(data.PositionPointer, count);
        }

        /// <summary>	
        /// Set the point rep data for the mesh.	
        /// </summary>	
        /// <param name="data">The point rep data to set. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetPointRepData([None] const int* pPointReps)</unmanaged>
        public void SetPointRepresentationData(DataStream data)
        {
            SetPointRepData(data.PositionPointer);
        }

        /// <summary>	
        /// Set vertex data into one of the mesh's vertex buffers.	
        /// </summary>	
        /// <param name="index">The vertex buffer to be filled with pData. </param>
        /// <param name="data">The vertex data to set. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX10Mesh::SetVertexData([None] int iBuffer,[None] const void* pData)</unmanaged>
        public void SetVertexData(int index, DataStream data)
        {
            SetVertexData(index, data.PositionPointer);
        }
    }
}
