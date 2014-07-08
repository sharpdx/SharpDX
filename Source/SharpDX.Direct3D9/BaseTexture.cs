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

namespace SharpDX.Direct3D9
{
    public partial class BaseTexture
    {

        /// <summary>
        /// Filters mipmap levels of a texture.
        /// </summary>
        /// <param name="sourceLevel">The source level.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXFilterTexture([In] IDirect3DBaseTexture9* pBaseTexture,[In, Buffer] const PALETTEENTRY* pPalette,[In] unsigned int SrcLevel,[In] D3DX_FILTER Filter)</unmanaged>
        public void FilterTexture(int sourceLevel, Filter filter)
        {
            D3DX9.FilterTexture(this, null, sourceLevel, filter);
        }

        /// <summary>
        /// Filters mipmap levels of a texture.
        /// </summary>
        /// <param name="sourceLevel">The source level.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXFilterTexture([In] IDirect3DBaseTexture9* pBaseTexture,[In, Buffer] const PALETTEENTRY* pPalette,[In] unsigned int SrcLevel,[In] D3DX_FILTER Filter)</unmanaged>	
        public void FilterTexture(int sourceLevel, Filter filter, PaletteEntry[] palette)
        {
            D3DX9.FilterTexture(this, palette, sourceLevel, filter);
        }

        /// <summary>
        /// Saves a texture to a file.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveTextureToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DBaseTexture9* pSrcTexture,[In, Buffer] const PALETTEENTRY* pSrcPalette)</unmanaged>
        public static void ToFile(BaseTexture texture, string fileName, ImageFileFormat format)
        {
            D3DX9.SaveTextureToFileW(fileName, format, texture, null);
        }

        /// <summary>
        /// Saves a texture to a file.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXSaveTextureToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DBaseTexture9* pSrcTexture,[In, Buffer] const PALETTEENTRY* pSrcPalette)</unmanaged>
        public static void ToFile(BaseTexture texture, string fileName, ImageFileFormat format, PaletteEntry[] palette)
        {
            D3DX9.SaveTextureToFileW(fileName, format, texture, palette);            
        }

        /// <summary>
        /// Saves a texture to a stream.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="DataStream"/> containing the saved texture.</returns>
        /// <unmanaged>HRESULT D3DXSaveTextureToFileInMemory([Out] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DBaseTexture9* pSrcTexture,[In, Buffer] const PALETTEENTRY* pSrcPalette)</unmanaged>
        public static DataStream ToStream(BaseTexture texture, ImageFileFormat format)
        {
            return new DataStream(D3DX9.SaveTextureToFileInMemory(format, texture, null ));            
        }

        /// <summary>
        /// Saves a texture to a stream.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="format">The format.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="DataStream"/> containing the saved texture.</returns>
        /// <unmanaged>HRESULT D3DXSaveTextureToFileInMemory([Out] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DBaseTexture9* pSrcTexture,[In, Buffer] const PALETTEENTRY* pSrcPalette)</unmanaged>
        public static DataStream ToStream(BaseTexture texture, ImageFileFormat format, PaletteEntry[] palette)
        {
            return new DataStream(D3DX9.SaveTextureToFileInMemory(format, texture, palette));
        }


        /// <summary>
        /// Gets or sets the level of details.
        /// </summary>
        /// <value>
        /// The level of details.
        /// </value>
        /// <unmanaged>unsigned int IDirect3DBaseTexture9::GetLOD()</unmanaged>
        /// <unmanaged>unsigned int IDirect3DBaseTexture9::SetLOD([In] unsigned int LODNew)</unmanaged>
        public int LevelOfDetails
        {
            get
            {
                return GetLOD();
            }

            set
            {
                SetLOD(value);
            }
        }
    }
}