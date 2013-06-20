﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
#if !WIN8METRO
using System.Drawing;
using System.Drawing.Imaging;
#endif
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
        /// <msdn-id>ee690282</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmap([In] unsigned int uiWidth,[In] unsigned int uiHeight,[In] const GUID&amp; pixelFormat,[In] WICBitmapCreateCacheOption option,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreateBitmap</unmanaged-short>	
        public Bitmap(ImagingFactory factory, int width, int height, System.Guid pixelFormat, SharpDX.WIC.BitmapCreateCacheOption option)
            : base(IntPtr.Zero)
        {
            factory.CreateBitmap(width, height, pixelFormat, option, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a memory location using <see cref="DataRectangle"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="dataRectangle">The data rectangle.</param>
        /// <param name="totalSizeInBytes">Size of the buffer in <see cref="dataRectangle"/>. If == 0, calculate the size automatically based on the height and row pitch.</param>
        /// <msdn-id>ee690291</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromMemory([In] unsigned int uiWidth,[In] unsigned int uiHeight,[In] const GUID&amp; pixelFormat,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreateBitmapFromMemory</unmanaged-short>	
        public Bitmap(ImagingFactory factory, int width, int height, System.Guid pixelFormat, DataRectangle dataRectangle, int totalSizeInBytes = 0)
            : base(IntPtr.Zero)
        {
            if (totalSizeInBytes == 0)
                totalSizeInBytes = height*dataRectangle.Pitch;
            factory.CreateBitmapFromMemory(width, height, pixelFormat, dataRectangle.Pitch, totalSizeInBytes, dataRectangle.DataPointer, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a <see cref="BitmapSource"/>
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="bitmapSource">The bitmap source ref.</param>
        /// <param name="option">The option.</param>
        /// <msdn-id>ee690293</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromSource([In, Optional] IWICBitmapSource* pIBitmapSource,[In] WICBitmapCreateCacheOption option,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreateBitmapFromSource</unmanaged-short>	
        public Bitmap(ImagingFactory factory, SharpDX.WIC.BitmapSource bitmapSource, SharpDX.WIC.BitmapCreateCacheOption option)
            : base(IntPtr.Zero)
        {
            factory.CreateBitmapFromSource(bitmapSource, option, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="bitmapSource">The bitmap source.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <msdn-id>ee690294</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromSourceRect([In, Optional] IWICBitmapSource* pIBitmapSource,[In] unsigned int x,[In] unsigned int y,[In] unsigned int width,[In] unsigned int height,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreateBitmapFromSourceRect</unmanaged-short>	
        public Bitmap(ImagingFactory factory, SharpDX.WIC.BitmapSource bitmapSource, Rectangle rectangle)
            : base(IntPtr.Zero)
        {
            factory.CreateBitmapFromSourceRect(bitmapSource, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from an array of pixel datas.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="pixelDatas">The pixel datas.</param>
        /// <param name="stride">Stride of a row of pixels (number of bytes per row). By default the stride is == 0, and calculated by taking the sizeof(T) * width.</param>
        /// <msdn-id>ee690291</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateBitmapFromMemory([In] unsigned int uiWidth,[In] unsigned int uiHeight,[In] const GUID&amp; pixelFormat,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer,[Out, Fast] IWICBitmap** ppIBitmap)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreateBitmapFromMemory</unmanaged-short>	
        public unsafe static Bitmap New<T>(ImagingFactory factory, int width, int height, System.Guid pixelFormat, T[] pixelDatas, int stride = 0) where T : struct
        {
            if (stride == 0)
                stride = width * Utilities.SizeOf<T>();

            return new Bitmap(factory, width, height, pixelFormat, new DataRectangle((IntPtr)Interop.Fixed(pixelDatas), stride));
        }

        /// <summary>	
        /// <p>Provides access to a rectangular area of the bitmap.</p>	
        /// </summary>	
        /// <param name="flags"><dd>  <p>The access mode you wish to obtain for the lock. This is a bitwise combination of <strong><see cref="SharpDX.WIC.BitmapLockFlags"/></strong> for read, write, or read and write access.</p> <table> <tr><th>Value</th><th>Meaning</th></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.WIC.BitmapLockFlags.Read"/></strong></dt> </dl> </td><td> <p>The read access lock.</p> </td></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.WIC.BitmapLockFlags.Write"/></strong></dt> </dl> </td><td> <p>The write access lock.</p> </td></tr> </table> <p>?</p> </dd></param>	
        /// <returns><dd>  <p>A reference that receives the locked memory location.</p> </dd></returns>	
        /// <remarks>	
        /// <p>Locks are exclusive for writing but can be shared for reading. You cannot call <strong>CopyPixels</strong> while the <strong><see cref="SharpDX.WIC.Bitmap"/></strong> is locked for writing. Doing so will return an error, since locks are exclusive.</p>	
        /// </remarks>	
        /// <msdn-id>ee690187</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmap::Lock([In, Optional] const WICRect* prcLock,[In] WICBitmapLockFlags flags,[Out] IWICBitmapLock** ppILock)</unmanaged>	
        /// <unmanaged-short>IWICBitmap::Lock</unmanaged-short>	
        public SharpDX.WIC.BitmapLock Lock(SharpDX.WIC.BitmapLockFlags flags)
        {
            return Lock(IntPtr.Zero, flags);
        }

        /// <summary>	
        /// <p>Provides access to a rectangular area of the bitmap.</p>	
        /// </summary>	
        /// <param name="rcLockRef"><dd>  <p>The rectangle to be accessed.</p> </dd></param>	
        /// <param name="flags"><dd>  <p>The access mode you wish to obtain for the lock. This is a bitwise combination of <strong><see cref="SharpDX.WIC.BitmapLockFlags"/></strong> for read, write, or read and write access.</p> <table> <tr><th>Value</th><th>Meaning</th></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.WIC.BitmapLockFlags.Read"/></strong></dt> </dl> </td><td> <p>The read access lock.</p> </td></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.WIC.BitmapLockFlags.Write"/></strong></dt> </dl> </td><td> <p>The write access lock.</p> </td></tr> </table> <p>?</p> </dd></param>	
        /// <returns><dd>  <p>A reference that receives the locked memory location.</p> </dd></returns>	
        /// <remarks>	
        /// <p>Locks are exclusive for writing but can be shared for reading. You cannot call <strong>CopyPixels</strong> while the <strong><see cref="SharpDX.WIC.Bitmap"/></strong> is locked for writing. Doing so will return an error, since locks are exclusive.</p>	
        /// </remarks>	
        /// <msdn-id>ee690187</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmap::Lock([In, Optional] const WICRect* prcLock,[In] WICBitmapLockFlags flags,[Out] IWICBitmapLock** ppILock)</unmanaged>	
        /// <unmanaged-short>IWICBitmap::Lock</unmanaged-short>	
        public unsafe SharpDX.WIC.BitmapLock Lock(Rectangle rcLockRef, SharpDX.WIC.BitmapLockFlags flags)
        {
            rcLockRef.MakeXYAndWidthHeight();
            return this.Lock(new IntPtr(&rcLockRef), flags);
        }

#if !WIN8METRO
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
#endif
    }
}

