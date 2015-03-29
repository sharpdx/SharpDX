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

namespace SharpDX.DXGI
{
    public partial class Output1
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Gets the display modes that match the requested format and other input options.</p>	
        /// </summary>	
        /// <param name="enumFormat"><dd> <p>A <strong><see cref="SharpDX.DXGI.Format"/></strong>-typed value for the color format.</p> </dd></param>	
        /// <param name="flags"><dd> <p>A combination of DXGI_ENUM_MODES-typed values that are combined by using a bitwise OR operation. The resulting value specifies options for display modes to include. You must specify <see cref="SharpDX.DXGI.DisplayModeEnumerationFlags.Scaling"/> to expose the display modes that require scaling.  Centered modes that require no  scaling and correspond directly to the display output are enumerated by default.</p> </dd></param>	
        /// <returns>A list of display modes</returns>	
        /// <remarks>	
        /// <p><strong>GetDisplayModeList1</strong> is updated from  <strong>GetDisplayModeList</strong> to return a list of <strong><see cref="SharpDX.DXGI.ModeDescription1"/></strong> structures, which are updated mode descriptions.  <strong>GetDisplayModeList</strong> behaves as though it calls <strong>GetDisplayModeList1</strong> because  <strong>GetDisplayModeList</strong> can return all of the modes that are specified by DXGI_ENUM_MODES, including stereo mode.  However, <strong>GetDisplayModeList</strong> returns a list of <strong><see cref="SharpDX.DXGI.ModeDescription"/></strong> structures, which are the former mode descriptions and do not indicate stereo mode.</p><p>The <strong>GetDisplayModeList1</strong> method does not enumerate stereo modes unless you specify the <see cref="SharpDX.DXGI.DisplayModeEnumerationFlags.Stereo"/> flag in the <em>Flags</em> parameter.  If you specify <see cref="SharpDX.DXGI.DisplayModeEnumerationFlags.Stereo"/>, stereo modes are included in the list of returned modes that the <em>pDesc</em> parameter points to.  In other words, the method returns both stereo and mono modes.</p><p>In general, when you switch from windowed to full-screen mode, a swap chain automatically chooses a display mode that meets (or exceeds) the resolution, color  depth, and refresh rate of the swap chain. To exercise more control over the display mode, use <strong>GetDisplayModeList1</strong> to poll the set of display modes that are validated  against monitor capabilities, or all modes that match the desktop (if the desktop settings are not validated against the monitor).</p><p>The following example code shows that you need to call <strong>GetDisplayModeList1</strong> twice. First call <strong>GetDisplayModeList1</strong> to get the number of modes available, and second call <strong>GetDisplayModeList1</strong> to return a description of the modes.</p><pre><code> UINT num = 0;	
        /// <see cref="SharpDX.DXGI.Format"/> format = <see cref="SharpDX.DXGI.Format.R32G32B32A32_Float"/>;	
        /// UINT flags         = <see cref="SharpDX.DXGI.DisplayModeEnumerationFlags.Interlaced"/>; pOutput-&gt;GetDisplayModeList1( format, flags, &amp;num, 0); ... <see cref="SharpDX.DXGI.ModeDescription1"/> * pDescs = new <see cref="SharpDX.DXGI.ModeDescription1"/>[num];	
        /// pOutput-&gt;GetDisplayModeList1( format, flags, &amp;num, pDescs); </code></pre>	
        /// </remarks>	
        /// <msdn-id>hh404606</msdn-id>	
        /// <unmanaged>HRESULT IDXGIOutput1::GetDisplayModeList1([In] DXGI_FORMAT EnumFormat,[In] unsigned int Flags,[InOut] unsigned int* pNumModes,[Out, Buffer, Optional] DXGI_MODE_DESC1* pDesc)</unmanaged>	
        /// <unmanaged-short>IDXGIOutput1::GetDisplayModeList1</unmanaged-short>	
        public ModeDescription1[] GetDisplayModeList1(Format enumFormat, DisplayModeEnumerationFlags flags)
        {
            int numberOfDisplayModes = 0;
            GetDisplayModeList1(enumFormat, (int)flags, ref numberOfDisplayModes, null);
            var list = new ModeDescription1[numberOfDisplayModes];
            if (numberOfDisplayModes > 0)
                GetDisplayModeList1(enumFormat, (int)flags, ref numberOfDisplayModes, list);
            return list;
        }
    }
}