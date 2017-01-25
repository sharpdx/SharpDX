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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    /// <summary>
    /// D3DX constants and methods
    /// </summary>
    public static class D3DX
    {
        /// <summary>
        /// The value used to signify that the default value for a parameter should be used.
        /// </summary>
        /// <unmanaged>D3DX_DEFAULT</unmanaged>
        public const int Default = -1;

        /// <summary>
        /// The default value for non power-of-two textures.
        /// </summary>
        /// <unmanaged>D3DX_DEFAULT_NONPOW2</unmanaged>
        public const int DefaultNonPowerOf2 = -2;

        /// <summary>
        /// Indicates that the method should format from file.
        /// </summary>
        /// <unmanaged>D3DFMT_FROM_FILE</unmanaged>
        public const int FormatFromFile = -3;

        /// <summary>
        /// Indicates that the method should load from file.
        /// </summary>
        /// <unmanaged>D3DX_FROM_FILE</unmanaged>
        public const int FromFile = -3;

        /// <summary>
        /// Checks the D3DX runtime version against this compiled version.
        /// </summary>
        /// <returns>True if version are compatible</returns>
        /// <unmanaged>BOOL D3DXCheckVersion([In] unsigned int D3DSdkVersion,[In] unsigned int D3DXSdkVersion)</unmanaged>
        public static bool CheckVersion()
        {
            return D3DX9.CheckVersion(D3D9.SdkVersion, D3DX9.SdkVersion);
        }

        /// <summary>
        /// Get and set debug mute mode.
        /// </summary>
        /// <param name="mute">if set to <c>true</c> [mute].</param>
        /// <returns>Return the debug mute mode</returns>
        /// <unmanaged>BOOL D3DXDebugMute([In] BOOL Mute)</unmanaged>
        public static bool DebugMute(bool mute)
        {
            return D3DX9.DebugMute(mute);
        }

        /// <summary>
        /// Converts a declarator from a flexible vertex format (FVF) code.
        /// </summary>
        /// <param name="fvf">Combination of <see cref="VertexFormat"/> that describes the FVF from which to generate the returned declarator array..</param>
        /// <returns>
        /// A declarator from a flexible vertex format (FVF) code.
        /// </returns>
        /// <unmanaged>HRESULT D3DXDeclaratorFromFVF([In] D3DFVF FVF,[In, Buffer] D3DVERTEXELEMENT9* pDeclarator)</unmanaged>
        public static VertexElement[] DeclaratorFromFVF(VertexFormat fvf)
        {
            var vertices = new VertexElement[(int)VertexFormatDeclaratorCount.Max];

            var result = D3DX9.DeclaratorFromFVF(fvf, vertices);
            if (result.Failure)
                return null;

            var copy = new VertexElement[D3DX9.GetDeclLength(vertices) + 1];
            Array.Copy(vertices, copy, copy.Length);
            copy[copy.Length - 1] = VertexElement.VertexDeclarationEnd;
            return copy;
        }

        /// <summary>
        /// Converts a flexible vertex format (FVF) code from a declarator.
        /// </summary>
        /// <param name="declarator">The declarator array.</param>
        /// <returns>A <see cref="VertexFormat"/> that describes the vertex format returned from the declarator.</returns>
        /// <unmanaged>HRESULT D3DXFVFFromDeclarator([In, Buffer] const D3DVERTEXELEMENT9* pDeclarator,[Out] D3DFVF* pFVF)</unmanaged>
        public static VertexFormat FVFFromDeclarator(VertexElement[] declarator)
        {
            return D3DX9.FVFFromDeclarator(declarator);
        }

        /// <summary>
        /// Generates an output vertex declaration from the input declaration. The output declaration is intended for use by the mesh tessellation functions.
        /// </summary>
        /// <param name="declaration">The input declaration.</param>
        /// <returns>The output declaration</returns>
        /// <unmanaged>HRESULT D3DXGenerateOutputDecl([In, Buffer] D3DVERTEXELEMENT9* pOutput,[In, Buffer] const D3DVERTEXELEMENT9* pInput)</unmanaged>
        public static VertexElement[] GenerateOutputDeclaration(VertexElement[] declaration)
        {
            var vertices = new VertexElement[(int)VertexFormatDeclaratorCount.Max];

            var result = D3DX9.GenerateOutputDecl(vertices, declaration);
            if (result.Failure)
                return null;

            var copy = new VertexElement[D3DX9.GetDeclLength(vertices) + 1];
            Array.Copy(vertices, copy, copy.Length);
            copy[copy.Length - 1] = VertexElement.VertexDeclarationEnd;
            return copy;
        }

        /// <summary>
        /// Gets the number of elements in the vertex declaration.
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        /// <returns>The number of elements in the vertex declaration.</returns>
        /// <unmanaged>unsigned int D3DXGetDeclLength([In, Buffer] const D3DVERTEXELEMENT9* pDecl)</unmanaged>
        public static int GetDeclarationLength(VertexElement[] declaration)
        {
            return D3DX9.GetDeclLength(declaration);
        }

        /// <summary>
        /// Gets the size of a vertex from the vertex declaration.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The vertex declaration size, in bytes.</returns>
        /// <unmanaged>unsigned int D3DXGetDeclVertexSize([In, Buffer] const D3DVERTEXELEMENT9* pDecl,[In] unsigned int Stream)</unmanaged>
        public static int GetDeclarationVertexSize(VertexElement[] elements, int stream)
        {
            return D3DX9.GetDeclVertexSize(elements, stream);
        }

        /// <summary>
        /// Returns the size of a vertex for a flexible vertex format (FVF).
        /// </summary>
        /// <param name="fvf">The vertex format.</param>
        /// <returns>The FVF vertex size, in bytes.</returns>
        /// <unmanaged>unsigned int D3DXGetFVFVertexSize([In] D3DFVF FVF)</unmanaged>
        public static int GetFVFVertexSize(VertexFormat fvf)
        {
            return D3DX9.GetFVFVertexSize(fvf);
        }

        /// <summary>
        /// Gets the size of the rectangle patch.
        /// </summary>
        /// <param name="segmentCount">The segment count.</param>
        /// <param name="triangleCount">The triangle count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXRectPatchSize([In] const float* pfNumSegs,[In] unsigned int* pdwTriangles,[In] unsigned int* pdwVertices)</unmanaged>
        public static Result GetRectanglePatchSize(float segmentCount, out int triangleCount, out int vertexCount)
        {
            return D3DX9.RectPatchSize(segmentCount, out triangleCount, out vertexCount);
        }

        /// <summary>
        /// Gets the size of the triangle patch.
        /// </summary>
        /// <param name="segmentCount">The segment count.</param>
        /// <param name="triangleCount">The triangle count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXTriPatchSize([In] const float* pfNumSegs,[In] unsigned int* pdwTriangles,[In] unsigned int* pdwVertices)</unmanaged>
        public static Result GetTrianglePatchSize(float segmentCount, out int triangleCount, out int vertexCount)
        {
            return D3DX9.TriPatchSize(segmentCount, out triangleCount, out vertexCount);
        }

        /// <summary>
        /// Gets an array of <see cref="RawVector3"/> from a <see cref="DataStream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <param name="format">The format.</param>
        /// <returns>An array of <see cref="RawVector3"/> </returns>
        public static RawVector3[] GetVectors(DataStream stream, int vertexCount, VertexFormat format)
        {
            int stride = GetFVFVertexSize(format);
            return GetVectors(stream, vertexCount, stride);
        }

        /// <summary>
        /// Gets an array of <see cref="RawVector3"/> from a <see cref="DataStream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <param name="stride">The stride.</param>
        /// <returns>An array of <see cref="RawVector3"/> </returns>
        public static RawVector3[] GetVectors(DataStream stream, int vertexCount, int stride)
        {
            unsafe
            {
                var results = new RawVector3[vertexCount];
                for (int i = 0; i < vertexCount; i++)
                {
                    results[i] = stream.Read<RawVector3>();
                    stream.Position += stride - sizeof(RawVector3);
                }
                return results;
            }
        }

        /// <summary>
        /// Creates a FOURCC Format code from bytes description.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <param name="c3">The c3.</param>
        /// <param name="c4">The c4.</param>
        /// <returns>A Format FourCC</returns>
        /// <unmanaged>MAKEFOURCC</unmanaged>
        public static Format MakeFourCC(byte c1, byte c2, byte c3, byte c4)
        {
            return (Format)((((((c4 << 8) | c3) << 8) | c2) << 8) | c1);
        }

        /// <summary>
        /// Generates an optimized face remapping for a triangle list.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="faceCount">The face count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>The original mesh face that was split to generate the current face.</returns>
        /// <unmanaged>HRESULT D3DXOptimizeFaces([In] const void* pbIndices,[In] unsigned int cFaces,[In] unsigned int cVertices,[In] BOOL b32BitIndices,[In, Buffer] int* pFaceRemap)</unmanaged>
        public static int[] OptimizeFaces(short[] indices, int faceCount, int vertexCount)
        {
            unsafe
            {
                var faces = new int[faceCount];
                Result result;
                fixed (void* pIndices = indices)
                    result = D3DX9.OptimizeFaces((IntPtr)pIndices, faceCount, indices.Length, false, faces);
                if (result.Failure) return null;
                return faces;
            }
        }

        /// <summary>
        /// Generates an optimized vertex remapping for a triangle list. This function is commonly used after applying the face remapping generated by D3DXOptimizeFaces.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="faceCount">The face count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>The original mesh face that was split to generate the current face.</returns>
        /// <unmanaged>HRESULT D3DXOptimizeFaces([In] const void* pbIndices,[In] unsigned int cFaces,[In] unsigned int cVertices,[In] BOOL b32BitIndices,[In, Buffer] int* pFaceRemap)</unmanaged>
        public static int[] OptimizeFaces(int[] indices, int faceCount, int vertexCount)
        {
            unsafe
            {
                var faces = new int[faceCount];
                Result result;
                fixed (void* pIndices = indices)
                    result = D3DX9.OptimizeFaces((IntPtr)pIndices, faceCount, indices.Length, true, faces);
                if (result.Failure) return null;
                return faces;
            }
        }

        /// <summary>
        /// Generates an optimized vertex remapping for a triangle list. This function is commonly used after applying the face remapping generated by <see cref="OptimizeFaces(short[],int,int)"/>.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="faceCount">The face count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>A buffer that will contain the new index for each vertex. The value stored in pVertexRemap for a given element is the source vertex location in the new vertex ordering.</returns>
        /// <unmanaged>HRESULT D3DXOptimizeVertices([In] const void* pbIndices,[In] unsigned int cFaces,[In] unsigned int cVertices,[In] BOOL b32BitIndices,[In, Buffer] int* pVertexRemap)</unmanaged>
        public static int[] OptimizeVertices(short[] indices, int faceCount, int vertexCount)
        {
            unsafe
            {
                var vertices = new int[vertexCount];
                Result result;
                fixed (void* pIndices = indices)
                    result = D3DX9.OptimizeVertices((IntPtr)pIndices, faceCount, indices.Length, false, vertices);
                if (result.Failure) return null;
                return vertices;
            }
        }

        /// <summary>
        /// Generates an optimized vertex remapping for a triangle list. This function is commonly used after applying the face remapping generated by <see cref="OptimizeFaces(int[],int,int)"/>.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="faceCount">The face count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>A buffer that will contain the new index for each vertex. The value stored in pVertexRemap for a given element is the source vertex location in the new vertex ordering.</returns>
        /// <unmanaged>HRESULT D3DXOptimizeVertices([In] const void* pbIndices,[In] unsigned int cFaces,[In] unsigned int cVertices,[In] BOOL b32BitIndices,[In, Buffer] int* pVertexRemap)</unmanaged>
        public static int[] OptimizeVertices(int[] indices, int faceCount, int vertexCount)
        {
            unsafe
            {
                var vertices = new int[vertexCount];
                Result result;
                fixed (void* pIndices = indices)
                    result = D3DX9.OptimizeVertices((IntPtr)pIndices, faceCount, indices.Length, true, vertices);
                if (result.Failure) return null;
                return vertices;
            }
        }
    }
}
