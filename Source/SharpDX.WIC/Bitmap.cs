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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpDX.WIC
{
    public partial class Bitmap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pixelFormat">The pixel format. <see cref="PixelFormat"/> for a list of valid formats. </param>
        /// <param name="option">The option.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmap([In] unsigned int uiWidth,[In] unsigned int uiHeight,[In] const GUID&amp; pixelFormat,[In] WICBitmapCreateCacheOption option,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>
        public Bitmap(ImagingFactory factory, int width, int height, System.Guid pixelFormat, SharpDX.WIC.BitmapCreateCacheOption option) : base(IntPtr.Zero)
        {
            factory.CreateBitmap(width, height, pixelFormat, option, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a <see cref="Icon"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="icon">The icon.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromHICON([In] HICON hIcon,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>
        public Bitmap(ImagingFactory factory, Icon icon) : base(IntPtr.Zero)
        {
            factory.CreateBitmapFromHICON(icon.Handle, this);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="options">The options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromHBITMAP([In] HBITMAP hBitmap,[In, Optional] HPALETTE hPalette,[In] WICBitmapAlphaChannelOption options,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>
        public Bitmap(ImagingFactory factory, System.Drawing.Bitmap bitmap, SharpDX.WIC.BitmapAlphaChannelOption options) : base(IntPtr.Zero)
        {
            var hBitmap = bitmap.GetHbitmap();
            var hPalette = ConvertToHPALETTE(bitmap.Palette);
            try
            {
                factory.CreateBitmapFromHBITMAP(hBitmap, hPalette, options, this);
            }
            finally
            {
                DeleteObject(hBitmap);
                Marshal.FreeHGlobal(hPalette);
            }
        }

        private static IntPtr ConvertToHPALETTE(ColorPalette colorPalette)
        {
            IntPtr ptr = Marshal.AllocHGlobal((int) (4 * (2 + colorPalette.Entries.Length)));
            Marshal.WriteInt32(ptr, 0, colorPalette.Flags);
            Marshal.WriteInt32((IntPtr) (((long) ptr) + 4L), 0, colorPalette.Entries.Length);
            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                Marshal.WriteInt32((IntPtr) (((long) ptr) + (4 * (i + 2))), 0, colorPalette.Entries[i].ToArgb());
            }
            return ptr;
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}

