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

namespace SharpDX.Direct2D1
{
    public partial class PrintControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintControl"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="wicFactory">The WIC factory.</param>
        /// <param name="documentTarget">The document target.</param>
        /// <unmanaged>HRESULT ID2D1Device::CreatePrintControl([In] IWICImagingFactory* wicFactory,[In] IPrintDocumentPackageTarget* documentTarget,[In, Optional] const D2D1_PRINT_CONTROL_PROPERTIES* printControlProperties,[Out] ID2D1PrintControl** printControl)</unmanaged>
        public PrintControl(Device device, SharpDX.WIC.ImagingFactory wicFactory, SharpDX.ComObject documentTarget)
        {
            device.CreatePrintControl(wicFactory, documentTarget, null, this);
        }

        /// <unmanaged>HRESULT ID2D1Device::CreatePrintControl([In] IWICImagingFactory* wicFactory,[In] IPrintDocumentPackageTarget* documentTarget,[In, Optional] const D2D1_PRINT_CONTROL_PROPERTIES* printControlProperties,[Out] ID2D1PrintControl** printControl)</unmanaged>
        public PrintControl(Device device, SharpDX.WIC.ImagingFactory wicFactory, SharpDX.ComObject documentTarget, SharpDX.Direct2D1.PrintControlProperties rintControlPropertiesRef)
        {
            device.CreatePrintControl(wicFactory, documentTarget, rintControlPropertiesRef, this);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>TBD</p>	
        /// </summary>	
        /// <param name="commandList">No documentation.</param>	
        /// <param name="pageSize">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <msdn-id>hh847997</msdn-id>	
        /// <unmanaged>HRESULT ID2D1PrintControl::AddPage([In] ID2D1CommandList* commandList,[In] D2D_SIZE_F pageSize,[In, Optional] IStream* pagePrintTicketStream,[Out, Optional] unsigned longlong* tag1,[Out, Optional] unsigned longlong* tag2)</unmanaged>	
        /// <unmanaged-short>ID2D1PrintControl::AddPage</unmanaged-short>	
        public void AddPage(SharpDX.Direct2D1.CommandList commandList, SharpDX.Size2F pageSize)
        {
            long tag1;
            long tag2;
            AddPage(commandList, pageSize, out tag1, out tag2);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>TBD</p>	
        /// </summary>	
        /// <param name="commandList">No documentation.</param>	
        /// <param name="pageSize">No documentation.</param>	
        /// <param name="tag1">No documentation.</param>	
        /// <param name="tag2">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <msdn-id>hh847997</msdn-id>	
        /// <unmanaged>HRESULT ID2D1PrintControl::AddPage([In] ID2D1CommandList* commandList,[In] D2D_SIZE_F pageSize,[In, Optional] IStream* pagePrintTicketStream,[Out, Optional] unsigned longlong* tag1,[Out, Optional] unsigned longlong* tag2)</unmanaged>	
        /// <unmanaged-short>ID2D1PrintControl::AddPage</unmanaged-short>	
        public void AddPage(SharpDX.Direct2D1.CommandList commandList, SharpDX.Size2F pageSize, out long tag1, out long tag2)
        {
            AddPage(commandList, pageSize, null, out tag1, out tag2);
        }
    }
}