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
    public partial class Output
    {
        /// <summary>	
        /// Find the display mode that most closely matches the requested display mode.	
        /// </summary>	
        /// <remarks>	
        /// Direct3D devices require UNORM formats. This method finds the closest matching available display mode to the mode specified in pModeToMatch. Similarly ranked fields (i.e. all specified, or all unspecified, etc) are resolved in the following order.  ScanlineOrdering Scaling Format Resolution RefreshRate  When determining the closest value for a particular field, previously matched fields are used to filter the display mode list choices, and  other fields are ignored. For example, when matching Resolution, the display mode list will have already been filtered by a certain ScanlineOrdering,  Scaling, and Format, while RefreshRate is ignored. This ordering doesn't define the absolute ordering for every usage scenario of FindClosestMatchingMode, because  the application can choose some values initially, effectively changing the order that fields are chosen. Fields of the display mode are matched one at a time, generally in a specified order. If a field is unspecified, FindClosestMatchingMode gravitates toward the values for the desktop related to this output.  If this output is not part of the desktop, then the default desktop output is used to find values. If an application uses a fully unspecified  display mode, FindClosestMatchingMode will typically return a display mode that matches the desktop settings for this output.   Unspecified fields are lower priority than specified fields and will be resolved later than specified fields. 	
        /// </remarks>	
        /// <param name="device">A reference to the Direct3D device interface. If this parameter is NULL, only modes whose format matches that of pModeToMatch will  be returned; otherwise, only those formats that are supported for scan-out by the device are returned. </param>
        /// <param name="modeToMatch">The desired display mode (see <see cref="SharpDX.DXGI.ModeDescription"/>). Members of DXGI_MODE_DESC can be unspecified indicating no preference for  that member.  A value of 0 for Width or Height indicates the value is unspecified.  If either Width or  Height are 0 both must be 0.  A numerator and denominator of 0 in RefreshRate indicate it is unspecified. Other members  of DXGI_MODE_DESC have enumeration values indicating the member is unspecified.  If pConnectedDevice is NULL Format cannot be DXGI_FORMAT_UNKNOWN. </param>
        /// <param name="closestMatch">The mode that most closely matches pModeToMatch. </param>
        /// <returns>Returns one of the following <see cref="SharpDX.DXGI.DXGIError"/>. </returns>
        /// <unmanaged>HRESULT IDXGIOutput::FindClosestMatchingMode([In] const DXGI_MODE_DESC* pModeToMatch,[Out] DXGI_MODE_DESC* pClosestMatch,[In, Optional] IUnknown* pConcernedDevice)</unmanaged>
        public void GetClosestMatchingMode(SharpDX.ComObject device, SharpDX.DXGI.ModeDescription modeToMatch, out SharpDX.DXGI.ModeDescription closestMatch)
        {
            FindClosestMatchingMode(ref modeToMatch, out closestMatch, device);
        }

        /// <summary>	
        /// Gets the display modes that match the requested format and other input options.	
        /// </summary>	
        /// <remarks>	
        /// In general, when switching from windowed to full-screen mode, a swap chain automatically chooses a display mode that meets (or exceeds) the resolution, color  depth and refresh rate of the swap chain. To exercise more control over the display mode, use this API to poll the set of display modes that are validated  against monitor capabilities, or all modes that match the desktop (if the desktop settings are not validated against the monitor). As shown, this API is designed to be called twice. First to get the number of modes available, and second to return a description of the modes. 	
        /// <code> UINT num = 0;	
        /// DXGI_FORMAT format = DXGI_FORMAT_R32G32B32A32_FLOAT;	
        /// UINT flags         = DXGI_ENUM_MODES_INTERLACED; pOutput-&gt;GetDisplayModeList( format, flags, &amp;num, 0); ... DXGI_MODE_DESC * pDescs = new DXGI_MODE_DESC[num];	
        /// pOutput-&gt;GetDisplayModeList( format, flags, &amp;num, pDescs); </code>	
        /// 	
        ///  	
        /// </remarks>	
        /// <param name="format">The color format (see <see cref="SharpDX.DXGI.Format"/>). </param>
        /// <param name="flags">format for modes to include (see {{DXGI_ENUM_MODES}}). DXGI_ENUM_MODES_SCALING needs to be specified to expose the display modes that require scaling.  Centered modes, requiring no  scaling and corresponding directly to the display output, are enumerated by default. </param>
        /// <returns>Returns a list of display modes (see <see cref="SharpDX.DXGI.ModeDescription"/>); </returns>
        /// <unmanaged>HRESULT IDXGIOutput::GetDisplayModeList([None] DXGI_FORMAT EnumFormat,[None] int Flags,[InOut] int* pNumModes,[Out, Buffer, Optional] DXGI_MODE_DESC* pDesc)</unmanaged>
        public ModeDescription[] GetDisplayModeList(Format format, DisplayModeEnumerationFlags flags)
        {
            int numberOfDisplayModes = 0;
            GetDisplayModeList(format, (int) flags, ref numberOfDisplayModes, null);
            var list = new ModeDescription[numberOfDisplayModes];
            if (numberOfDisplayModes > 0)
                GetDisplayModeList(format, (int) flags, ref numberOfDisplayModes, list);
            return list;
        }
    }
}